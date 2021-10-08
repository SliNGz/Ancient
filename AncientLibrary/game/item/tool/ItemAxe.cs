using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block;
using ancient.game.world.block.type;

namespace ancientlib.game.item.tool
{
    public class ItemAxe : ItemTool
    {
        public ItemAxe(string name) : base(name)
        {
            this.renderRoll = 70;
            this.baseRenderRoll = 25;
        }

        protected override bool CanDestroyBlock(Block block)
        {
            return block.GetBlockType() == BlockType.wood || block.GetBlockType() == BlockType.leaves;
        }
    }
}
