using System;
using System.Drawing;
using System.Windows.Forms;
using moe.yo3explorer.azusa.SedgeTree.Control;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;

namespace moe.yo3explorer.azusa.SedgeTree.Boundary
{
    internal partial class Editor : Form
    {
        Person target;
        Bloodline data;

        public Editor(Person _t)
        {
            data = SedgeTreeMemoryCardEmulation.GetInstance().GetData();
            AzusaContext context = AzusaContext.GetInstance();

            target = _t;
            InitializeComponent(); 
            UpdateView(_t);


            byte[] buffer = context.DatabaseDriver.SedgeTree_GetPhotoByPerson(_t);
            if (buffer != null)
            {
                pictureBox1.Image = Image.FromStream(new System.IO.MemoryStream(buffer));
            }

            button2.Enabled = data.ContainsFemales;
            button3.Enabled = data.ConatinsMales;
        }

        private void UpdateView(Person _t)
        {

            textBox1.Text = _t.forename;
            textBox2.Text = _t.surname;
            comboBox1.SelectedIndex = (int)_t.gender;
            textBox3.Text = _t.remarks;
            if (_t.mother != null) textBox4.Text = _t.mother.ToString();
            if (_t.children != null) textBox5.Text = _t.children.ToString();
            if (_t.father != null) textBox6.Text = _t.father.ToString();
            if (_t.marriage != null) textBox7.Text = _t.marriage.ToString();
            if (_t.siblings != null) textBox8.Text = _t.siblings.ToString();
            label10.Text = "";
            if (_t.last_edited != DateTime.MinValue) label10.Text = "Zuletzt bearbeitet: " + _t.last_edited;
            textBox9.Text = _t.maiden_name;
            textBox10.Text = _t.birthplace;
            if (_t.born != DateTime.MinValue) textBox11.Text = _t.born.ToShortDateString();
            checkBox1.Checked = _t.illegitimate_marriages;
            if (_t.died != DateTime.MinValue) textBox12.Text = _t.died.ToShortDateString();
            checkBox2.Checked = _t.consistent;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveData();
            target.last_edited = DateTime.Now;
            this.Close();
        }

        private void SaveData()
        {
            target.forename = textBox1.Text;
            target.surname = textBox2.Text;
            target.gender = (Gender)comboBox1.SelectedIndex;
            target.remarks = textBox3.Text;
            target.maiden_name = textBox9.Text;
            target.birthplace = textBox10.Text;
            target.illegitimate_marriages = checkBox1.Checked;
            target.consistent = checkBox2.Checked;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Person mom = PersonSelector.DoSelection(this, Gender.Female);
            
            if (mom != null)
            {
                SaveData();
                target.mother = mom;
                UpdateView(target);
                if (mom.children == null) mom.children = new Family();
                mom.children.Add(target);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Person dad = PersonSelector.DoSelection(this, Gender.Male);
            if (dad != null)
            {
                SaveData();
                target.father = dad;
                UpdateView(target);
                if (dad.children == null) dad.children = new Family();
                dad.children.Add(target);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Person partner = PersonSelector.DoSelection(this);
            if (partner != null)
            {
                SaveData();
                target.marriage = partner;
                partner.marriage = target;
                UpdateView(target);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (Person p in data)
            {
                if (p.siblings != null)
                {
                    if (p.siblings.Contains(target))
                    {
                        p.siblings.Remove(target);
                    }
                }
            }
            target.siblings = null;
            UpdateView(target);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Person sibling = PersonSelector.DoSelection(this);
            if (sibling != null)
            {
                SaveData();
                if (target.siblings == null) target.siblings = new Family();
                if (sibling.siblings == null) sibling.siblings = new Family();
                target.siblings.Add(sibling);
                sibling.siblings.Add(target);
                UpdateView(target);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            SaveData();
            DateTime dt = Calendar.ShowCalendar(this);
            if (dt != DateTime.MinValue)
            {
                target.born = dt;
                UpdateView(target);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            checkBox1.Text = textBox1.Text + " " + textBox2.Text + " war mehrmals verheiratet.";
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            checkBox1.Text = textBox1.Text + " " + textBox2.Text + " war mehrmals verheiratet.";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Person child = PersonSelector.DoSelection(this);
            if (child != null)
            {
                SaveData();
                if (target.gender == Gender.Female) child.mother = target;
                if (target.gender == Gender.Male) child.father = target;
                if (target.children == null) target.children = new Family();
                target.children.Add(child);
                UpdateView(target);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            target.children = null;
            foreach (Person p in data)
            {
                if (p.mother == target) p.mother = null;
                if (p.father == target) p.father = null;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SaveData();
            DateTime dt = Calendar.ShowCalendar(this);
            if (dt != DateTime.MinValue)
            {
                target.died = dt;
                UpdateView(target);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            System.IO.MemoryStream original_image_stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(openFileDialog1.FileName));
            Image original_image = Image.FromStream(original_image_stream);
            double aspect = ((double)original_image.Width / (double)original_image.Height);
            int width = 165;
            int height = (int)(width / aspect);
            Image resized = original_image.GetThumbnailImage(width, height, null, IntPtr.Zero);
            pictureBox1.Image = resized;

            AzusaContext context = AzusaContext.GetInstance();
            bool update = context.DatabaseDriver.SedgeTree_TestForPhoto(target._guid.ToString());

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            resized.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            if (update)
                context.DatabaseDriver.SedgeTree_UpdatePhoto(ms.ToArray(), target._guid.ToString());
            else
                context.DatabaseDriver.SedgeTree_InsertPhoto(ms.ToArray(), target._guid.ToString());
            
            original_image_stream.Dispose();
            original_image.Dispose();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;

            AzusaContext.GetInstance().DatabaseDriver.SedgeTree_ErasePhoto(target._guid.ToString());
        }
    }
}
