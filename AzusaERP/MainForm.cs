using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
        }

        private AzusaContext context;

        internal void ModuleScan()
        {
            AzusaContext context = AzusaContext.GetInstance();
            Assembly azusaExe = Assembly.GetCallingAssembly();
            Type[] classes = azusaExe.GetTypes();
            Type moduleType = typeof(IAzusaModule);
            List<IAzusaModule> loadedModules = new List<IAzusaModule>();
            foreach (Type type in classes)
            {
                Type[] interfaces = type.GetInterfaces();
                foreach (Type iface in interfaces)
                {
                    if (iface.Equals(moduleType))
                    {
                        IAzusaModule control = (IAzusaModule)Activator.CreateInstance(type);

                        if (context.Ini.ContainsKey("hidepage"))
                        {
                            var hidepage = context.Ini["hidepage"];
                            if (hidepage.ContainsKey(control.IniKey))
                            {
                                int result = Convert.ToInt32(hidepage[control.IniKey]);
                                if (result != 0)
                                    continue;
                            }
                        }

                        if (context.CatchModuleExceptions)
                        {
                            try
                            {
                                control.OnLoad();
                                loadedModules.Add(control);
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e);
                                context.Splash.SetLabel(String.Format("Konnte Modul nicht laden: {0} ({1})",
                                    control.IniKey,
                                    e.Message));
                                Thread.Sleep(1000);
                            }
                        }
                        else
                        {
                            control.OnLoad();
                            loadedModules.Add(control);
                        }
                    }
                }
            }

            loadedModules.Sort((x, y) => x.Priority.CompareTo(y.Priority));

            foreach(IAzusaModule loadedModule in loadedModules)
            {
                TabPage tabPage = new TabPage();
                System.Windows.Forms.Control control = loadedModule.GetSelf();
                control.Dock = DockStyle.Fill;
                tabPage.Controls.Add(control);
                tabPage.Text = loadedModule.Title;
                tabControl1.TabPages.Add(tabPage);
            }
        }

        public void SetStatusBar(string s)
        {
            Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = s; });
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (Convert.ToInt16(context.Ini["form"]["bringToFront"]) > 0)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }
            if (Convert.ToInt16(context.Ini["form"]["maximize"]) > 0)
            {
                this.WindowState = FormWindowState.Minimized;
                this.WindowState = FormWindowState.Maximized;
            }
            if (context.Ini["form"].ContainsKey("setsize"))
            {
                string setsize = context.Ini["form"]["setsize"];
                if (!string.IsNullOrEmpty(setsize))
                {
                    string[] sizeArgs = setsize.Split(',');
                    Width = Convert.ToInt32(sizeArgs[0]);
                    Height = Convert.ToInt32(sizeArgs[1]);
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabPage tabPage = tabControl1.SelectedTab;
            System.Windows.Forms.Control control = tabPage.Controls[0];
            context.CurrentOnScreenModule = (IAzusaModule)control;
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
