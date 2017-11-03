using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world.chunk;
using ancientlib.game.world.chunk;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.generator.metadata
{
    public class Metadata
    {
        private Dictionary<Tuple<int, int, int>, List<Tuple<Block, int>>> metadataBlocks = new Dictionary<Tuple<int, int, int>, List<Tuple<Block, int>>>();

        public void AddBlockToMetadata(Block block, int x, int y, int z)
        {
            int xChunk = (int)Math.Floor(x / 16f);
            int yChunk = (int)Math.Floor(y / 16f);
            int zChunk = (int)Math.Floor(z / 16f);
            Tuple<int, int, int> key = new Tuple<int, int, int>(xChunk, yChunk, zChunk);

            x = (x % 16 + 16) % 16;
            y = (y % 16 + 16) % 16;
            z = (z % 16 + 16) % 16;

            int index = x * 16 * 16 + y * 16 + z;

            List<Tuple<Block, int>> blocks;

            if (metadataBlocks.TryGetValue(key, out blocks))
                blocks.Add(new Tuple<Block, int>(block, index));
            else
            {
                blocks = new List<Tuple<Block, int>>();
                blocks.Add(new Tuple<Block, int>(block, index));
                metadataBlocks.Add(key, blocks);
            }
        }

        public void LoadMetadata(Chunk chunk)
        {
            Vector3 index = chunk.GetIndex();
            Tuple<int, int, int> key = new Tuple<int, int, int>((int)index.X, (int)index.Y, (int)index.Z);

            List<Tuple<Block, int>> blocks;

            if (metadataBlocks.TryGetValue(key, out blocks))
            {
                foreach (Tuple<Block, int> metadataBlock in blocks)
                {
                    if (metadataBlock != null)
                        chunk.GetBlockArray().SetBlock(metadataBlock.Item1, metadataBlock.Item2);
                }
            }
        }
    }
}