using ancientlib.game.item.tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block;
using ancientlib.game.init;

namespace ancientlib.game.item
{
    public class ItemHand : ItemTool
    {
        public ItemHand() : base("Hand")
        { }

        protected override bool CanDestroyBlock(Block block)
        {
            return block == Blocks.branch;
        }
    }
}
