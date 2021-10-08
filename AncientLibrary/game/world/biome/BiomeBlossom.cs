using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.chunk;
using Microsoft.Xna.Framework;
using ancientlib.game.init;
using ancientlib.game.world.structure;

namespace ancientlib.game.world.biome
{
    class BiomeBlossom : Biome
    {
        private string treeStructureName = "tree_blossom";
        private int treeStructureNum = Structures.TREE_BLOSSOM_NUM;
        protected float treeSpawnChance = 0.07F;

        public BiomeBlossom(string name) : base(name)
        {
            this.grassColor1 = new Color(104, 171, 23);
            this.grassColor2 = new Color(104, 71, 50);

            this.elevation = 2.11F;
            this.roughness = 5.15F;
            this.detail = 8;

            this.maxHeight = 144;
            this.minHeight = 80;
        }

        public override void GenerateScenery(Chunk chunk, int x, int wy, int z)
        {
            GenerateTrees(chunk, x, wy, z);
        }

        public override void GenerateDecorationBlocks(Chunk chunk, int wy, int height, int index, int x, int z)
        {
            base.GenerateDecorationBlocks(chunk, wy, height, index, x, z);

            if (wy == height + 1)
            {
                double nx = (chunk.GetX() + x) / 2.0;
                double nz = (chunk.GetZ() + z) / 2.0;

                double noise = chunk.GetWorld().GetSimplexNoise().Evaluate(nx, nz);

                if(noise > 0.07)
                    chunk.GetBlockArray().SetBlock(Blocks.snow_layer, index);
            }
        }

        protected virtual void GenerateTrees(Chunk chunk, int x, int wy, int z)
        {
            double modifier = 1;

            if (chunk.rand.NextDouble() * 100 >= treeSpawnChance * modifier)
                return;

            Structures.GetStructureFromName(treeStructureName + "_" + chunk.rand.Next(treeStructureNum)).Generate(chunk.GetWorld(), chunk.GetX() + x, wy, chunk.GetZ() + z);
        }

        public override float GetFogStart()
        {
            return 0.25F;
        }
    }
}
