using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block;
using ancient.game.world.block.type;

namespace ancientlib.game.item.tool
{
    public class ItemPickaxe : ItemTool
    {
        public ItemPickaxe(string name) : base(name)
        { }

        protected override bool CanDestroyBlock(Block block)
        {
            BlockType type = block.GetBlockType();
            return type == BlockType.stone || type == BlockType.ice || type == BlockType.cloud;
        }
    }
}
