using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.chunk;
using ancientlib.game.init;

namespace ancientlib.game.world.biome
{
    public class BiomeDesert : Biome
    {
        public BiomeDesert()
        {
            this.maxHeight = 80;
            this.minHeight = 60;

            this.elevation = 0.75F;
            this.roughness = 2;
            this.detail = 8;

            this.topBlock = Blocks.sand;
            this.baseBlock = Blocks.sand;
        }

        public override void GenerateScenery(Chunk chunk, int x, int wy, int z)
        { }
    }
}
