using System.Collections.Generic;

namespace libazusax360
{
    internal class BinaryTree
    {
        // Fields
        public int leftNodeId;
        public int leftNodeOffset;
        public int listposition;
        public int offset;
        public int rightNodeId;
        public int rightNodeOffset;

        // Methods
        public static int findTreeCentre(int maxcentre, int totalsize)
        {
            int num = 1;
            int num2 = 1;
            while (true)
            {
                num2 *= 2;
                if ((num + num2) > maxcentre)
                {
                    break;
                }
                num += num2;
            }
            int num3 = totalsize - (num * 2);
            if (num3 >= num2)
            {
                num += num2;
            }
            return num;
        }

        public int populate(string[] sortedList, int highLimit, int lowLimit, int startoffset, int centrepoint, List<BinaryTree> treeList)
        {
            this.offset = startoffset;
            this.listposition = centrepoint;
            BinaryTree tree = new BinaryTree();
            BinaryTree tree2 = new BinaryTree();
            int num = this.offset + sortedList[centrepoint].Length;
            num += 14;
            if ((num % 4) != 0)
            {
                num += 4 - (num % 4);
            }
            if (centrepoint > lowLimit)
            {
                int totalsize = (centrepoint - 1) - lowLimit;
                int maxcentre = totalsize / 2;
                if ((maxcentre * 2) != totalsize)
                {
                    maxcentre++;
                }
                if ((maxcentre != 0) && (maxcentre != 0))
                {
                    maxcentre = findTreeCentre(maxcentre, totalsize);
                }
                this.leftNodeId = lowLimit + maxcentre;
            }
            else
            {
                this.leftNodeId = -1;
            }
            if (centrepoint < highLimit)
            {
                int num4 = centrepoint + 1;
                int num5 = highLimit - num4;
                int num6 = num5 / 2;
                if (((num6 * 2) != num5) && (num6 != 0))
                {
                    num6++;
                }
                if (num6 != 0)
                {
                    num6 = findTreeCentre(num6, num5);
                }
                this.rightNodeId = num4 + num6;
            }
            else
            {
                this.rightNodeId = -1;
            }
            treeList.Add(this);
            int num7 = treeList.Count - 1;
            if (this.leftNodeId != -1)
            {
                treeList[num7].leftNodeOffset = num;
                num = tree.populate(sortedList, centrepoint - 1, lowLimit, num, this.leftNodeId, treeList);
            }
            else
            {
                treeList[num7].leftNodeOffset = 0;
            }
            if (this.rightNodeId != -1)
            {
                treeList[num7].rightNodeOffset = num;
                return tree2.populate(sortedList, highLimit, centrepoint + 1, num, this.rightNodeId, treeList);
            }
            treeList[num7].rightNodeOffset = 0;
            return num;
        }
    }
}