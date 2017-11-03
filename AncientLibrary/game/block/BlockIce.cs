using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block.type;
using Microsoft.Xna.Framework;

namespace ancientlib.game.block
{
    public class BlockIce : Block
    {
        public BlockIce() : base("Ice", BlockType.ice)
        {
            this.slipperiness = 0.975F;
            this.color = new Color(173, 216, 230, 240);
            this.secondaryColor = color;
        }
    }
}
