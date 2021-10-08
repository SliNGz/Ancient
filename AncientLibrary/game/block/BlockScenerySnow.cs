using ancient.game.world.block.type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.block
{
    public abstract class BlockScenerySnow : BlockScenery
    {
        protected bool snow;

        public BlockScenerySnow(bool snow, string name, BlockType type) : base(name + (snow ? " Snow" : ""), type)
        {
            this.snow = snow;
        }
    }
}
