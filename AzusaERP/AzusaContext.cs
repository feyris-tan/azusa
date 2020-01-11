using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using AzusaERP;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.Control.Licensing;
using Renci.SshNet;

namespace moe.yo3explorer.azusa
{
    public class AzusaContext
    {
        private static AzusaContext instance;

        public static Queue<AzusaPlugin> GetPluginQueue()
        {
            return GetInstance().PluginLoadQueue;
        }

        internal static AzusaContext GetInstance()
        {
            if (instance == null)
            {
                instance = new AzusaContext();
                instance.CultureInfo = CultureInfo.CurrentUICulture;
                instance.RandomNumberGenerator = new Random();
                instance.LicenseSeed = NetworkAdapterLicenseMapper.GetLicenseMapping();
                instance.Plugins = new List<AzusaPlugin>();
                instance.PluginLoadQueue = new Queue<AzusaPlugin>();
            }
            
            instance.NumOperations++;
            return instance;
        }
        
        internal static int FindFreePort()
        {
            TcpListener tl = new TcpListener(IPAddress.Loopback, 0);
            tl.Start();
            int result = ((IPEndPoint)tl.LocalEndpoint).Port;
            tl.Stop();
            return result;
        }

        public void DestroyContext()
        {
            foreach (AzusaPlugin azusaPlugin in Plugins)
            {
                azusaPlugin.Dispose();
            }

            if (WebServer != null)
            {
                if (WebServer.IsRunning)
                {
                    string url = String.Format("{0}stop.elf", WebServer.Prefix);
                    new System.Net.WebClient().DownloadString(url);
                }
            }

            if (SSH != null)
            {
                foreach(ForwardedPort port in SSH.ForwardedPorts)
                {
                    if (port.IsStarted)
                        port.Stop();
                    
                }
                if (SSH.IsConnected)
                    SSH.Disconnect();
            }

            if (Splash != null)
            {
                Splash.Dispose();
            }

            if (MainForm != null)
            {
                MainForm.Dispose();
            }
        }

        public bool IsModuleEnabled(IAzusaModule module)
        {
            if (!Ini.ContainsKey("disabledModules"))
                return true;

            if (!Ini["disabledModules"].ContainsKey(module.IniKey))
                return true;

            int value = Convert.ToInt32(Ini["disabledModules"][module.IniKey]);
            return value > 0;
        }

        public string ReadIniKey(string cat, string key, string defaultValue)
        {
            if (!Ini.ContainsKey(cat))
                return defaultValue;

            if (!Ini[cat].ContainsKey(key))
                return defaultValue;

            return Ini[cat][key];
        }

        public int ReadIniKey(string cat, string key, int defaultValue)
        {
            return Convert.ToInt32(ReadIniKey(cat, key, defaultValue.ToString()));
        }

        private AzusaContext() { }
        
        public Ini Ini { get; set; }
        public Splash Splash { get; set; }
        public MainForm MainForm { get; set; }
        public IDatabaseDriver DatabaseDriver { get; set; }
        public ulong NumOperations { get; set; }
        public CultureInfo CultureInfo { get; private set; }
        public bool TabletMode { get; set; }
        public IAzusaModule CurrentOnScreenModule { get; set; }
        public Random RandomNumberGenerator { get; private set; }
        public WebServer WebServer { get; set; }
        public Icon Icon { get; set; }
        public SshClient SSH { get; set; }
        public byte[] LicenseSeed { get; private set; }
        public List<AzusaPlugin> Plugins { get; private set; }
        public Queue<AzusaPlugin> PluginLoadQueue { get; private set; }
    }

}
