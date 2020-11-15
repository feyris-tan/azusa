using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class Attachment : ListViewItem
    {
        public Attachment()
        {
            this.ImageIndex = 2;
        }

        private int mediaId;
        private int typeId;
        private byte[] buffer;
        private DateTime dateAdded;
        private DateTime dateUpdated;
        private bool complete;
        public long _Serial;

        public int _MediaId
        {
            get => mediaId;
            set => mediaId = value;
        }

        public int _TypeId
        {
            get => typeId;
            set => typeId = value;
        }

        public byte[] _Buffer
        {
            get => buffer;
            set => buffer = value;
        }

        public DateTime _DateAdded
        {
            get => dateAdded;
            set => dateAdded = value;
        }

        public DateTime _DateUpdated
        {
            get => dateUpdated;
            set => dateUpdated = value;
        }

        public bool _Complete
        {
            get => complete;
            set
            {
                complete = value;
                ImageIndex = complete ? 1 : 2;
            }
        }

        public bool _IsInDatabase { get; set; }
    }
}
