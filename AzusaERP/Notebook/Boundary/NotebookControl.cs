using System;
using System.Linq;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.DatabaseIO;
using moe.yo3explorer.azusa.Notebook.Control;
using moe.yo3explorer.azusa.Notebook.Entity;

namespace moe.yo3explorer.azusa.Notebook.Boundary
{
    public partial class NotebookControl : UserControl, IAzusaModule
    {
        private AzusaContext context;

        public NotebookControl()
        {
            InitializeComponent();
        }

        public string IniKey => "notebook";
        public string Title => "Notizblock";

        public System.Windows.Forms.Control GetSelf()
        {
            return this;
        }

        public void OnLoad()
        {
            context = AzusaContext.GetInstance();
            context.Splash.SetLabel("Lade Notizblock...");

            IDatabaseDriver database = context.DatabaseDriver;
            Note[] notes = database.Notebook_GetAllNotes().ToArray();
            NodeTreeViewItem[] nodes = Array.ConvertAll(notes, x => new NodeTreeViewItem(x));

            foreach (NodeTreeViewItem node in nodes)
            {
                if (node.ParentId.HasValue)
                {
                    NodeTreeViewItem parent = nodes.First(x => x.DatabaseId == node.ParentId);
                    parent.Nodes.Add(node);
                }
                else
                {
                    treeView1.Nodes.Add(node);
                }
            }
        }

        public int Priority => 4;

        private int GetCurrentNoteId()
        {
            TreeNode treeView1SelectedNode = treeView1.SelectedNode;
            NodeTreeViewItem node = (NodeTreeViewItem)treeView1SelectedNode;
            return node.DatabaseId;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                NodeTreeViewItem node = (NodeTreeViewItem) e.Node;
                neueKategorieToolStripMenuItem.Enabled = node.IsCategory;
                neueNotizToolStripMenuItem.Enabled = node.IsCategory;
                richTextBox1.Enabled = !node.IsCategory;
                speichernToolStripMenuItem.Enabled = !node.IsCategory;
                if (!node.IsCategory)
                {
                    richTextBox1.Text = context.DatabaseDriver.Notebook_GetRichText(GetCurrentNoteId());
                }
                else
                {
                    richTextBox1.Text = node.Text;
                }
            }
            else
            {
                neueKategorieToolStripMenuItem.Enabled = true;
                neueNotizToolStripMenuItem.Enabled = true;
                richTextBox1.Enabled = false;
                speichernToolStripMenuItem.Enabled = false;
            }
        }

        private void neueKategorieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = TextInputForm.Prompt("Name der neuen Kategorie?", this.FindForm());
            if (string.IsNullOrEmpty(name))
                return;

            CreateNewItem(true, name);
        }

        private void neueNotizToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = TextInputForm.Prompt("Name der neuen Notiz?", this.FindForm());
            if (string.IsNullOrEmpty(name))
                return;

            CreateNewItem(false, name);
        }

        private void CreateNewItem(bool isCategory, string name)
        {
            TreeNode treeNode = treeView1.SelectedNode;
            NodeTreeViewItem node = null;
            if (treeNode != null)
                node = (NodeTreeViewItem)treeNode;

            Note newCategory = context.DatabaseDriver.Notebook_CreateNote(name, isCategory, node?.DatabaseId);
            NodeTreeViewItem newNode = new NodeTreeViewItem(newCategory);

            if (node == null)
                treeView1.Nodes.Add(newNode);
            else
                node.Nodes.Add(newNode);
        }

        private void speichernToolStripMenuItem_Click(object sender, EventArgs e)
        {
            context.DatabaseDriver.Notebook_UpdateNote(GetCurrentNoteId(), richTextBox1.Text);
        }
    }
}
