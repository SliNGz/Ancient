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
    public class BlockCloud : Block
    {
        public BlockCloud() : base("Cloud", BlockType.cloud)
        {
            this.color = new Color(255, 255, 255);
            this.secondaryColor = new Color(220, 220, 255);
        }
    }
}
