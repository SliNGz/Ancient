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
    public class BlockBase : Block
    {
        public BlockBase() : base("Base Block", BlockType.stone)
        {
            this.color = Color.MediumPurple;
            this.secondaryColor = Color.LightCyan;
        }

        public override bool CanBeDestroyed()
        {
            return false;
        }
    }
}
