using ancient.game.world.block.type;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.block
{
    public class BlockGrass : Block
    {
        public BlockGrass() : base("Grass", BlockType.grass)
        {
            // this.color = new Color(0, 159, 95);
            //  this.secondaryColor = new Color(0, 109, 38);

            this.color = new Color(41, 102, 25);
            this.secondaryColor = new Color(95, 162, 77);
        }
    }
}