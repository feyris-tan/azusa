using System;
using System.Windows.Forms;
using AzusaERP.OldStuff;
using moe.yo3explorer.azusa.SedgeTree.Control;
using moe.yo3explorer.azusa.SedgeTree.Entitiy;

namespace moe.yo3explorer.azusa.SedgeTree.Boundary
{
    public partial class Checker : Form
    {
        public Checker()
        {
            InitializeComponent();
            context = AzusaContext.GetInstance();
            this.Text = "Konsistenzprüfung " + DateTime.Now;
            int cons = 0;
            Bloodline data = SedgeTreeMemoryCardEmulation.GetInstance().GetData();
            
            foreach (Person p in data)
            {
                if (!p.consistent)
                {
                    if (p.birthplace == "")
                    {
                        AddEntry(p.FullName + " hat keinen Geburtsort!");
                    }
                    if (p.born == DateTime.MinValue)
                    {
                        AddEntry(p.FullName + " hat kein Geburtsdatum!");
                    }
                    else
                    {
                        DateTime maximumTheoreticalAge = DateTime.Now;
                        if (p.died != DateTime.MinValue)
                            maximumTheoreticalAge = p.died;
                        TimeSpan age_check = maximumTheoreticalAge - p.born;
                        int year = age_check.Days / 365;
                        if (year > 100)
                        {
                            AddEntry(p.FullName + " ist anscheinend " + year.ToString() + " Jahre alt. Stimmt das?");
                        }
                    }
                    if (p.father == null)
                    {
                        AddEntry(p.FullName + " hat keinen Vater!");
                    }
                    if (p.forename == "")
                    {
                        AddEntry(p.FullName + " hat keinen Vornamen!");
                    }
                    if (p.gender == Gender.DontCare)
                    {
                        AddEntry(p.FullName + " hat kein Geschlecht!");
                    }
                    if (p.mother == null)
                    {
                        AddEntry(p.FullName + " hat keine Mutter!");
                    }
                    if (p.surname == "")
                    {
                        AddEntry(p.FullName + " hat keinen Nachnamen!");
                    }
                    if (!CheckPhoto(p))
                    {
                        AddEntry(p.FullName + " hat kein Foto!");
                    }
                    cons++;
                }
            }
            if (cons == data.Count)
            {
                AddEntry("Es existiert kein konsistenter Datensatz!");
            }
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
            if (branches < 3)
            {
                AddEntry("Es gibt weniger als drei bekannte Generationen.");
            }
        }

        void AddEntry(string name)
        {
            checkedListBox1.Items.Add(name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        AzusaContext context;
        private bool CheckPhoto(Person p)
        {
            return context.DatabaseDriver.SedgeTree_TestForPhoto(p._guid.ToString());
        }
    }
}
