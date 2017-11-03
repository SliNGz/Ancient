using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.block
{
    public class BlockScenerySnow : BlockScenery
    {
        protected bool snow;

        public BlockScenerySnow(bool snow, string name) : base(name + (snow ? " Snow" : ""))
        {
            this.snow = snow;
        }
    }
}
