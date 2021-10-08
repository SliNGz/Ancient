using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.chunk;
using Microsoft.Xna.Framework;
using ancientlib.game.init;

namespace ancientlib.game.world.biome
{
    public class BiomeSkyland : Biome
    {
        public BiomeSkyland() : base("skyland")
        {
            this.maxHeight = 80;
            this.minHeight = 60;

            this.elevation = 1.75F;
            this.roughness = 4;
            this.detail = 8;

            this.topBlock = Blocks.grass;
            this.baseBlock = Blocks.dirt;

            this.skyColor = Color.CornflowerBlue;

            AddDecorationBlock(0.00625F, Blocks.flowers);
        }

        public override void GenerateScenery(Chunk chunk, int x, int wy, int z)
        { }
    }
}
