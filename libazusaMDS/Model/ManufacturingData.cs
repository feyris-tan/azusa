using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace moe.yo3explorer.azusa.mds.Model
{
    public class ManufacturingData
    {
        internal ManufacturingData(byte[] buffer, int pointer)
        {
            manufacturingData = new byte[2048];
            Array.Copy(buffer, pointer, manufacturingData, 0, 2048);
        }

        private byte[] manufacturingData;

        public uint Checksum
        {
            get
            {
                uint result = 0;
                for (int i = 0; i < 2048; i++)
                {
                    result += manufacturingData[i];
                }
                return result;
            }
        }
    }
}
