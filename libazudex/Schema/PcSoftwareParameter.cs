using System;
using System.IO;
using System.Xml;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class PcSoftwareParameter : GenericXmlRecord
    {
        public PcSoftwareParameter()
            : base()
        {

        }

        public PcSoftwareParameter(BinaryReader br)
            : base(br)
        {
            StringReader sr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(sr);
            xr.Read();
            _receiverId = Guid.Parse(xr.GetAttribute("ReceiverId"));
            _firmwareImageId = xr.GetAttribute("FirmwareImageID");
            _dateTimeCreated = DateTime.Parse(xr.GetAttribute("DateTimeCreated"));
            _nickname = xr.GetAttribute("Nickname");
        }

        private Guid _receiverId;
        private string _firmwareImageId;
        private DateTime _dateTimeCreated;
        private string _nickname;

        public Guid ReceiverId
        {
            get { return _receiverId; }
        }

        public string FirmwareImageId
        {
            get { return _firmwareImageId; }
        }

        public DateTime DateTimeCreated
        {
            get { return _dateTimeCreated; }
        }

        public string Nickname
        {
            get { return _nickname; }
        }
    }
}