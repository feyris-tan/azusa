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
            List<Type> foundModules = new List<Type>();
            foreach (Type type in classes)
            {
                Type[] interfaces = type.GetInterfaces();
                foreach (Type iface in interfaces)
                {
                    if (iface.Equals(moduleType))
                    {
                        foundModules.Add(type);
                    }
                }
            }
            
            foreach(Type loadedModule in foundModules)
            {
                ToolStripButton tsb = new ToolStripButton(loadedModule.Name);
                tsb.Click += delegate(object sender, EventArgs args)
                {
                    IAzusaModule instance = (IAzusaModule)Activator.CreateInstance(loadedModule);
                    instance.OnLoad();

                    Form subform = new Form();
                    System.Windows.Forms.Control control = instance.GetSelf();
                    control.Dock = DockStyle.Fill;
                    subform.Controls.Add(control);
                    subform.Text = instance.Title;
                    subform.Show(this);
                };

                stammdatenToolStripMenuItem.DropDownItems.Add(tsb);
                stammdatenToolStripMenuItem.Visible = true;
            }
        }
        

        private bool mediaLibraryBooted;
        internal void BootMediaLibrary()
        {
            if (!mediaLibraryBooted)
            {
                foreach (ToolStripItem toolStripItem in mediaLibraryControl1.DestroyMenuStrip())
                {
                    menuStrip1.Items.Add(toolStripItem);
                }

                mediaLibraryControl1.OnLoad();
                mediaLibraryBooted = true;
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
        
        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
