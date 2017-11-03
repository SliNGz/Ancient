using ancient.game.world.block.type;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.block
{
    public class BlockLog : Block
    {
        public BlockLog() : base("Log", BlockType.wood)
        {
            this.color = new Color(36, 17, 0);
            this.secondaryColor = color;
        }
    }
}
