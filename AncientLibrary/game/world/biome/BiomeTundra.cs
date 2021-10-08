using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.chunk;
using ancientlib.game.init;

namespace ancientlib.game.world.biome
{
    public class BiomeTundra : Biome
    {
        public BiomeTundra() : base("tundra")
        {
            this.maxHeight = 144;
            this.minHeight = 64;

            this.elevation = 0.25F;
            this.roughness = 2;
            this.detail = 4;

            this.topBlock = Blocks.snow;

            AddDecorationBlock(0.0015625F, Blocks.tall_grass_snow);
            AddDecorationBlock(0.0003125F, Blocks.blueberries_bush_snow);
        }

        public override void GenerateScenery(Chunk chunk, int x, int wy, int z)
        { }
    }
}
