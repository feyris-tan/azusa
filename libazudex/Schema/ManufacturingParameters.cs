using System;
using System.IO;
using System.Xml;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class ManufacturingParameters : GenericXmlRecord
    {
        public ManufacturingParameters()
            : base()
        {

        }

        public ManufacturingParameters(BinaryReader br)
            : base(br)
        {
            StringReader sr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(sr);
            xr.Read();
            serialNumber = xr.GetAttribute("SerialNumber");
            hardwarePartNumber = xr.GetAttribute("HardwarePartNumber");
            hardwareRevision = xr.GetAttribute("HardwareRevision");
            dateTimeCreated = DateTime.Parse(xr.GetAttribute("DateTimeCreated"));
            hardwareId = Guid.Parse(xr.GetAttribute("HardwareId"));
        }
        private String serialNumber;
        private String hardwarePartNumber;
        private String hardwareRevision;
        private DateTime dateTimeCreated;
        private Guid hardwareId;

        public string SerialNumber
        {
            get { return serialNumber; }
        }

        public string HardwarePartNumber
        {
            get { return hardwarePartNumber; }
        }

        public string HardwareRevision
        {
            get { return hardwareRevision; }
        }

        public DateTime DateTimeCreated
        {
            get { return dateTimeCreated; }
        }

        public Guid HardwareId
        {
            get { return hardwareId; }
        }
    }
}