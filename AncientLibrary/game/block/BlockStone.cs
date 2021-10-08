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
    public class BlockStone : Block
    {
        public BlockStone() : base("Stone", BlockType.stone)
        {
            this.color = new Color(146, 146, 146);
            this.secondaryColor = new Color(72, 72, 72);
        }
    }
}
