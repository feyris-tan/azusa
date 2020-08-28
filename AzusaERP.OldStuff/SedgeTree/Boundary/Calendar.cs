using System;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.SedgeTree.Boundary
{
    public partial class Calendar : Form
    {
        public Calendar()
        {
            InitializeComponent();
            for (int i = 1760; i < DateTime.Now.Year; i += 10)
            {
                comboBox1.Items.Add(i);
            }
        }

        DateTime selected;

        public static DateTime ShowCalendar(Form r)
        {
            Calendar c = new Calendar();
            c.ShowDialog(r);
            return c.selected;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            selected = monthCalendar1.SelectionStart;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            selected = DateTime.MinValue;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            monthCalendar1.SelectionStart = new DateTime((int)comboBox1.SelectedItem, DateTime.Now.Month, DateTime.Now.Day);
            monthCalendar1.SelectionEnd = monthCalendar1.SelectionStart;
            
        }
    }
}
