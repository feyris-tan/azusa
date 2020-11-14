using System;
using System.Drawing;
using System.Windows.Forms;

namespace colormatrix
{
    public partial class Form1 : Form
    {
        private Color[] vgacolors;
        private int[] currentMatrix;
        private Panel[] panels;

        public Form1()
        {
            InitializeComponent();

            vgacolors = new Color[]
            {
                Color.FromArgb(0,0,0),
                Color.FromArgb(128,0,0),
                Color.FromArgb(0,128,0),
                Color.FromArgb(128,128,0),
                Color.FromArgb(0,0,128),
                Color.FromArgb(128,0,128),
                Color.FromArgb(0,128,128),
                Color.FromArgb(192,192,192),
                Color.FromArgb(128,128,128),
                Color.FromArgb(255,0,0),
                Color.FromArgb(0,255,0),
                Color.FromArgb(255,255,0),
                Color.FromArgb(0,0,255),
                Color.FromArgb(255,0,255),
                Color.FromArgb(0,255,255),
                Color.FromArgb(255,255,255)
            };
            int total = tableLayoutPanel1.ColumnCount * tableLayoutPanel1.RowCount;
            currentMatrix = new int[total];
            panels = new Panel[total];

            for (int y = 0; y < tableLayoutPanel1.RowCount; y++)
            {
                for (int x = 0; x < tableLayoutPanel1.ColumnCount; x++)
                {
                    Panel panel = new Panel();
                    panel.Dock = DockStyle.Fill;
                    panel.Margin = new Padding(0, 0, 0, 0);
                    tableLayoutPanel1.Controls.Add(panel, x, y);
                    panels[(y * tableLayoutPanel1.RowCount) + x] = panel;
                }
            }

            ApplyColors();
        }

        private void ApplyColors()
        {
            for (int i = 0; i < currentMatrix.Length; i++)
            {
                panels[i].BackColor = vgacolors[currentMatrix[i]];
            }
        }

        private void Increment()
        {
            for (int i = 0; i < currentMatrix.Length; i++)
            {
                currentMatrix[i]++;
                if (currentMatrix[i] > (vgacolors.Length - 1))
                {
                    currentMatrix[i] = 0;
                    currentMatrix[i + 1]++;
                }
                else
                {
                    break;
                }
            }
        }

        private long frames;
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            Increment();
            ApplyColors();
            Text = String.Format("colormatrix {0}", frames++);
        }
    }
}
