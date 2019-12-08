using System;
using System.Windows.Forms;
using moe.yo3explorer.azusa.SedgeTree.Control;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;

namespace moe.yo3explorer.azusa.SedgeTree.Boundary
{
    public partial class SedgeTreeControl : UserControl, IAzusaModule
    {
        public SedgeTreeControl()
        {
            InitializeComponent();
        }

        public string IniKey => "sedgetree";
        public string Title => "Familienstammbaum";
        public int Priority => 5;
        private SedgeTreeMemoryCardEmulation memoryCard;

        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
            memoryCard = SedgeTreeMemoryCardEmulation.GetInstance();
            UpdateDate();
        }

        void UpdateDate()
        {
            if (memoryCard.GetData()._last_edited != DateTime.MinValue)
            {
                toolStripStatusLabel1.Text = "Zuletzt gespeichert: " + memoryCard.GetData()._last_edited.ToString();
            }
            else
            {
                toolStripStatusLabel1.Text = "Stammbaum ist im Moment leer.";
            }
            if (speichernToolStripMenuItem.Enabled)
            {
                toolStripStatusLabel1.Text += " (Es bestehen ungespeicherte Änderungen!)";
            }
            bool enableStuff = memoryCard.GetData().Count > 0;
            personBearbeitenToolStripMenuItem.Enabled = enableStuff;
            personLöschenToolStripMenuItem.Enabled = enableStuff;
            baumansichtToolStripMenuItem.Enabled = enableStuff;
            datenbankÜberprüfenToolStripMenuItem.Enabled = enableStuff;

        }
        
        private void neuePersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Person newcomer = new Person();
            Editor ed = new Editor(newcomer);
            ed.ShowDialog();
            if (newcomer.forename == "")
            {
                if (newcomer.surname == "")
                {
                    return;
                }
            }
            memoryCard.GetData().Add(newcomer);
            speichernToolStripMenuItem.Enabled = true;
            UpdateDate();
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            memoryCard.SaveData();
            UpdateDate();
            speichernToolStripMenuItem.Enabled = false;
        }

        private void personBearbeitenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Person target = PersonSelector.DoSelection(this);
            if (target == null)
                return;
            Editor ed = new Editor(target);
            ed.ShowDialog();
            speichernToolStripMenuItem.Enabled = true;
            UpdateDate();
        }

        private void personLöschenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bloodline data = memoryCard.GetData();
            Person target = PersonSelector.DoSelection(this);
            if (MessageBox.Show(this, "Soll " + target.FullName + " gelöscht werden?", "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }
            data.Remove(target);
            int a = 0;
            foreach (Person quake in data)
            {
                if (quake.children != null)
                {
                    if (quake.children.Contains(target))
                    {
                        quake.children.Remove(target);
                        a++;
                    }
                }
                if (quake.father == target)
                {
                    quake.father = null;
                    a++;
                }
                if (quake.marriage == target)
                {
                    quake.marriage = null;
                    a++;
                }
                if (quake.mother == target)
                {
                    quake.mother = null;
                    a++;
                }
                if (quake.siblings != null)
                {
                    if (quake.siblings.Contains(target))
                    {
                        quake.siblings.Remove(target);
                        a++;
                    }
                }
            }
            MessageBox.Show(target.FullName + " wurde gelöscht.\n" + a.ToString() + " Bezüge wurden entfernt.");
            speichernToolStripMenuItem.Enabled = true;
        }

        private void baumansichtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bloodline data = memoryCard.GetData();
            if (data.Count == 0)
                return;

            Person target = null;
            int branches = 0;
            foreach (Person p in data)
            {
                if (p.Generations > branches)
                {
                    target = p;
                    branches = p.Generations;
                }
            }
            TreeDisplay ed = new TreeDisplay(target);
            ed.Show();
        }

        private void datenbankÜberprüfenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Checker c = new Checker();
            c.Show();
        }
    }
}
