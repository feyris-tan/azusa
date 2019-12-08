using System;

namespace libazusax360
{
    public class SCSIDevice : IDisposable
    {
        public class DiscData
        {
            // Fields
            public uint EndSectorNumber;
            public uint Layer0LastSector;
            public uint Layer0LastSectorLBA;
            public short LayerCount;
            public uint StartSectorNumber;
            
            public DiscData(byte[] buffer, int offset)
            {
                this.LayerCount = (short)(((buffer[2 + offset] & 0x60) >> 5) + 1);
                byte[] destinationArray = new byte[4];
                Array.Copy(buffer, 5 + offset, destinationArray, 1, 3);
                Array.Reverse(destinationArray);
                this.StartSectorNumber = BitConverter.ToUInt32(destinationArray, 0);
                destinationArray = new byte[4];
                Array.Copy(buffer, 9 + offset, destinationArray, 1, 3);
                Array.Reverse(destinationArray);
                this.EndSectorNumber = BitConverter.ToUInt32(destinationArray, 0);
                destinationArray = new byte[4];
                Array.Copy(buffer, 13 + offset, destinationArray, 1, 3);
                Array.Reverse(destinationArray);
                this.Layer0LastSector = BitConverter.ToUInt32(destinationArray, 0);
                this.Layer0LastSectorLBA = BitConverter.ToUInt32(destinationArray, 0) - (this.StartSectorNumber - 1);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}