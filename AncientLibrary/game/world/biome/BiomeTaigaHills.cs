using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.biome
{
    class BiomeTaigaHills : BiomeTaiga
    {
        public BiomeTaigaHills()
        {
            this.maxHeight = 128;
            this.minHeight = 56;

            this.elevation = 2.125F;
            this.roughness = 4;
            this.detail = 12;

            this.treeSpawnChance = 0.65F;
        }
    }
}
