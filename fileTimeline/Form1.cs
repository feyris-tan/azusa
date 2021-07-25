using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace fileTimeline
{
    public partial class Form1 : Form
    {
        public Form1(DirectoryInfo rootDir)
        {
            InitializeComponent();
            scanThreadMember = new Thread(ScanThread);
            scanThreadMember.Priority = ThreadPriority.Lowest;
            scanThreadMember.Name = "Scanner";
            scanThreadMember.Start(rootDir);
        }

        private Thread scanThreadMember;

        private void ScanThread(object boxedRootDir)
        {
            DirectoryInfo rootDir = (DirectoryInfo) boxedRootDir;
            timeline = new List<TimelineDate>();
            ScanDirectory(rootDir);
            Invoke((MethodInvoker) delegate { statusStrip1.Hide(); });
        }

        private void ScanDirectory(DirectoryInfo di)
        {
            foreach (FileSystemInfo fileSystemInfo in di.GetFileSystemInfos())
            {
                if (fileSystemInfo is DirectoryInfo)
                {
                    ScanDirectory((DirectoryInfo) fileSystemInfo);
                }
                else
                {
                    DateTime creationDate = fileSystemInfo.LastAccessTime.Date;
                    TimelineDate timelineEntry = GetTimelineEntry(creationDate);
                    if (timelineEntry == null)
                    {
                        timelineEntry = new TimelineDate(creationDate);
                        timeline.Add(timelineEntry);
                        timeline.Sort((x,y) => DateTime.Compare(x.Date,y.Date) / -1);
                        Invoke((MethodInvoker) delegate
                        {
                            listBox1.Items.Clear();
                            listBox1.Items.AddRange(timeline.ToArray());
                            if (listBox1.Items.Count == 1)
                                listBox1.SelectedIndex = 0;
                        });
                    }
                    if (++scanned % 100 == 0)
                        Invoke((MethodInvoker)delegate { toolStripStatusLabel1.Text = String.Format("{0} files scanned.", scanned); });
                    lock (timeline)
                    {
                        timelineEntry.files.Add((FileInfo) fileSystemInfo);
                    }

                    if (timelineEntry == currentlySelected)
                    {
                        Invoke((MethodInvoker)delegate { listBox2.Items.Add(fileSystemInfo.FullName); });
                    }
                }
            }

        }

        private List<TimelineDate> timeline;
        private long scanned;
        private TimelineDate currentlySelected;

        private TimelineDate GetTimelineEntry(DateTime dt)
        {
            TimelineDate timelineDate = timeline.Find(x => x.Date.Equals(dt));
            return timelineDate;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            TimelineDate date = (TimelineDate)listBox1.SelectedItem;
            if (date == null)
                return;
            date.ForeColor = Color.Black;
            if (date == null)
                return;

            lock (timeline)
            {
                foreach (FileInfo dateFile in date.files)
                {
                    listBox2.Items.Add(dateFile.FullName);
                }
            }
        }
    }
}
