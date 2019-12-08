using System.Windows.Forms;

namespace moe.yo3explorer.azusa.MediaLibrary.Entity
{
    public class MediaInProduct : ListViewItem
    {
        private int _id, _mediaTypeId;
        private string _name;

        public int MediaId
        {
            get { return _id; }
            set { _id = value; }
        }

        public int MediaTypeId
        {
            get { return _mediaTypeId; }
            set
            {
                _mediaTypeId = value;
                ImageIndex = value;
            }
        }

        public string MediaName
        {
            get { return _name; }
            set
            {
                _name = value;
                Text = value;
            }
        }
    }
}
