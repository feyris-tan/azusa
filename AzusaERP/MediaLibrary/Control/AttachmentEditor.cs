using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using moe.yo3explorer.azusa.MediaLibrary.Entity;
using moe.yo3explorer.azusa.Properties;

namespace moe.yo3explorer.azusa.MediaLibrary.Control
{
    public partial class AttachmentEditor : Form
    {
        private readonly Media _currentMedia;
        private readonly AzusaContext _context;
        private static Dictionary<int, AttachmentType> attachmentTypes;
        private ISidecarDisplayControl currentControl;
        private Attachment currentAttachment;

        public AttachmentEditor(Media currentMedia, AzusaContext context)
        {
            _currentMedia = currentMedia;
            _context = context;
            InitializeComponent();
            if (attachmentTypes == null)
            {
                attachmentTypes = context.DatabaseDriver.GetAllMediaAttachmentTypes().ToDictionary(x => x.id);
            }

            Dictionary<int, AttachmentType> missingAttachmentTypes = new Dictionary<int, AttachmentType>();
            foreach (var keyValuePair in attachmentTypes)
                missingAttachmentTypes.Add(keyValuePair.Key, keyValuePair.Value);

            imageList1.Images.Add(Resources.Find_VS);
            imageList1.Images.Add(Resources.accept);
            imageList1.Images.Add(Resources.link_break);

            foreach (Attachment attachment in context.DatabaseDriver.GetAllMediaAttachments(currentMedia))
            {
                attachment._IsInDatabase = true;
                attachment.Text = attachmentTypes[attachment._TypeId].name;
                listView1.Items.Add(attachment);
                missingAttachmentTypes.Remove(attachment._TypeId);
            }

            foreach (AttachmentType attachmentType in missingAttachmentTypes.Values)
            {
                Attachment added = new Attachment();
                added._IsInDatabase = false;
                added._MediaId = currentMedia.Id;
                added._TypeId = attachmentType.id;
                added._Complete = false;
                added.Text = attachmentType.name;
                listView1.Items.Add(added);
            }

            listView1.SelectedIndices.Add(0);
            Text = String.Format("Anhänge für {0}", currentMedia.Name);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Abort;
            this.Close();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            if (listView1.SelectedItems.Count == 0)
                return;

            if (currentControl != null)
            {
                currentControl.OnDataChanged -= CurrentControl_OnDataChanged;
            }

            currentAttachment = (Attachment)listView1.SelectedItems[0];
            AttachmentType attachmentType = attachmentTypes[currentAttachment._TypeId];
            Type controlType = _context.SidecarDisplayControls[attachmentType.displayControlUuid];
            ConstructorInfo constructor = controlType.GetConstructor(new Type[0]);
            currentControl = (ISidecarDisplayControl)constructor.Invoke(new object[0]);
            currentControl.Data = currentAttachment._Buffer;
            currentControl.MediumId = _currentMedia.Id;
            currentControl.ForceEnabled();
            currentControl.OnDataChanged += CurrentControl_OnDataChanged;

            System.Windows.Forms.Control control = currentControl.ToControl();
            control.Dock = DockStyle.Fill;
            control.AllowDrop = true;
            control.Enabled = true;
            panel1.Controls.Add(control);
        }

        private void CurrentControl_OnDataChanged(byte[] data, bool complete, int mediumId)
        {
            currentAttachment._Buffer = data;
            currentAttachment._Complete = complete;
            currentAttachment._DateUpdated = DateTime.Now;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem listView1Item in listView1.Items)
            {
                Attachment attachment = (Attachment) listView1Item;
                if (attachment._Complete)
                {
                    if (attachment._IsInDatabase)
                    {
                        //Vollständig und schon in DB, also UPDATE
                        _context.DatabaseDriver.UpdateAttachment(attachment);
                    }
                    else
                    {
                        //Vollständig und noch nicht in DB, also INSERT.
                        _context.DatabaseDriver.InsertAttachment(attachment);
                    }
                }
                else
                {
                    if (attachment._IsInDatabase)
                    {
                        //Fehlt, ist aber in DB, also DELETE
                        _context.DatabaseDriver.DeleteAttachment(attachment);
                    }
                    else
                    {
                        //Fehlt, und ist auch nicht in DB, also NOP.
                    }
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
