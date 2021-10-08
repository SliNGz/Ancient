using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.chunk;
using ancientlib.game.init;
using ancientlib.game.world.chunk;

namespace ancientlib.game.world.biome
{
    public class BiomeOcean : Biome
    {
        public BiomeOcean() : base("ocean")
        {
            this.maxHeight = 48;
            this.minHeight = 24;

            this.elevation = 2;
            this.roughness = 6.25F;
            this.detail = 16;

            this.topBlock = Blocks.dirt;
            this.baseBlock = Blocks.dirt;
        }

        public override void Generate(Chunk chunk, int x, int z, int height)
        {
            bool genScenery = false;

            BlockArray blocks = chunk.GetBlockArray();

            int wx = x + chunk.GetX();
            int wz = z + chunk.GetZ();

            int xzIndex = x * 16 * 16 + z;
            int sceneryHeight = height + 1;

            for (int y = 0; y < 16; y++)
            {
                int index = xzIndex + y * 16;

                int wy = chunk.GetY() + y;

                if (wy <= height)
                {
                    if (wy == height)
                    {
                        blocks.SetBlock(topBlock, index);

                        if (wy >= WATER_LEVEL)
                            genScenery = true;
                    }
                    else if (wy == height - 1)
                        blocks.SetBlock(baseBlock, index);
                    else
                        blocks.SetBlock(Blocks.stone, index);
                }
                else
                {
                    if (wy <= WATER_LEVEL)
                        blocks.SetBlock(Blocks.water, index);
                }
            }

            if (genScenery)
                GenerateScenery(chunk, x, sceneryHeight, z);

            GenerateClouds(chunk, wx, chunk.GetY(), wz);
        }

        public override void GenerateScenery(Chunk chunk, int x, int wy, int z)
        { }
    }
}
