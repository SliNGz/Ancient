using ancient.game.world.block.type;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.block
{
    public class BlockDirt : Block
    {
        public BlockDirt() : base("Dirt", BlockType.dirt)
        {
            this.color = new Color(81, 44, 0);
            this.secondaryColor = new Color(77, 65, 50);
        }
    }
}