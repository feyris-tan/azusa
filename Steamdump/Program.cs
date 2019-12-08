using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Steamdump.Logging;
using System.IO;
namespace Steamdump
{
    class Program
    {
        static void Main(string[] args)
        {
            TextWriterLogAdapter.LogCallback = new ConsoleMessageCallback();
            Steamdump sd = new Steamdump();

            if (File.Exists(args[0]))
            {
                sd.RestoreGame(new FileInfo(args[0]));
                return;
            }
            int requestedAppId;
            bool isInteger = Int32.TryParse(args[0], out requestedAppId);
            if (isInteger)
            {
                FileInfo outputFilename = new FileInfo(String.Format("{0}.azd", requestedAppId));
                List<SteamdumpGameMetadata> availableGames = sd.GetAvailableGames();
                SteamdumpGameMetadata targetGame = availableGames.FirstOrDefault(x => x.appId == requestedAppId);
                try
                {
                    sd.BackupGame(targetGame, outputFilename);
                }
                catch (FileAlreadyExistsException faee)
                {
                    Console.WriteLine("output file already exists!");
                }
            }
            else
            {
                Console.WriteLine("Don't know what to do.");
            }
        }
    }
}
