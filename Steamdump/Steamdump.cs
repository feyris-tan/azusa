using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Xml.Serialization;
using Ionic.Zip;
using Steamdump.Logging;

namespace Steamdump
{
    public class Steamdump
    {
        public List<SteamdumpGameMetadata> GetAvailableGames()
        {
            List<SteamdumpGameMetadata> result = new List<SteamdumpGameMetadata>();
            result.AddRange(GetGamesFromLibraryFolder(GetLibraryPath()));
            return result;
        }

        private static DirectoryInfo GetLibraryPath()
        {
            string steamPath = GetSteamPath();
            steamPath = Path.Combine(steamPath, "SteamApps", "libraryfolders.vdf");

            string[] libraryFoldersVdf = File.ReadAllLines(steamPath);
            for (int i = 4; i < libraryFoldersVdf.Length; i++)
            {
                string[] vdfLineArgs = libraryFoldersVdf[i].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (vdfLineArgs[0].StartsWith("\""))
                {
                    vdfLineArgs[0] = vdfLineArgs[0].Replace("\"", "");
                    vdfLineArgs[1] = vdfLineArgs[1].Replace("\"", "");
                    DirectoryInfo di = new DirectoryInfo(vdfLineArgs[1]);
                    di = new DirectoryInfo(Path.Combine(di.FullName, "steamapps"));
                    return di;
                }
                else
                {
                    DirectoryInfo di = new FileInfo(steamPath).Directory;
                    di = new DirectoryInfo(Path.Combine(di.FullName, "common"));
                    return di;
                }
            }
            throw new AccessViolationException();
        }

        private static string GetSteamPath()
        {
            RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
            rk = rk.OpenSubKey("Software");
            rk = rk.OpenSubKey("Valve");
            rk = rk.OpenSubKey("Steam");
            string steamPath = rk.GetValue("SteamPath", "-1").ToString();
            steamPath = steamPath.Replace('/', '\\');
            return steamPath;
        }

        private List<SteamdumpGameMetadata> GetGamesFromLibraryFolder(DirectoryInfo di)
        {
            List<SteamdumpGameMetadata> result = new List<SteamdumpGameMetadata>();
            foreach (FileInfo fi in di.GetFiles("*.acf"))
            {
                int appId = -1;
                string name = null;
                string installdir = null;
                string[] acfLines = File.ReadAllLines(fi.FullName);
                foreach (string acfLine in acfLines)
                {
                    if (acfLine.Contains("\"appid\""))
                    {
                        string acfArg = acfLine.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                        appId = Convert.ToInt32(acfArg);
                    }
                    if (acfLine.Contains("\"name\""))
                    {
                        name = acfLine.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                    }
                    if (acfLine.Contains("\"installdir\""))
                    {
                        installdir = acfLine.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                    }
                    if (appId != -1 && name != null && installdir != null)
                        break;
                }
                DirectoryInfo gameRoot = new DirectoryInfo(Path.Combine(di.FullName, "common", installdir));
                SteamdumpGameMetadata sdgm = new SteamdumpGameMetadata(appId, name, fi, gameRoot);
                result.Add(sdgm);
            }
            return result;
        }

        public void BackupGame(SteamdumpGameMetadata input, FileInfo outputFile)
        {
            if (outputFile.Exists)
            {
                throw new FileAlreadyExistsException(outputFile.FullName);
            }
            XmlCommentary xc = new XmlCommentary();
            xc.magic = "Azusa Steam Dumper";
            xc.acfName = input.acfFile.Name;
            xc.acfContent = File.ReadAllText(input.acfFile.FullName);
            XmlSerializer xs = new XmlSerializer(xc.GetType());
            StringWriter sw = new StringWriter();
            xs.Serialize(sw, xc);

            TextWriterLogAdapter twla = new TextWriterLogAdapter();
            ZipFile zip = new ZipFile(outputFile.FullName, twla, Encoding.UTF8);

            DirectoryInfo gameRoot = input.rootDirectory;
            FileInfo[] gameFiles = gameRoot.GetFiles("*", SearchOption.AllDirectories);
            string cutoff = gameRoot.FullName;
            zip.SaveProgress += (sender, e) => { TextWriterLogAdapter.LogCallback.SetProgress(e); };
            zip.AddDirectory(gameRoot.FullName);
            zip.Comment = sw.ToString();
            zip.Save();
        }

        public void RestoreGame(FileInfo inputFile)
        {
            TextWriterLogAdapter twla = new TextWriterLogAdapter();
            ZipFile zipFile = new ZipFile(inputFile.FullName, twla, Encoding.UTF8);
            zipFile.ExtractProgress += (sender, e) => { TextWriterLogAdapter.LogCallback.SetProgress(e); };

            StringReader sr = new StringReader(zipFile.Comment);
            XmlSerializer xs = new XmlSerializer(typeof(XmlCommentary));
            XmlCommentary xml = (XmlCommentary)xs.Deserialize(sr);
            if (!xml.magic.StartsWith("Azusa Steam"))
                throw new ArgumentException(nameof(inputFile));
            
            string name = null;
            string installdir = null;
            StringReader acfReader = new StringReader(xml.acfContent);

            do
            {
                string acfLine = acfReader.ReadLine();
                if (string.IsNullOrEmpty(acfLine))
                    break;
                if (acfLine.Contains("\"name\""))
                {
                    name = acfLine.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                }
                if (acfLine.Contains("\"installdir\""))
                {
                    installdir = acfLine.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[1].Replace("\"", "");
                }
                if (name != null && installdir != null)
                    break;
            } while (true);

            DirectoryInfo libPath = GetLibraryPath();
            DirectoryInfo zipExtractOutputDir = new DirectoryInfo(Path.Combine(libPath.FullName, installdir));
            zipFile.ExtractAll(zipExtractOutputDir.FullName, ExtractExistingFileAction.DoNotOverwrite);

            DirectoryInfo acfOutputDir = libPath.Parent;
            FileInfo acfFile = new FileInfo(Path.Combine(acfOutputDir.FullName, xml.acfName));
            File.WriteAllText(acfFile.FullName, xml.acfContent);
        }
    }
}