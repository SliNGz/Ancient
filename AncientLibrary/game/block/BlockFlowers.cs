using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.world.block.type;
using ancient.game.world;
using ancientlib.game.init;

namespace ancientlib.game.block
{
    public class BlockFlowers : BlockScenery, IBlockModel
    {
        public BlockFlowers() : base("Flowers")
        {
            this.dimensions = new Vector3(1, 0.25F, 1);
        }

        public override Vector3 GetItemModelScale()
        {
            return base.GetItemModelScale() / 8F;
        }
    }
}
