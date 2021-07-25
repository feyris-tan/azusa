using System;
using System.IO;
using System.Windows.Forms;
using moe.yo3explorer.azusa.Control.FilesystemMetadata.Entity;

namespace moe.yo3explorer.azusa.Control.FilesystemMetadata.Boundary
{
    class FilesystemMetadataTreeViewItem : TreeNode
    {
        public FilesystemMetadataTreeViewItem(FilesystemMetadataEntity entity)
        {
            Entity = entity;
            this.Text = Path.GetFileName(entity.FullName);
            if (string.IsNullOrEmpty(this.Text))
            {
                string[] vines = entity.FullName.Split(new char[] {'\\'}, StringSplitOptions.RemoveEmptyEntries);
                if (vines.Length == 0)
                {
                    this.Text = "/";
                }
                else
                {
                    this.Text = vines[vines.GetUpperBound(0)];
                }
            }
            if (entity.ParentId == -1)
            {
                //Root
                ImageIndex = 0;
            }
            else if (entity.IsDirectory)
            {
                ImageIndex = 1;
                SelectedImageIndex = 3;
            }
            else
            {
                //File
                ImageIndex = 2;
                SelectedImageIndex = 2;
            }
        }

        public FilesystemMetadataEntity Entity { get; private set; }

        
    }
}
