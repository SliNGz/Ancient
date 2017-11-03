using ancient.game.world.chunk;
using ancient.game.world.generator;
using ancientlib.game.init;
using ancientlib.game.world.structure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.biome
{
    public class BiomeTaiga : Biome
    {
        private string treeStructureName = "tree";
        private int treeStructureNum = Structures.TREE_NUM;
        protected float treeSpawnChance;

        public BiomeTaiga()
        {
            this.maxHeight = 112;
            this.minHeight = 56;

            this.elevation = 1.25F;
            this.roughness = 2;
            this.detail = 12;

            this.skyColor = new Color(185, 216, 218);

            this.treeSpawnChance = 0.75F;

            AddDecorationBlock(0.03125F, Blocks.tall_grass);
            AddDecorationBlock(1 / 47300F, Blocks.campfire);
            AddDecorationBlock(0.0003125F, Blocks.blueberries_bush);
        }

        public Biome SetTreeStructure(string treeStructureName, int treeStructureNum)
        {
            this.treeStructureName = treeStructureName;
            this.treeStructureNum = treeStructureNum;
            return this;
        }

        public override void GenerateScenery(Chunk chunk, int x, int wy, int z)
        {
            GenerateTrees(chunk, x, wy, z);
        }

        protected virtual void GenerateTrees(Chunk chunk, int x, int wy, int z)
        {
            double modifier = 1;

            if (chunk.rand.NextDouble() * 100 >= treeSpawnChance * modifier)
                return;

            Structures.GetStructureFromName(treeStructureName + "_" + chunk.rand.Next(treeStructureNum)).Generate(chunk.GetWorld(), chunk.GetX() + x, wy, chunk.GetZ() + z);
        }
    }
}
