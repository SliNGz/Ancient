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
    public class BlockSnow : Block
    {
        public BlockSnow() : base("Snow", BlockType.snow)
        {
            this.color = Color.FloralWhite;
            this.secondaryColor = Color.LightBlue;
        }
    }
}
