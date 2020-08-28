using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using AzusaERP.OldStuff;
using moe.yo3explorer.azusa.DexcomHistory.Control;
using moe.yo3explorer.azusa.DexcomHistory.Entity;

namespace moe.yo3explorer.azusa.DexcomHistory.Boundary
{
    public partial class ManualDataEntires : Form
    {
        AzusaContext azusaContext;

        public ManualDataEntires()
        {
            InitializeComponent();
            azusaContext = AzusaContext.GetInstance();
        }

        private void beendenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ManualDataEntires_Shown(object sender, EventArgs e)
        {
            isLoading = true;
            dataGridView1.Rows.Clear();
            foreach (ManualDataEntity entity in ManualDataRepository.GetAllValues())
            {
                dataGridView1.Rows.Add(entity.GetDgvRow());
            }
            isLoading = false;
            dataGridView1.ClearSelection();
            dataGridView1.Rows[dataGridView1.RowCount - 2].Selected = true;
            dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 2;
        }

        private void dateiImportierenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) != DialogResult.OK)
                return;

            StreamReader sr = new StreamReader(openFileDialog1.FileName);
            while (!sr.ReadLine().StartsWith("Datum;Uhrzeit;Messwert;Einheit;")) ;

            string line;
            while (true)
            {
                line = sr.ReadLine();
                if (line.StartsWith("   "))
                    break;

                string[] args = line.Split(';');
                args[0] = String.Format("{0} {1}", args[0], args[1]);
                args[1] = "";

                DateTime dt = DateTime.ParseExact(args[0], "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture);
                short value = Convert.ToInt16(args[2]);

                if (!ManualDataRepository.HasTimestamp(dt))
                {
                    ManualDataRepository.StoreValue(dt, value, args[3]);
                }
            }
            ManualDataEntires_Shown(sender, e);
            
        }

        private bool isLoading;

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (isLoading)
                return;
            if (e.RowIndex == -1)
                return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            int id = (int)row.Cells[0].Value;
            DateTime dateAdded = (DateTime)row.Cells[1].Value;
            short messwer = (short)row.Cells[3].Value;
            string einheit = (string)row.Cells[4].Value;
            byte be = Convert.ToByte(row.Cells[5].Value);
            byte novorapid = Convert.ToByte(row.Cells[6].Value);
            byte levemir = Convert.ToByte(row.Cells[7].Value);
            bool hide = (bool)row.Cells[8].Value;
            string note = (string)row.Cells[10].Value;
            //TODO: Continue here

            ManualDataRepository.EditValue(id, be, novorapid, levemir, note);
        }

        private void ManualDataEntires_Load(object sender, EventArgs e)
        {

        }
    }
}
