using System;
using System.Windows.Forms;
using moe.yo3explorer.azusa.SedgeTree.Control;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;

namespace moe.yo3explorer.azusa.SedgeTree.Boundary
{
    internal partial class PersonSelector : Form
    {
        private Person result;
        private Bloodline data;

        public PersonSelector()
        {
            InitializeComponent();
            data = SedgeTreeMemoryCardEmulation.GetInstance().GetData();
            foreach (Person member in data)
            {
                comboBox1.Items.Add(member);
                comboBox1.SelectedItem = member;
            }
        }
        public PersonSelector(Family siblings)
        {
            InitializeComponent();
            data = SedgeTreeMemoryCardEmulation.GetInstance().GetData();
            foreach (Person member in siblings)
            {
                comboBox1.Items.Add(member);
                comboBox1.SelectedItem = member;
            }
        }

        public PersonSelector(Gender g)
        {
            InitializeComponent();
            data = SedgeTreeMemoryCardEmulation.GetInstance().GetData();
            foreach (Person member in data)
            {
                if (member.gender == g)
                {
                    comboBox1.Items.Add(member);
                    comboBox1.SelectedItem = member;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (Person member in data)
            {
                if (member == comboBox1.SelectedItem )
                {
                    result = member;
                    return;
                }
            }
            throw new Exception();
        }

        public static Person DoSelection(System.Windows.Forms.Control parent)
        {
            PersonSelector ps = new PersonSelector();
            ps.ShowDialog(parent);
            return ps.result;
        }

        public static Person DoSelection(Form parent, Family f)
        {
            PersonSelector ps = new PersonSelector(f);
            ps.ShowDialog(parent);
            return ps.result;
        }

        public static Person DoSelection(Form parent, Gender g)
        {
            PersonSelector ps = new PersonSelector(g);
            ps.ShowDialog(parent);
            return ps.result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            result = null;
            this.Close();
        }
    }
}
