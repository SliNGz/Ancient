using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.world.block;
using ancient.game.world.block.type;
using ancient.game.world;
using ancientlib.game.init;

namespace ancientlib.game.block
{
    public class BlockTallGrass : BlockScenerySnow, IBlockModel
    {
        public BlockTallGrass(bool snow) : base(snow, "Tall Grass")
        { }

        public override Vector3 GetItemModelScale()
        {
            return base.GetItemModelScale() / 8F;
        }

        protected override void DropItems(World world, int x, int y, int z)
        { }

        public override string GetModelName()
        {
            return "tall_grass_taiga";
        }
    }
}
