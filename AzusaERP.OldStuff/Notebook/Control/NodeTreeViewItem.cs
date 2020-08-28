using System.Windows.Forms;
using moe.yo3explorer.azusa.Notebook.Entity;

namespace moe.yo3explorer.azusa.Notebook.Control
{
    class NodeTreeViewItem : TreeNode
    {
        public NodeTreeViewItem(Note note)
        {
            this.note = note;
            if (note.isCategory)
            {
                this.ImageIndex = 1;
                this.SelectedImageIndex = 0;
            }
            else
            {
                this.ImageIndex = 2;
                this.SelectedImageIndex = 2;
            }

            this.Text = note.name;
            this.Name = note.id.ToString();
        }

        private Note note;

        public int DatabaseId => note.id;
        public int? ParentId => note.parent;
        public bool IsCategory => note.isCategory;
    }
}
