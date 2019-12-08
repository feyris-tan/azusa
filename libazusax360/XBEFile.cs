using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace libazusax360
{
    public class XBEFile
    {
        // Fields
        public XBECertificate certificate;
        public XBEFileHeader header;
        public byte[] RawRegionFlags = new byte[4];
        public RegionFlags RegionFlags;
        private XBESectionHeader[] XBESections;

        // Methods
        public XBEFile(Stream infile)
        {
            BinaryReader reader = new BinaryReader(infile);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            byte[] bytes = reader.ReadBytes(0x800);
            if (Encoding.ASCII.GetString(bytes, 0, 4) != "XBEH")
            {
                throw new SystemException("XBE FILE WITHOUT XBEH MAGIC!");
            }
            this.header = new XBEFileHeader(bytes);
            if (this.header.SizeOfHeaders > 0x800)
            {
                int sizeOfHeaders = (int)this.header.SizeOfHeaders;
                if ((sizeOfHeaders % 0x800) != 0)
                {
                    sizeOfHeaders += 0x800 - (sizeOfHeaders % 0x800);
                }
                reader.BaseStream.Seek(0L, SeekOrigin.Begin);
                bytes = reader.ReadBytes(sizeOfHeaders);
            }
            uint num2 = this.header.CertificateAddress - this.header.BaseAddress;
            this.certificate = new XBECertificate(bytes, (int)num2);
            this.RegionFlags = new RegionFlags();
            byte[] gameRegion = this.certificate.GameRegion;
            gameRegion.CopyTo(this.RawRegionFlags, 0);
            if (gameRegion[3] == 80)
            {
                this.RegionFlags.All = true;
            }
            if ((gameRegion[0] & 1) == 1)
            {
                this.RegionFlags.USA = true;
            }
            if (((gameRegion[0] & 2) >> 1) == 1)
            {
                this.RegionFlags.Japan = true;
                this.RegionFlags.China = true;
                this.RegionFlags.RestOfAsia = true;
            }
            if (((gameRegion[0] & 4) >> 2) == 1)
            {
                this.RegionFlags.Australia = true;
                this.RegionFlags.RestOfEurope = true;
            }
            uint num3 = this.header.SectionHeadersAddress - this.header.BaseAddress;
            this.XBESections = new XBESectionHeader[this.header.NumSections];
            int num4 = Marshal.SizeOf(typeof(XBESectionHeader));
            for (int i = 0; i < this.header.NumSections; i++)
            {
                this.XBESections[i] = new XBESectionHeader(bytes, ((int)num3) + (num4 * i));
            }
        }
        
        // Nested Types
        public class XBECertificate
        {
            // Fields
            public byte[][] AltKeys = new byte[0x10][];
            public uint CertificateSize;
            public uint CertificateVersion;
            public byte[] DateTime = new byte[4];
            public uint DiscNumber;
            public byte[] GameRatings = new byte[4];
            public byte[] GameRegion = new byte[4];
            public byte[] LanKey = new byte[0x10];
            public byte[] MediaFlags = new byte[4];
            public byte[] SignatureKey = new byte[0x10];
            public uint TitleID;
            public string TitleName;

            // Methods
            public XBECertificate(byte[] buffer, int CertificateOffset)
            {
                byte[] destinationArray = new byte[4];
                Array.Copy(buffer, CertificateOffset, destinationArray, 0, 4);
                this.CertificateSize = BitConverter.ToUInt32(destinationArray, 0);
                Array.Copy(buffer, 4 + CertificateOffset, this.DateTime, 0, 4);
                this.TitleID = BitConverter.ToUInt32(buffer, 8 + CertificateOffset);
                this.TitleName = Encoding.Unicode.GetString(buffer, 12 + CertificateOffset, 80);
                char[] chArray2 = new char[2];
                chArray2[1] = '\n';
                char[] trimChars = chArray2;
                this.TitleName = this.TitleName.Trim(trimChars);
                uint[] numArray = new uint[0x10];
                for (int i = 0; i < 0x10; i++)
                {
                    numArray[i] = BitConverter.ToUInt32(buffer, (0x5c + (4 * i)) + CertificateOffset);
                }
                Array.Copy(buffer, 0x9c + CertificateOffset, this.MediaFlags, 0, 4);
                Array.Copy(buffer, 160 + CertificateOffset, this.GameRegion, 0, 4);
                Array.Copy(buffer, 0xa4 + CertificateOffset, this.GameRatings, 0, 4);
                Array.Copy(buffer, 0xa8 + CertificateOffset, destinationArray, 0, 4);
                this.DiscNumber = BitConverter.ToUInt32(destinationArray, 0);
                Array.Copy(buffer, 0xac + CertificateOffset, destinationArray, 0, 4);
                this.CertificateVersion = BitConverter.ToUInt32(destinationArray, 0);
                Array.Copy(buffer, 0xb0 + CertificateOffset, this.LanKey, 0, 0x10);
                Array.Copy(buffer, 0xc0 + CertificateOffset, this.SignatureKey, 0, 0x10);
                for (int j = 0; j < 0x10; j++)
                {
                    this.AltKeys[j] = new byte[0x10];
                    Array.Copy(buffer, ((j * 0x10) + 0xd0) + CertificateOffset, this.AltKeys[j], 0, 0x10);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class XBEFileHeader
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Magic = new byte[4];
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
            public byte[] Signature = new byte[0x100];
            public uint BaseAddress;
            public uint SizeOfHeaders;
            public uint SizeOfImage;
            public uint SizeOfImageHeader;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] DateTime = new byte[4];
            public uint CertificateAddress;
            public uint NumSections;
            public uint SectionHeadersAddress;
            public uint InitFlags;
            public uint EntryPoint;
            public uint TLSAddress;
            public uint PEStackCommit;
            public uint PEHeapReserve;
            public uint PEHeapCommit;
            public uint PEBaseAddress;
            public uint PESize;
            public uint PEChecksum;
            public uint PEDateTime;
            public uint PathnameAddress;
            public uint FilenameAddress;
            public uint UnicodeFilenameAddress;
            public uint KernelThunkAddress;
            public uint NonKernelImportAddress;
            public uint NumLibraryVersions;
            public uint LibraryVersionsAddress;
            public uint KernelLibraryVersionAddress;
            public uint XAPILibraryVersionAddress;
            public uint LogoBitmapAddress;
            public uint LogoBitmapSize;

            public XBEFileHeader(byte[] buffer)
            {
                int cb = Marshal.SizeOf(typeof(XBEFile.XBEFileHeader));
                IntPtr destination = Marshal.AllocHGlobal(cb);
                Marshal.Copy(buffer, 0, destination, cb);
                Marshal.PtrToStructure(destination, this);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private class XBESectionHeader
        {
            public uint SectionFlags;
            public uint VirtualAddress;
            public uint VirtualSize;
            public uint RawAddress;
            public uint RawSize;
            public uint SectionNameAddress;
            public uint SectionNameReferenceCount;
            public uint HeadSharedPageReferenceCountAddress;
            public uint TailSharedPageReferenceCountAddress;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] SectionDigest = new byte[20];

            public XBESectionHeader(byte[] buffer, int startoffset)
            {
                int cb = Marshal.SizeOf(typeof(XBEFile.XBESectionHeader));
                IntPtr destination = Marshal.AllocHGlobal(cb);
                Marshal.Copy(buffer, startoffset, destination, cb);
                Marshal.PtrToStructure(destination, this);
            }
        }
    }
}