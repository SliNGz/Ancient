using ancient.game.world.block.type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.block
{
    public class BlockBranch : BlockScenery, IBlockModel
    {
        public BlockBranch() : base("Branch", BlockType.wood)
        {
            this.dimensions.Y = 1 / 15F;
        }
    }
}
