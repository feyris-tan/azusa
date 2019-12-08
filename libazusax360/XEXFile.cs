using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace libazusax360
{
    public class XEXFile
    {
        // Fields
        public Bootflags BootFlags;
        public byte[] MediaID = new byte[0x10];
        public byte[] RawRegionFlags = new byte[4];
        public RegionFlags RegionFlags;

        // Methods
        public XEXFile(Stream infile)
        {
            BinaryReader reader = new BinaryReader(infile);
            reader.BaseStream.Seek(0L, SeekOrigin.Begin);
            byte[] bytes = reader.ReadBytes(0x800);
            byte[] destinationArray = new byte[4];
            if (Encoding.ASCII.GetString(bytes, 0, 4) != "XEX2")
            {
                throw new SystemException("XEX FILE WITHOUT XEX2 MAGIC!");
            }
            Array.Copy(bytes, 0x10, destinationArray, 0, 4);
            Array.Reverse(destinationArray);
            int num = BitConverter.ToInt32(destinationArray, 0);
            Array.Copy(bytes, num + 320, this.MediaID, 0, 0x10);
            Array.Copy(bytes, num + 0x178, destinationArray, 0, 4);
            destinationArray.CopyTo(this.RawRegionFlags, 0);
            this.RegionFlags = new RegionFlags();
            if (((destinationArray[0] == 0xff) && (destinationArray[1] == 0xff)) && ((destinationArray[2] == 0xff) && (destinationArray[3] == 0xff)))
            {
                this.RegionFlags.All = true;
            }
            if (destinationArray[0] == 0xff)
            {
                this.RegionFlags.RestOfWorld = true;
            }
            if ((destinationArray[1] & 1) == 1)
            {
                this.RegionFlags.Australia = true;
            }
            if ((destinationArray[1] & 0xfe) == 0xfe)
            {
                this.RegionFlags.RestOfEurope = true;
            }
            if ((destinationArray[2] & 1) == 1)
            {
                this.RegionFlags.Japan = true;
            }
            if ((destinationArray[2] & 2) == 2)
            {
                this.RegionFlags.China = true;
            }
            if ((destinationArray[2] & 0xfc) == 0xfc)
            {
                this.RegionFlags.RestOfAsia = true;
            }
            if (destinationArray[3] == 0xff)
            {
                this.RegionFlags.USA = true;
            }
            Array.Copy(bytes, num + 380, destinationArray, 0, 4);
            int num2 = (((destinationArray[0] << 0x18) + (destinationArray[1] << 0x10)) + (destinationArray[2] << 8)) + destinationArray[3];
            if (num2 != 4)
            {
                Trace.WriteLine("XEX found with strange mediaflags!");
            }
            this.BootFlags = new Bootflags();
            Trace.WriteLine("XEX with mediaflags value of " + num2.ToString());
            if ((num2 & 1) == 1)
            {
                this.BootFlags.HardDisk = true;
                Trace.WriteLine("Boots from Hard disk");
            }
            if ((num2 & 2) == 2)
            {
                this.BootFlags.DVDX2 = true;
                Trace.WriteLine("Boots from XBOX disk");
            }
            if ((num2 & 4) == 4)
            {
                this.BootFlags.DVDCD = true;
                Trace.WriteLine("Boots from DVD/CD");
            }
            if ((num2 & 8) == 8)
            {
                this.BootFlags.DVD5 = true;
                Trace.WriteLine("Boots from DVD5");
            }
            if ((num2 & 0x10) == 0x10)
            {
                this.BootFlags.DVD9 = true;
                Trace.WriteLine("Boots from DVD9");
            }
            if ((num2 & 0x20) == 0x20)
            {
                this.BootFlags.SysFlash = true;
                Trace.WriteLine("Boots from system flash memory");
            }
            if ((num2 & 0x80) == 0x80)
            {
                this.BootFlags.MemUnit = true;
                Trace.WriteLine("Boots from memory unit");
            }
            if ((num2 & 0x100) == 0x100)
            {
                this.BootFlags.MassStorage = true;
                Trace.WriteLine("Boots from USB Mass storage device");
            }
            if ((num2 & 0x200) == 0x200)
            {
                this.BootFlags.SMBFilesystem = true;
                Trace.WriteLine("Boots from SMB filesystem");
            }
            if ((num2 & 0x400) == 0x400)
            {
                this.BootFlags.DirectfromRam = true;
                Trace.WriteLine("Boots from ram");
            }
            if ((num2 & 0x1000000) == 0x1000000)
            {
                this.BootFlags.InsecurePackage = true;
                Trace.WriteLine("Is an insecure package?");
            }
            if ((num2 & 0x2000000) == 0x2000000)
            {
                this.BootFlags.SaveGamePackage = true;
                Trace.WriteLine("Is a savegame package?");
            }
            if ((num2 & 0x4000000) == 0x4000000)
            {
                this.BootFlags.LocallySigned = true;
                Trace.WriteLine("Is locally signed");
            }
            if ((num2 & 0x8000000) == 0x8000000)
            {
                this.BootFlags.LiveSigned = true;
                Trace.WriteLine("Is XboxLive Signed");
            }
            if ((num2 & 0x10000000) == 0x10000000)
            {
                this.BootFlags.XboxPlatformPackage = true;
                Trace.WriteLine("Is XboxPlatformPackage");
            }
        }
    }
}