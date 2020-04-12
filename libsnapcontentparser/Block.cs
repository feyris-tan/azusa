using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moe.yo3explorer.azusa.snapcontentparser
{

    public class Block
    {
        public Block(BlockState? blockState, byte[] hash)
        {
            this.BlockState = blockState;
            this.Hash = hash;
        }

        public BlockState? BlockState { get; private set; }
        public byte[] Hash { get; private set; }
    }
}
