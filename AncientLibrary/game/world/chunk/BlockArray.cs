using ancient.game.world.block;
using ancient.game.world.chunk;
using ancientlib.game.init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.chunk
{
    public class BlockArray
    {
        private Chunk chunk;

        private byte[] blocks;
        private int blocksNum;

        private int ticks;
        private List<int> blocksToUpdate;

        public BlockArray(Chunk chunk)
        {
            this.chunk = chunk;
            this.blocks = new byte[4096];
            this.blocksToUpdate = new List<int>();
        }

        public void Update()
        {
            ticks++;

            for (int i = 0; i < blocksToUpdate.Count; i++)
            {
                int index = blocksToUpdate[i];
                Block block = GetBlock(index);

                if (block.GetTickRate() > 0 && ticks % block.GetTickRate() == 0)
                {
                    int x = chunk.GetX() + index / (16 * 16);
                    int y = chunk.GetY() + (index % (16 * 16)) / 16;
                    int z = chunk.GetZ() + index % 16;

                    block.OnTick(chunk.GetWorld(), x, y, z);
                }
            }
        }

        public Block GetBlock(int x, int y, int z)
        {
            return GetBlock(x * 16 * 16 + y * 16 + z);
        }

        public void SetBlock(Block block, int x, int y, int z)
        {
            SetBlock(block, x * 16 * 16 + y * 16 + z);
        }

        public void DestroyBlock(int x, int y, int z)
        {
            DestroyBlock(x * 16 * 16 + y * 16 + z);
        }

        public Block GetBlock(int index)
        {
            return Blocks.GetBlockFromID(this.blocks[index]);
        }

        public void SetBlock(Block block, int index)
        {
            lock (blocks)
            {
                this.blocks[index] = (byte)Blocks.GetIDFromBlock(block);

                if (!(block is BlockAir))
                    this.blocksNum++;

                if (block.GetTickRate() > 0)
                    blocksToUpdate.Add(index);
            }
        }

        public void DestroyBlock(int index)
        {
            lock (blocks)
            {
                this.blocks[index] = (byte)Blocks.GetIDFromBlock(Blocks.air);
                this.blocksNum--;

                blocksToUpdate.Remove(index);
            }
        }

        public int GetNumberOfBlocks()
        {
            return this.blocksNum;
        }
    }
}
