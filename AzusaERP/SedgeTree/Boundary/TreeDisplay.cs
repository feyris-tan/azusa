using System;
using System.Windows.Forms;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;

namespace moe.yo3explorer.azusa.SedgeTree.Boundary
{
    internal partial class TreeDisplay : Form
    {
        public TreeDisplay(Person t)
        {
            InitializeComponent();
            child = t;
            UpdateView();
            int f = t.Generations;
        }
        Person child, mother, father, maternal_grandmother, maternal_grandfather, paternal_grandmother, paternal_grandfather;

        void UpdateView()
        {
            this.Text = "Baumansicht für " + child.FullName;
            string unk_const = "???";
            button1.Text = unk_const;
            button2.Text = unk_const;
            button3.Text = unk_const;
            button4.Text = unk_const;
            button5.Text = unk_const;
            button6.Text = unk_const;
            button7.Text = unk_const;
            mother = child.mother;
            father = child.father;
            if (mother != null)
            {
                maternal_grandfather = mother.father;
                maternal_grandmother = mother.mother;
            }
            if (father != null)
            {
                paternal_grandfather = father.father;
                paternal_grandmother = father.mother;
            }
            if (child != null)
            {
                button1.Text = child.CallName;
            }
            if (father != null)
            {
                button2.Text = father.CallName;
            }
            if (mother != null)
            {
                button3.Text = mother.CallName;
            }
            if (paternal_grandfather != null)
            {
                button4.Text = paternal_grandfather.CallName;
            }
            if (paternal_grandmother != null)
            {
                button5.Text = paternal_grandmother.CallName;
            }
            if (maternal_grandfather != null)
            {
                button7.Text = maternal_grandfather.CallName;
            }
            if (maternal_grandmother != null)
            {
                button6.Text = maternal_grandmother.CallName;
            }
            button1.Enabled = (child != null);
            button2.Enabled = (father != null);
            button3.Enabled = (mother != null);
            button4.Enabled = (paternal_grandfather != null);
            button5.Enabled = (paternal_grandmother != null);
            button6.Enabled = (maternal_grandmother != null);
            button7.Enabled = (maternal_grandfather != null);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            TreeDisplay n = new TreeDisplay(father);
            n.MdiParent = this.MdiParent;
            n.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TreeDisplay n = new TreeDisplay(mother);
            n.MdiParent = this.MdiParent;
            n.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TreeDisplay n = new TreeDisplay(paternal_grandfather);
            n.MdiParent = this.MdiParent;
            n.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            TreeDisplay n = new TreeDisplay(paternal_grandmother);
            n.MdiParent = this.MdiParent;
            n.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TreeDisplay n = new TreeDisplay(maternal_grandmother);
            n.MdiParent = this.MdiParent;
            n.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            TreeDisplay n = new TreeDisplay(maternal_grandfather);
            n.MdiParent = this.MdiParent;
            n.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (child.siblings == null)
            {
                MessageBox.Show(child.FullName + " hat keine Geschwister.");
                return;
            }
            else if (child.siblings.Count == 0)
            {
                MessageBox.Show(child.FullName + " hat keine Geschwister.");
                return;
            }

            Person target = PersonSelector.DoSelection(this, child.siblings);
            if (target == null)
                return;
            TreeDisplay td = new TreeDisplay(target);
            td.MdiParent = this.MdiParent;
            td.Show();
        }
    }
}
