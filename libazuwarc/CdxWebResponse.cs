using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace libazuwarc
{
    public class CdxWebResponse : WebResponse
    {
        private const string WARC_TARGET_URI = "WARC-Target-URI: ";

        internal static CdxWebResponse Build(CdxEntry entry,Stream warcFile)
        {
            CdxWebResponse result = new CdxWebResponse();

            warcFile.Position = entry.Offset;
            GZipStream gz = new GZipStream(warcFile, CompressionMode.Decompress, true);
            BinaryReader br = new BinaryReader(gz);
            string srLine = br.ReadAsciiLine();
            while (!srLine.Equals(""))
            {
                srLine = br.ReadAsciiLine();
                if (srLine.StartsWith(WARC_TARGET_URI))
                {
                    result.responseUri = new Uri(srLine.Substring(WARC_TARGET_URI.Length));
                }
            }

            
            string httpStatusCode = br.ReadAsciiLine();

            while (true)
            {
                srLine = br.ReadAsciiLine();
                if (srLine.Equals(""))
                    break;
                int colonPos = srLine.IndexOf(':');
                string headerKey = srLine.Substring(0, colonPos);
                string headerValue = srLine.Substring(colonPos + 1);
                headerValue = headerValue.Trim();
                result.Headers.Add(headerKey, headerValue);
            }

            switch (result.TransferEncoding)
            {
                case "chunked":
                    result.responseStream = unchunk(gz);
                    gz.Dispose();
                    break;
                case null:
                    result.responseStream = gz;
                    break;
                default:
                    throw new NotImplementedException(result.TransferEncoding);
            }
            
            return result;
        }

        private static MemoryStream unchunk(Stream input)
        {
            MemoryStream output = new MemoryStream();
            BinaryReader br = new BinaryReader(input);
            int chunkSize = 0;
            do
            {
                string hexLine = br.ReadAsciiLine();
                chunkSize = Int32.Parse(hexLine, NumberStyles.HexNumber);
                byte[] buffer = br.ReadBytes(chunkSize);
                output.Write(buffer, 0, chunkSize);
                br.ReadByte();
                br.ReadByte();
            } while (chunkSize > 0);

            output.Position = 0;
            return output;
        }

        private WebHeaderCollection headers;
        public override WebHeaderCollection Headers
        {
            get
            {
                if (headers == null)
                    headers = new WebHeaderCollection();
                return headers;
            }
        }

        public override long ContentLength
        {
            get
            {
                if (Headers.AllKeys.Contains("Content-Length"))
                    return Convert.ToInt64(Headers.Get("Content-Length"));
                else
                    return -1;
            }
        }

        public override bool SupportsHeaders => true;

        public override string ContentType => Headers.Get("Content-Type");

        public override bool IsFromCache => true;

        public override bool IsMutuallyAuthenticated => false;

        private Uri responseUri;
        public override Uri ResponseUri => responseUri;

        public override string ToString()
        {
            return responseUri.ToString();
        }

        private Stream responseStream;
        public override Stream GetResponseStream()
        {
            return responseStream;
        }

        public string TransferEncoding => Headers.Get("Transfer-Encoding");
    }
}
