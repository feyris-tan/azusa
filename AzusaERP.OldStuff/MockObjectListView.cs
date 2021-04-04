using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrightIdeasSoftware
{
    class TreeListView : Control
    {
        public List<OLVColumn> AllColumns { get; set; }
        public bool CellEditUseWholeCell { get; set; }
        public List<ColumnHeader> Columns { get; set; }
        public bool HideSelection { get; set; }
        public ImageList LargeImageList { get; set; }
        public bool MultiSelect { get; set; }
        public bool ShowGroups { get; set; }
        public ImageList SmallImageList { get; set; }
        public ImageList StateImageList { get; set; }
        public bool UseCompatibleStateImageBehavior { get; set; }
        public View View { get; set; }
        public bool VirtualMode { get; set; }
        public CanExpandGetterDelegate CanExpandGetter { get; set; }
        public ChildrenGetterDelegate ChildrenGetter { get; set; }
        public object SelectedObject { get; set; }

        public event SelectionChangedDelegate SelectionChanged;

        public void SetObjects(object p0)
        {
            throw new NotImplementedException();
        }

        public delegate bool CanExpandGetterDelegate(object model);

        public delegate IEnumerable ChildrenGetterDelegate(object model);

        public delegate void SelectionChangedDelegate(object sender, EventArgs e);
    }

    class OLVColumn : ColumnHeader
    {
        public string AspectName { get; set; }
        public ImageGetterDelegate ImageGetter { get; set; }
    }

    public delegate object ImageGetterDelegate(object o);
}
