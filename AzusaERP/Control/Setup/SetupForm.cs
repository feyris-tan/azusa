using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AzusaERP;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.Control.DatabaseIO.Drivers;
using moe.yo3explorer.azusa.Control.Licensing;
using TagLib;
using File = System.IO.File;

namespace moe.yo3explorer.azusa.Control.Setup
{
    public partial class SetupForm : Form
    {
        public SetupForm(Exception e)
            : this()
        {
            caughtException = e;
            fehlerdiagnoseToolStripMenuItem.Enabled = caughtException != null;
        }

        public SetupForm()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
            azusaIniFileInfo = new FileInfo("azusa.ini");
            toolStripStatusLabel1.Text = "";
            InitForm();
        }

        private void InitForm()
        {
            if (context.Ini == null)
            {
                context.Ini = new Ini();
            }

            if (!context.Ini.ContainsKey("azusa"))
            {
                context.Ini["azusa"] = new IniSection();
                context.Ini["azusa"]["forceOffline"] = "0";
                stuffChanged = true;
            }

            IniSection azusaSection = context.Ini["azusa"];
            if (azusaSection.ContainsKey("forceOffline"))
            {
                offlineModusErzwingenToolStripMenuItem.Checked = Convert.ToInt32(azusaSection["forceOffline"]) > 0;
            }

            if (!context.Ini.ContainsKey("form"))
            {
                context.Ini["form"] = new IniSection();
                stuffChanged = true;
            }

            IniSection formSection = context.Ini["form"];
            if (!formSection.ContainsKey("bringToFront"))
            {
                formSection["bringToFront"] = "1";
                stuffChanged = true;
            }
            if (!formSection.ContainsKey("maximize"))
            {
                formSection["maximize"] = "1";
                stuffChanged = true;
            }

            nachStartInDenVordergrundBringenToolStripMenuItem.Checked = Convert.ToInt32(formSection["bringToFront"]) > 0;
            nachStartMaximierenToolStripMenuItem.Checked = Convert.ToInt32(formSection["maximize"]) > 0;

            postgresqlModel = new PostgresqlModel();
            if (!context.Ini.ContainsKey("postgresql"))
                context.Ini.Add("postgresql",new IniSection());
            IniSection postgresSection = context.Ini["postgresql"];
            if (postgresSection.ContainsKey("server"))
                postgresqlModel.server = postgresSection["server"];
            if (postgresSection.ContainsKey("port"))
                postgresqlModel.port = Convert.ToUInt16(postgresSection["port"]);
            if (postgresSection.ContainsKey("database"))
                postgresqlModel.database = postgresSection["database"];
            if (postgresSection.ContainsKey("password"))
                postgresqlModel.password = postgresSection["password"];
            if (postgresSection.ContainsKey("username"))
                postgresqlModel.username = postgresSection["username"];

            if (!context.Ini.ContainsKey("offline"))
            {
                context.Ini["offline"] = new IniSection();
                stuffChanged = true;
            }

            IniSection offlineSection = context.Ini["offline"];
            if (!offlineSection.ContainsKey("sync"))
            {
                offlineSection["sync"] = "0";
                stuffChanged = true;
            }
            synchronisationErlaubenToolStripMenuItem.Checked = Convert.ToInt32(offlineSection["sync"]) > 0;

            if (!offlineSection.ContainsKey("alwaysFullCopy"))
            {
                offlineSection["alwaysFullCopy"] = "0";
                stuffChanged = true;
            }
            jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem.Checked = Convert.ToInt32(offlineSection["alwaysFullCopy"]) > 0;

            if (context.DatabaseDriver == null)
            {
                diesesGerätLizensierenToolStripMenuItem.Enabled = false;
                toolStripStatusLabel1.Text = "Lizenz kann nicht aktiviert werden, da keine Datenbankverbindung besteht.";
            }

            if (context.DatabaseDriver is OfflineDriver)
            {
                diesesGerätLizensierenToolStripMenuItem.Enabled = false;
                toolStripStatusLabel1.Text = "Lizenzen können offline nicht aktiviert werden.";
            }
        }

        private bool stuffChanged;
        private AzusaContext context;
        private FileInfo azusaIniFileInfo;
        private PostgresqlModel postgresqlModel;
        private Exception caughtException;

        private void setupBeendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            if (stuffChanged)
            {
                byte[] export = context.Ini.Export();
                if (azusaIniFileInfo.Exists)
                    azusaIniFileInfo.Delete();
                File.WriteAllBytes(azusaIniFileInfo.FullName, export);
                DialogResult = DialogResult.OK;
            }
            this.Close();
        }

        private void offlineModusErzwingenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!context.Ini.ContainsKey("azusa"))
            {
                context.Ini.Add("azusa",new IniSection());
            }

            context.Ini["azusa"]["forceOffline"] = offlineModusErzwingenToolStripMenuItem.Checked ? "1" : "0";
            stuffChanged = true;
        }

        private void postgreSQLVerbindungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyGridForm pgf = new PropertyGridForm(postgresqlModel);
            DialogResult dialogResult = pgf.ShowDialog(this);
            if (pgf.StuffChanged)
            {
                context.Ini["postgresql"]["server"] = postgresqlModel.server;
                context.Ini["postgresql"]["port"] = postgresqlModel.port.ToString();
                context.Ini["postgresql"]["database"] = postgresqlModel.database;
                context.Ini["postgresql"]["username"] = postgresqlModel.username;
                context.Ini["postgresql"]["password"] = postgresqlModel.password;
                stuffChanged = true;
            }
        }

        private void nachStartInDenVordergrundBringenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            context.Ini["form"]["bringToFront"] = nachStartInDenVordergrundBringenToolStripMenuItem.Checked ? "1" : "0";
        }

        private void nachStartMaximierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            context.Ini["form"]["maximize"] = nachStartMaximierenToolStripMenuItem.Checked ? "1" : "0";
        }

        private void synchronisationErlaubenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            context.Ini["offline"]["sync"] = synchronisationErlaubenToolStripMenuItem.Checked ? "1" : "0";
        }

        private void fehlerdiagnoseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PropertyGridForm pgf = new PropertyGridForm(caughtException);
            pgf.ShowDialog(this);
        }

        private void jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            context.Ini["offline"]["alwaysFullCopy"] = jedesMalVollsynchronisierennichtEmpfohlenToolStripMenuItem.Checked ? "1" : "0";
        }

        private void diesesGerätLizensierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PostgresDriver databaseDriver = AzusaContext.GetInstance().DatabaseDriver as PostgresDriver;
            if (postgresqlModel == null)
            {
                MessageBox.Show("Aktuelle Datenbankverbindung ist nicht lizensierbar!");
                return;
            }

            if (context.LicenseSeed == null)
            {
                MessageBox.Show("Lizenzschlüssel konnte nicht ermittelt werden!");
                return;
            }

            LicenseState checkLicenseStatus = databaseDriver.CheckLicenseStatus(context.LicenseSeed);
            switch (checkLicenseStatus)
            {
                case LicenseState.LicenseNotActivated:
                    databaseDriver.ActivateLicense(context.LicenseSeed);
                    MessageBox.Show("Lizenz wurde aktiviert!");
                    break;
                default:
                    MessageBox.Show(checkLicenseStatus.ToString());
                    break;
            }
        }
    }
}
