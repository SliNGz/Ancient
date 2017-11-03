using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block.type;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancient.game.world.chunk;

namespace ancientlib.game.block
{
    public class BlockLantern : BlockLightSource, IBlockModel
    {
        public BlockLantern() : base("Lantern", BlockType.ice)
        {
            this.lightEmission = 15;
            this.dimensions = new Vector3(11 / 17F, 1, 11 / 17F);
        }

        public override Vector3 GetItemModelScale()
        {
            return base.GetItemModelScale() / 8F;
        }

        public override bool IsFullBlock()
        {
            return false;
        }
    }
}
