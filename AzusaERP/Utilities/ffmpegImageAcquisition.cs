using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using File = System.IO.File;

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


            TagLib.Matroska.File file = new TagLib.Matroska.File(fileInfo.FullName);
            double totalDuration = file.Properties.Duration.TotalSeconds;
            double tenPercent = totalDuration * 0.1;
            double startOffset = tenPercent;
            double endoffset = totalDuration - tenPercent;
            int second = context.RandomNumberGenerator.Next((int)startOffset, (int)endoffset);

            string tempFileName = Path.Combine(Path.GetTempPath(),String.Format("{0}.png",context.RandomNumberGenerator.Next()));
            string ffmpeg_params = String.Format("-i \"{0}\" -ss {1} -vframes 1 -aspect 16:9 \"{2}\"", fileInfo.FullName, second, tempFileName);
            Process ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = ffmpegPath;
            ffmpeg.StartInfo.Arguments = ffmpeg_params;
            ffmpeg.Start();
            ffmpeg.WaitForExit();

            byte[] imageBytes = File.ReadAllBytes(tempFileName);
            File.Delete(tempFileName);
            MemoryStream ms = new MemoryStream(imageBytes);
            return Image.FromStream(ms);
        }

        private bool? startable;
        private string ffmpegPath;
        private AzusaContext context;

        public string Name => "Screenshot aus MKV anfertigen.";

        public bool CanStart()
        {
            if (startable.HasValue)
                return startable.Value;

            context = AzusaContext.GetInstance();
            ffmpegPath = context.ReadIniKey("ripkit", "ffmpegPath", null);
            if (string.IsNullOrEmpty(ffmpegPath))
            {
                Console.WriteLine("ffmpegPath in category ripkit is not set!");
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
