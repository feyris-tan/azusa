using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace libazusax360
{
    public class FileWriter
    {
        public void SaveFileToDisk(XDvdFsFileSystemEntry sourceXDvdFsFile, string Destination)
        {
            int num;
            FileStream stream = new FileStream(Destination, FileMode.Create);
            Stream stream2 = sourceXDvdFsFile.GetStream();
            byte[] buffer = new byte[0x10000];
            do
            {
                num = stream2.Read(buffer, 0, 0x10000);
                stream.Write(buffer, 0, num);
            }
            while (num == 0x10000);
            stream.Close();
            stream2.Close();
        }
        
    }
}