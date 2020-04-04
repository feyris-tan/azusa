using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NEbml.Core;

namespace moe.yo3explorer.azusa.Utilities
{
    public class ffmpegImageAcquisition : IImageAcquisitionPlugin
    {
        public Image Acquire()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckPathExists = true;
            ofd.Filter = "MatrosKa Video (*.mkv)|*.mkv";
            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return null;
            }
            
            FileInfo fileInfo = new FileInfo(ofd.FileName);
            if (!fileInfo.Exists)
                return null;

            FileStream fstream = fileInfo.OpenRead();
            EbmlReader ebmlReader = new EbmlReader(fstream);

            // https://code.acr.moe/kazari/winterchan/blob/09b64a289b30035024a202be06bf139346d4600e/inc/lib/webm/matroska-elements.txt
            while (ebmlReader.ReadNext())
            {
                VInt elementId = ebmlReader.ElementId;
                Console.WriteLine("{0:X} - {1:X}",elementId.EncodedValue,elementId.Value);
            }
            ebmlReader.Dispose();

            throw new NotImplementedException();
        }

        private bool? startable;
        private string ffmpegPath;
        private Process ffmpegProcess;

        public string Name => "Screenshot aus MKV anfertigen.";

        public bool CanStart()
        {
            if (startable.HasValue)
                return startable.Value;

            AzusaContext azusaContext = AzusaContext.GetInstance();
            ffmpegPath = azusaContext.ReadIniKey("ripkit", "ffmpegPath", null);
            if (string.IsNullOrEmpty(ffmpegPath))
            {
                startable = false;
                return false;
            }

            Process ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = ffmpegPath;
            ffmpeg.StartInfo.UseShellExecute = false;
            ffmpeg.StartInfo.RedirectStandardOutput = true;
            ffmpeg.StartInfo.RedirectStandardError = true;
            ffmpeg.Start();
            ffmpeg.WaitForExit();
            string line = ffmpeg.StandardError.ReadLine();
            if (line.StartsWith("ffmpeg version N"))
            {
                startable = true;
                return true;
            }
            int exitCode = ffmpeg.ExitCode;
            Console.WriteLine("ffmpeg exit code {0}", exitCode);
            startable = false;
            return false;
        }
    }
}
