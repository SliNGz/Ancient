using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.chunk;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using ancient.game.world.block;
using ancientlib.game.world.structure;

namespace ancientlib.game.world.biome
{
    class BiomeJungle : Biome
    {
        public BiomeJungle() : base("jungle")
        {
            this.maxHeight = 96;
            this.minHeight = 54;

            this.elevation = 4;
            this.roughness = 0;
            this.detail = 24;

            this.topBlock = Blocks.grass;
            this.baseBlock = Blocks.dirt;

            this.grassColor1 = new Color(105, 193, 24);
            this.grassColor2 = new Color(177, 238, 4);

            this.skyColor = Color.CornflowerBlue;
            this.waterColor = Blocks.water.GetColor();

            AddDecorationBlock(0.0625F, Blocks.tall_grass);
        }

        public override void GenerateScenery(Chunk chunk, int x, int wy, int z)
        {
            GenerateTrees(chunk, x, wy, z);
        }

        protected virtual void GenerateTrees(Chunk chunk, int x, int wy, int z)
        {
            double modifier = 1;

            if (chunk.rand.NextDouble() * 100 >= 0.3F * modifier)
                return;

            Structures.GetStructureFromName("tree_jungle" + "_" + chunk.rand.Next(1)).Generate(chunk.GetWorld(), chunk.GetX() + x, wy, chunk.GetZ() + z);
        }
    }
}
