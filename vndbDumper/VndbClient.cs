using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net.Security;
using Newtonsoft.Json;
using vndbDumper.Replay;
using vndbDumper.Model;
using System.Threading;
using System.Net;
using log4net;

namespace vndbDumper
{
    class VndbClient : IDisposable
    {
        public VndbClient()
        {
            logger = LogManager.GetLogger(GetType());

            logger.Info("Connecting to VNDB...");
            TcpClient tc = new TcpClient();
            tc.Connect("api.vndb.org", 19535);
            ssl = new SslStream(tc.GetStream(), false);
            ssl.AuthenticateAsClient("api.vndb.org");
            logger.Info("Connected successfully!");
        }

        private const double THROTTLE_BASE = 10000;

        private ILog logger;
        private WebClient webClient;
        private Random rng;
        private SslStream ssl;
        private BinaryWriter recorderStream;

        private byte[] SendRequest(string objName)
        {
            Throttle();
            byte[] buffer = Encoding.UTF8.GetBytes(objName);
            Array.Resize(ref buffer, buffer.Length + 1);
            buffer[buffer.Length - 1] = 0x04;

            if (recorderStream != null)
            {
                recorderStream.Write(FileFormatConstants.OPCODE_VNDB_REQUEST);
                recorderStream.Write(buffer.Length);
                recorderStream.Write(buffer, 0, buffer.Length);
            }

            ssl.Write(buffer, 0, buffer.Length);
            return ReadResponse();
        }

        private byte[] SendRequest(string objName, object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return SendRequest(String.Format("{0} {1}", objName, json));
        }

        private byte[] ReadResponse()
        {
            MemoryStream ms = new MemoryStream();
            byte[] buffer = new byte[1024];
            while (true)
            {
                Array.Clear(buffer, 0, 1024);
                int blockSize = ssl.Read(buffer, 0, 1024);
                if (blockSize > 0)
                {
                    ms.Write(buffer, 0, blockSize);
                    if (ContainsEOT(buffer))
                        break;
                }
                else
                {
                    break;
                }
            }

            buffer = ms.ToArray();

            if (recorderStream != null)
            {
                recorderStream.Write(FileFormatConstants.OPCODE_VNDB_RESPONSE);
                recorderStream.Write(buffer.Length);
                recorderStream.Write(buffer, 0, buffer.Length);
            }
            
            return buffer;
        }

        bool ContainsEOT(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == 0x04)
                    return true;
            }
            return false;
        }

        public void Dispose()
        {
            logger.Info("Shutting down connection to VNDB...");

            if (recorderStream != null)
            {
                recorderStream.Flush();
                recorderStream.Close();
                recorderStream.Dispose();
            }
            ssl.Close();
            ssl.Dispose();
        }

        public bool Login(string clientName, string clientVersion)
        {
            logger.Info(String.Format("Logging in for {0} {1}...", clientName, clientVersion));

            LoginRequest lr = new LoginRequest();
            lr.protocol = 1;
            lr.client = clientName;
            lr.clientver = clientVersion;
            byte[] buffer = SendRequest("login", lr);
            string result = Encoding.ASCII.GetString(buffer, 0, buffer.Length - 1);
            result = result.ToLowerInvariant();
            return result.Equals("ok");
        }

        public void BeginRecording()
        {
            if (recorderStream != null)
                throw new Exception("is already recording");

            FileInfo fi = new FileInfo("vndb.dmp");
            FileStream outStream;
            if (fi.Exists)
            {
                outStream = fi.Open(FileMode.Append, FileAccess.Write, FileShare.Read);
                recorderStream = new BinaryWriter(outStream);
                recorderStream.Write(FileFormatConstants.OPCODE_RESUME_RECORDING);
            }
            else
            {
                outStream = fi.OpenWrite();
                recorderStream = new BinaryWriter(outStream);
                recorderStream.Write(FileFormatConstants.MAGIC_NUMER);
                recorderStream.Write(FileFormatConstants.OPCODE_BEGIN_RECORDING);
            }
            recorderStream.Write(DateTime.Now.Ticks);
        }

        public Character[] GetCharacters(int vnId)
        {
            logger.Info(String.Format("Getting characters for {0}...", vnId));
            byte[] buffer = SendRequest(String.Format("get character basic,details,meas,traits,vns,voiced,instances (vn = {0})", vnId));
            int jsonOffset = FindJsonOffset(buffer);
            int jsonLength = buffer.Length - jsonOffset;
            jsonLength--;
            string json = Encoding.UTF8.GetString(buffer, jsonOffset, jsonLength);
            CharacterResults results = JsonConvert.DeserializeObject<CharacterResults>(json);
            return results.items;
        }

        public Release[] GetReleases(int vnId)
        {
            logger.Info(String.Format("Getting releases for {0}...", vnId));

            byte[] buffer = SendRequest(String.Format("get release basic,details,vn,producers (vn = {0})", vnId));
            int jsonOffset = FindJsonOffset(buffer);
            int jsonLength = buffer.Length - jsonOffset;
            jsonLength--;
            string json = Encoding.UTF8.GetString(buffer, jsonOffset, jsonLength);
            ReleaseResults results = JsonConvert.DeserializeObject<ReleaseResults>(json);
            return results.items;
        }

        public VisualNovel GetVisualNovel(int id)
        {
            logger.Info(String.Format("Getting VN {0}...", id));

            byte[] buffer = SendRequest(String.Format("get vn basic,details,anime,relations,tags,stats,screens,staff (id = {0})",id));
            int jsonOffset = FindJsonOffset(buffer);
            int jsonLength = buffer.Length - jsonOffset;
            jsonLength--;
            string json = Encoding.UTF8.GetString(buffer, jsonOffset, jsonLength);
            VisualNovelResults results = JsonConvert.DeserializeObject<VisualNovelResults>(json);
            return results.items[0];
        }

        private int FindJsonOffset(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] == (char)'{')
                    return i;
            }
            throw new Exception(Encoding.UTF8.GetString(buffer));
        }

        public void Throttle()
        {
            if (rng == null)
                rng = new Random();

            double multiplier = rng.NextDouble();
            multiplier += 0.5;

            double wait = THROTTLE_BASE * multiplier;
            int waitInt = (int)wait;

            Thread.Sleep(waitInt);            
        }

        public byte[] HttpRequest(string url)
        {
            Throttle();

            if (webClient == null)
                webClient = new WebClient();

            logger.Info(String.Format("Downloading {0}", url));

            byte[] data;
            try
            {
                data = webClient.DownloadData(url);
            }
            catch (Exception ex)
            {
                logger.Warn(String.Format("Download failed: {0} - {1}", url, ex.Message));
                data = Encoding.UTF8.GetBytes(url);
            }

            if (recorderStream != null)
            {
                byte[] urlBuffer = Encoding.UTF8.GetBytes(url);

                recorderStream.Write(FileFormatConstants.OPCODE_HTTP_OBJECT);
                recorderStream.Write(urlBuffer.Length);
                recorderStream.Write(urlBuffer, 0, urlBuffer.Length);
                recorderStream.Write(data.Length);
                recorderStream.Write(data, 0, data.Length);
            }
            return data;
        }
    }
}
