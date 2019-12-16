using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AzusaERP;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.Control.DatabaseIO.Drivers;
using moe.yo3explorer.azusa.Control.DatabaseIO.Migrations;
using moe.yo3explorer.azusa.Control.Licensing;
using moe.yo3explorer.azusa.Control.Setup;
using Renci.SshNet;

namespace moe.yo3explorer.azusa
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            AzusaContext context = AzusaContext.GetInstance();

            if (args.Length != 0)
            {
                switch (args[0])
                {
                    case "--launcher":
                        context.TabletMode = true;
                        break;
                    case "--setup":
                        break;
                    case "--upgrade1to2":
                        Program p = new Program();
                        p.context = AzusaContext.GetInstance();
                        p.CreateSplashThread();
                        p.context.Ini = new Ini("azusa.ini");
                        p.ConnectOnline();
                        p.context.Splash.InvokeClose();
                        Migration1to2.Migrate();
                        return;
                    default:
                        return;
                }
            }

            Program program = new Program();

            try
            {
                program.Run(args);
            }
            catch (StartupFailedException e)
            {
                context.Splash.InvokeClose();
                DialogResult dialogResult = MessageBox.Show(
                    "Die Azusa-Applikation konnte nicht gestartet werden. Soll das Setup-Werkzeug ausgeführt werden?",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (dialogResult == DialogResult.Yes)
                {
                    SetupForm setupForm = new SetupForm(e);
                    dialogResult = setupForm.ShowDialog();
                }
            }
            
            context.DestroyContext();
        }

        private AzusaContext context;

        private void Run(string[] args)
        {
            if (PrepareRun())
            {
                throw new StartupFailedException(StartupFailReason.PrepareRunReturnedTrue);
            }
            context.MainForm.ShowDialog();
        }

        private bool PrepareRun()
        {
            context = AzusaContext.GetInstance();
            CreateSplashThread();

            context.Splash.SetLabel("Lade Icon...");
            string procFileName = Process.GetCurrentProcess().MainModule.FileName;
            Icon icon = Icon.ExtractAssociatedIcon(procFileName);
            context.Icon = icon;

            context.Splash.SetLabel("Starte Webserver...");
            context.WebServer = new WebServer();

            context.Splash.SetLabel("Lade Konfigurationsdatei...");
            FileInfo exePath = new FileInfo(Assembly.GetEntryAssembly().Location);
            FileInfo iniPath = new FileInfo(Path.Combine(exePath.Directory.FullName, "azusa.ini"));
            if (!iniPath.Exists)
            {
                throw new StartupFailedException(StartupFailReason.AzusaIniNotFound);
            }

            context.Ini = new Ini("azusa.ini");

            context.Splash.SetLabel("Überprüfe Verfügbarkeit der Online-Datenbank...");
            bool available = false;
            try
            {
                if (Convert.ToInt32(context.Ini["azusa"]["forceOffline"]) == 0)
                {
                    Ping ping = new Ping();
                    PingReply pong = ping.Send(context.Ini["postgresql"]["server"]);
                    if (pong.Status == IPStatus.Success)
                    {
                        available = TcpProbe(context.Ini["postgresql"]["server"],
                            Convert.ToInt16(context.Ini["postgresql"]["port"]));
                    }

                    if (!available)
                    {
                        if (context.Ini.ContainsKey("sshProxy"))
                        {
                            if (Convert.ToInt32(context.Ini["sshProxy"]["allow"]) > 0)
                            {
                                available = AttemptSshPortForward();
                            }
                        }
                    }
                }
            }
            catch
            {
                available = false;
            }

            if (available)
                try
                {
                    ConnectOnline();
                }
                catch (DatabaseConnectionException e)
                {
                    context.Splash.SetLabel("");
                    DialogResult askOffline =
                        MessageBox.Show(
                            String.Format(
                                "Die Verbindung zur Datenbank ist fehlgeschlagen. Soll offline gearbeitet werden?\n{0}", e),
                            "Azusa", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                    if (askOffline == DialogResult.Yes)
                    {
                        ConnectOffline();
                    }
                    else
                    {
                        context.Splash.InvokeClose();
                        return true;
                    }
                }
            else
            {
                ConnectOffline();
            }

            if (context.DatabaseDriver == null)
            {
                throw new StartupFailedException(StartupFailReason.NoDatabaseAvailable);
            }

            context.Splash.SetLabel("Erstelle Hauptfenster...");
            context.MainForm = new MainForm();
            context.Splash.SetLabel("Lade Module...");
            context.MainForm.ModuleScan();
            context.MainForm.Icon = icon;

            context.Splash.SetLabel("Lade Plug-Ins...");
            LoadPlugins(context);

            context.Splash.InvokeClose();
            return false;
        }

        private void CreateSplashThread()
        {
            Thread splashthread = new Thread(ShowSplash);
            splashthread.Name = "Splash Screen";
            splashthread.Priority = ThreadPriority.Lowest;
            splashthread.Start();
            while (context.Splash == null)
                Thread.Sleep(10);
            context.Splash.WaitForHandle();
        }

        private void LoadPlugins(AzusaContext context)
        {
            if (!context.Ini.ContainsKey("plugins"))
                return;

            Type pluginType = typeof(AzusaPlugin);
            IniSection pluginSection = context.Ini["plugins"];
            foreach (KeyValuePair<string, string> keyValuePair in pluginSection)
            {
                if (!File.Exists(keyValuePair.Value))
                {
                    context.Splash.SetLabel(String.Format("Datei nicht gefunden: " + keyValuePair.Value));
                    continue;
                }

                string path = new FileInfo(keyValuePair.Value).FullName;

                Assembly assembly = Assembly.LoadFile(path);
                Type[] exportedTypes = assembly.GetExportedTypes();
                foreach (Type exportedType in exportedTypes)
                {
                    if (pluginType.IsAssignableFrom(exportedType))
                    {
                        try
                        {
                            AzusaPlugin instance = (AzusaPlugin)Activator.CreateInstance(exportedType);
                            context.Splash.SetLabel(String.Format("Lade Plug-In:" + instance.DisplayName));
                            instance.OnLoad();
                            if (instance.IsExecutable)
                            {
                                ToolStripButton tsb = new ToolStripButton(instance.DisplayName);
                                tsb.Click += delegate(object sender, EventArgs args) { instance.Execute(); };
                                context.MainForm.plugInsToolStripMenuItem.DropDownItems.Add(tsb);
                                context.MainForm.plugInsToolStripMenuItem.Visible = true;
                            }

                            context.Plugins.Add(instance);
                        }
                        catch (Exception e)
                        {
                            context.Splash.SetLabel(String.Format("Konnte Plug-In {0} nicht starten: {1}",exportedType.Name,e));
                        }
                    }
                }
            }
        }

        private bool AttemptSshPortForward()
        {
            context.Splash.SetLabel("Überprüfe Verfügbarkeit des SSH-Proxy...");

            string hostname = context.Ini["sshProxy"]["host"];
            ushort port = Convert.ToUInt16(context.Ini["sshProxy"]["port"]);
            bool tcpProbe = TcpProbe(hostname,port);
            if (!tcpProbe)
                return false;

            context.Splash.SetLabel("Versuche über SSH-Proxy mit Datenbank zu verbinden...");
            string sshHost = context.Ini["sshProxy"]["host"];
            int sshPort = Convert.ToInt32(context.Ini["sshProxy"]["port"]);
            string sshUser = context.Ini["sshProxy"]["username"];
            string sshPassword = null;
            if (context.Ini["sshProxy"].ContainsKey("password"))
            {
                sshPassword = context.Ini["sshProxy"]["password"];
            }

            List<AuthenticationMethod> authenticationMethods = new List<AuthenticationMethod>();
            authenticationMethods.Add(new NoneAuthenticationMethod(sshUser));

            if (File.Exists("ssh.key"))
            {
                authenticationMethods.Add(
                    new PrivateKeyAuthenticationMethod(sshUser, new PrivateKeyFile[] {new PrivateKeyFile("ssh.key")}));
            }

            if (string.IsNullOrEmpty(sshPassword) && authenticationMethods.Count == 1)
            {
                context.Splash.Invoke((MethodInvoker) delegate
                {
                    sshPassword =
                        TextInputForm.PromptPassword(String.Format("Passwort für {0} auf {1}?", sshUser, sshHost),
                            context.Splash);
                });
            }

            if (!string.IsNullOrEmpty(sshPassword) && authenticationMethods.Count == 1)
            {
                authenticationMethods.Add(new PasswordAuthenticationMethod(sshUser, sshPassword));
            }

            if (authenticationMethods.Count > 1)
            {
                context.Splash.SetLabel("Verbinde mit SSH Server...");
                ConnectionInfo sshConnectionInfo =
                    new ConnectionInfo(sshHost, sshPort, sshUser, authenticationMethods.ToArray());
                context.SSH = new SshClient(sshConnectionInfo);
                if (!context.SSH.IsConnected)
                    context.SSH.Connect();

                context.Splash.SetLabel("Starte Port-Forwarding...");
                uint localPort = (uint) AzusaContext.FindFreePort();
                ForwardedPort sqlPort = new ForwardedPortLocal("127.0.0.1", localPort, context.Ini["postgresql"]["server"], Convert.ToUInt16(context.Ini["postgresql"]["port"]));
                context.SSH.AddForwardedPort(sqlPort);
                sqlPort.Start();
                bool worksWithForward = TcpProbe("127.0.0.1", (int) localPort);
                if (worksWithForward)
                {
                    context.Ini["postgresql"]["server"] = "127.0.0.1";
                    context.Ini["postgresql"]["port"] = localPort.ToString();
                    return true;
                }
                else
                {
                    context.SSH.Disconnect();
                    context.SSH = null;
                }
            }

            return false;
        }

        private void ConnectOnline()
        {
            context.Splash.SetLabel("Verbinde mit Datenbank...");
            context.DatabaseDriver = new PostgresDriver();
            if (!context.DatabaseDriver.ConnectionIsValid())
            {
                MessageBox.Show("Verbindung zur Datenbank ist ungültig!");
                context.DatabaseDriver.Dispose();
                context.DatabaseDriver = null;
                return;
            }

            context.Splash.SetLabel("Überprüfe Lizenz...");
            LicenseState licenseState = context.DatabaseDriver.CheckLicenseStatus(context.LicenseSeed);
            if (licenseState != LicenseState.Valid)
            {
                throw new StartupFailedException(new LicenseValidationFailedException(licenseState),StartupFailReason.LicenseNotValid);
            }

            context.Splash.SetLabel("Validiere Datenbank...");
            List<string> knownTables = context.DatabaseDriver.GetAllSchemas();

            if (knownTables.Count < 3)
                throw new DatabaseValidationFailedException();

            bool hasAzusaTables = knownTables.Any(x => x.ToLowerInvariant().StartsWith("azusa"));
            if (!hasAzusaTables)
                throw new DatabaseValidationFailedException();

            context.Splash.SetLabel("Führe Aufgaben aus, die nach dem Herstellen der Verbindung ausgeführt werden müssen...");
            ExecutePreSyncTasks();

            try
            {
                SynchronizeIfNecessary();
            }
            catch (KeyNotFoundException knfe)
            {
                throw new StartupFailedException(knfe, StartupFailReason.IniBroken);
            }
        }

        private void SynchronizeIfNecessary()
        {
            bool enableSync = Convert.ToInt16(context.Ini["offline"]["sync"]) != 0;
            
            if (enableSync)
            {
                bool enableFullCopy = Convert.ToInt16(context.Ini["offline"]["alwaysFullCopy"]) != 0;
                DirectoryInfo derbyDir = new DirectoryInfo("azusaOffline");

                if (enableFullCopy)
                {
                    RecursiveDeletion(derbyDir);
                }

                try
                {
                    context.Splash.SetLabel("Bereite Synchronisation vor...");
                    derbyDir.Refresh();
                    IDatabaseDriver offlineDriver = new OfflineDriver();
                    Sync azusync = new Sync(context.DatabaseDriver, offlineDriver);
                    azusync.Message += Azusync_Message;
                    azusync.Execute();
                    offlineDriver.Dispose();
                }
                catch (SyncException e)
                {
                    MessageBox.Show(String.Format("Synchronisation fehlgeschlagen!\n\n{0}", e));
                }
            }
        }

        private void Azusync_Message(string message)
        {
            context.Splash.SetLabel(message);
        }

        private void ConnectOffline()
        {
            context.Splash.SetLabel("Starte Offline-Datenbank...");
            DirectoryInfo derbyDir = new DirectoryInfo("azusaOffline");
            if (!derbyDir.Exists)
            {
                context.Splash.InvokeClose();
                MessageBox.Show("Die Offline-Datenbank existiert nicht!");
                return;
            }
            context.DatabaseDriver = new OfflineDriver();

            context.Splash.SetLabel("Überprüfe Lizenz...");
            LicenseState licenseState = context.DatabaseDriver.CheckLicenseStatus(context.LicenseSeed);
            if (licenseState != LicenseState.Valid)
            {
                throw new LicenseValidationFailedException(licenseState);
            }
        }

        private void ShowSplash()
        {
            context.Splash = new Splash();
            context.Splash.ShowDialog();
        }

        private void RecursiveDeletion(DirectoryInfo di)
        {
            if (!di.Exists)
                return;

            foreach(DirectoryInfo subdir in di.GetDirectories())
            {
                RecursiveDeletion(subdir);
            }
            foreach(FileInfo subfile in di.GetFiles())
            {
                context.Splash.SetLabel(String.Format("Deleting file: {0}\\{1}", di.Name, subfile.Name));
                subfile.Delete();
            }
            context.Splash.SetLabel(String.Format("Deleting directory: {0}", di.Name));
            di.Delete();
        }

        private bool TcpProbe(string ip, int port)
        {
            try
            {
                TcpClient tc = new TcpClient();
                tc.Connect(ip, port);
                tc.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void ExecutePreSyncTasks()
        {
            Assembly azusaExe = Assembly.GetCallingAssembly();
            Type[] classes = azusaExe.GetTypes();
            Type moduleType = typeof(IPostConnectionTask);
            foreach (Type type in classes)
            {
                Type[] interfaces = type.GetInterfaces();
                foreach (Type iface in interfaces)
                {
                    if (iface.Equals(moduleType))
                    {
                        IPostConnectionTask control = (IPostConnectionTask)Activator.CreateInstance(type);
                        control.ExecutePostConnectionTask();
                    }
                }
            }
            
        }
    }

    internal class DatabaseValidationFailedException : Exception
    {
    }
}
