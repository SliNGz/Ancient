using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.world;
using Microsoft.Xna.Framework;
using ancientlib.game.init;
using ancient.game.world.block.type;

namespace ancientlib.game.block
{
    public class BlockBlueberriesBush : BlockScenerySnow, IBlockModel
    {
        public BlockBlueberriesBush(bool snow) : base(snow, "Blueberries Bush", BlockType.grass)
        {
            this.dimensions = new Vector3(1, (9 + (snow ? 1 : 0)) / 16F, 1);
        }

        public override bool IsSolid()
        {
            return true;
        }

        protected override void DropItems(World world, int x, int y, int z)
        {
            DropItem(world, x, y, z, Items.blueberries, world.rand.Next(1, 3), 95);
        }
    }
}
