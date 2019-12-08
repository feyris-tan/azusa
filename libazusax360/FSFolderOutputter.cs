using System;
using System.Collections.Generic;
using System.Text;

namespace libazusax360
{
    internal class FSFolderOutputter
    {
        // Methods
        private int alignTree(FileSystemFolder fsfolder, List<BinaryTree> treelist, Dictionary<string, int> fsDictionary, string[] fsArray)
        {
            bool flag = true;
            int num = 0;
            while (flag)
            {
                int num2 = 1;
                flag = false;
                foreach (BinaryTree tree in treelist)
                {
                    string str = fsArray[tree.listposition];
                    XDvdFsFileSystemEntry entry = fsfolder[fsDictionary[str]];
                    if ((tree.offset < (0x800 * num2)) && ((tree.offset + (entry.FileName.Length + 14)) > (0x800 * num2)))
                    {
                        int offset = tree.offset;
                        int num4 = (0x800 * num2) - tree.offset;
                        num += num4;
                        foreach (BinaryTree tree2 in treelist)
                        {
                            if (tree2.offset >= offset)
                            {
                                tree2.offset += num4;
                            }
                            if (tree2.leftNodeOffset >= offset)
                            {
                                tree2.leftNodeOffset += num4;
                            }
                            if (tree2.rightNodeOffset >= offset)
                            {
                                tree2.rightNodeOffset += num4;
                            }
                        }
                        flag = true;
                        num2++;
                    }
                }
            }
            return num;
        }

        public byte[] DirContentsToByteArray(FileSystemFolder fsfolder)
        {
            if (fsfolder.Count == 0)
            {
                return new byte[0];
            }
            Dictionary<string, int> fsDictionary = new Dictionary<string, int>();
            List<string> list = new List<string>(fsfolder.Count);
            for (int i = 0; i < fsfolder.Count; i++)
            {
                if (fsfolder[i].CopyMethod != OutputMethod.Special)
                {
                    fsDictionary.Add(fsfolder[i].FileName, i);
                    list.Add(fsfolder[i].FileName);
                }
            }
            string[] array = list.ToArray();
            Array.Sort<string>(array, new FSFilenameComparer());
            BinaryTree tree = new BinaryTree();
            int maxcentre = (array.Length - 1) / 2;
            if (maxcentre != 0)
            {
                maxcentre = BinaryTree.findTreeCentre(maxcentre, array.Length - 1);
            }
            List<BinaryTree> treeList = new List<BinaryTree>();
            int num3 = tree.populate(array, array.Length - 1, 0, 0, maxcentre, treeList);
            if (num3 > 0x800)
            {
                num3 += this.alignTree(fsfolder, treeList, fsDictionary, array);
            }
            byte[] buffer = new byte[num3];
            for (int j = 0; j < num3; j++)
            {
                buffer[j] = 0xff;
            }
            foreach (BinaryTree tree2 in treeList)
            {
                string str = array[tree2.listposition];
                XDvdFsFileSystemEntry entry = fsfolder[fsDictionary[str]];
                tree2.leftNodeOffset /= 4;
                tree2.rightNodeOffset /= 4;
                BitConverter.GetBytes((short)tree2.leftNodeOffset).CopyTo(buffer, tree2.offset);
                BitConverter.GetBytes((short)tree2.rightNodeOffset).CopyTo(buffer, (int)(tree2.offset + 2));
                BitConverter.GetBytes(entry.StartSector).CopyTo(buffer, (int)(tree2.offset + 4));
                if (entry.IsFolder)
                {
                    BitConverter.GetBytes((uint)entry.Files.LogicalDirTableSize).CopyTo(buffer, (int)(tree2.offset + 8));
                }
                else
                {
                    BitConverter.GetBytes((uint)entry.LogicalFileSize).CopyTo(buffer, (int)(tree2.offset + 8));
                }
                BitConverter.GetBytes((short)entry.Attributes).CopyTo(buffer, (int)(tree2.offset + 12));
                BitConverter.GetBytes((short)((byte)entry.FileName.Length)).CopyTo(buffer, (int)(tree2.offset + 13));
                ASCIIEncoding encoding = new ASCIIEncoding();
                encoding.GetBytes(entry.FileName).CopyTo(buffer, (int)(tree2.offset + 14));
            }
            return buffer;
        }
    }
}