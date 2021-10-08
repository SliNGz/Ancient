using ancient.game.world;
using ancient.game.world.block;
using ancient.game.world.chunk;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.lighting
{
    public class LightingManager
    {
        private World world;

        private Queue<LightNode> lightBlockList;

        public LightingManager(World world)
        {
            this.world = world;
            this.lightBlockList = new Queue<LightNode>();
        }

        public void Update()
        {
            List<Chunk> chunks = new List<Chunk>();

            while (lightBlockList.Count > 0)
            {
                LightNode vector = lightBlockList.Dequeue();
                int x = vector.X;
                int y = vector.Y;
                int z = vector.Z;

                Chunk chunk = world.GetChunkFromBlock(x, y, z);

                if (!chunks.Contains(chunk))
                    chunks.Add(chunk);

                int blocklight = world.GetBlocklight(x, y, z);

                UpdateLight(x, y - 1, z, blocklight - 1);
                UpdateLight(x, y + 1, z, blocklight - 1);
                UpdateLight(x - 1, y, z, blocklight - 1);
                UpdateLight(x + 1, y, z, blocklight - 1);
                UpdateLight(x, y, z - 1, blocklight - 1);
                UpdateLight(x, y, z + 1, blocklight - 1);
            }

            for (int i = 0; i < chunks.Count; i++)
                world.GetChunkLoader().AddToReloadSet(chunks[i].GetIndex());
        }

        private void UpdateLight(int x, int y, int z, int blocklight)
        {
            Block block = world.GetBlock(x, y, z);

            if (!block.IsSolid() && world.GetBlocklight(x, y, z) + 1 < blocklight)
            {
                world.SetBlocklight(x, y, z, blocklight);
                lightBlockList.Enqueue(new LightNode(x, y, z, blocklight));
            }
        }

        public void AddLightSource(int x, int y, int z, int blocklight)
        {
            lightBlockList.Enqueue(new LightNode(x, y, z, blocklight));
            world.SetBlocklight(x, y, z, blocklight);
        }
    }

    public struct LightNode
    {
        public int X;
        public int Y;
        public int Z;
        public int LightValue;

        public LightNode(int x, int y, int z, int lightValue)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.LightValue = lightValue;
        }
    }
}
