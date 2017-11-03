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
    public class BlockSand : Block
    {
        public BlockSand() : base("Sand", BlockType.sand)
        {
            this.color = Color.LightGoldenrodYellow;
            this.secondaryColor = new Color(255, 220, 21);
        }
    }
}
