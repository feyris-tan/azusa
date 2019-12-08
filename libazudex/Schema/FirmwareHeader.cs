using System;
using System.IO;
using System.Xml;

namespace moe.yo3explorer.azusa.dex.Schema
{
    public class FirmwareHeader
    {
        internal FirmwareHeader(byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            XmlReader xr = XmlReader.Create(ms);
            xr.Read();
            _schemaVersion = Int32.Parse(xr.GetAttribute("SchemaVersion"));
            _apiVersion = Version.Parse(xr.GetAttribute("ApiVersion"));
            _testApiVersion = Version.Parse(xr.GetAttribute("TestApiVersion"));
            _productId = xr.GetAttribute("ProductId");
            _productName = xr.GetAttribute("ProductName");
            _softwareNumber = xr.GetAttribute("SoftwareNumber");
            _firmwareVersion = Version.Parse(xr.GetAttribute("FirmwareVersion"));
            _portVersion = Version.Parse(xr.GetAttribute("PortVersion"));
            _rfVersion = Version.Parse(xr.GetAttribute("RFVersion"));
            _dexBootVersion = Int32.Parse(xr.GetAttribute("DexBootVersion"));
            ms.Dispose();
        }

        private int _schemaVersion;
        private Version _apiVersion;
        private Version _testApiVersion;
        private string _productId;
        private string _productName;
        private string _softwareNumber;
        private Version _firmwareVersion;
        private Version _portVersion;
        private Version _rfVersion;
        private int _dexBootVersion;

        public int SchemaVersion
        {
            get { return _schemaVersion; }
        }

        public Version ApiVersion
        {
            get { return _apiVersion; }
        }

        public Version TestApiVersion
        {
            get { return _testApiVersion; }
        }

        public string ProductId
        {
            get { return _productId; }
        }

        public string ProductName
        {
            get { return _productName; }
        }

        public string SoftwareNumber
        {
            get { return _softwareNumber; }
        }

        public Version FirmwareVersion
        {
            get { return _firmwareVersion; }
        }

        public Version PortVersion
        {
            get { return _portVersion; }
        }

        public Version RfVersion
        {
            get { return _rfVersion; }
        }

        public int DexBootVersion
        {
            get { return _dexBootVersion; }
        }
    }
}