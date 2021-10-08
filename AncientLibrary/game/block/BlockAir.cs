using ancient.game.world.block.type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using Microsoft.Xna.Framework;

namespace ancient.game.world.block
{
    public class BlockAir : Block
    {
        public BlockAir() : base("Air", BlockType.air)
        {
            this.color = Color.Transparent;
            this.secondaryColor = color;
        }
    }
}