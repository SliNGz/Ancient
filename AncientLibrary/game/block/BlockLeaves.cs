using ancient.game.world.block.type;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.block
{
    public class BlockLeaves : Block
    {
        public BlockLeaves() : base("Leaves", BlockType.leaves)
        {
            this.color = new Color(0, 38, 2);
            this.secondaryColor = new Color(0, 19, 1);
        }
    }
}
