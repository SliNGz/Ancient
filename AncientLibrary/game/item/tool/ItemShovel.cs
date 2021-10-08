using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancient.game.world;
using ancient.game.world.block;
using Microsoft.Xna.Framework;
using ancient.game.world.block.type;

namespace ancientlib.game.item.tool
{
    public class ItemShovel : ItemTool
    {
        public ItemShovel(string name) : base(name)
        {
            this.renderRoll = 15;
            this.baseRenderRoll = -15;
        }

        protected override bool CanDestroyBlock(Block block)
        {
            return block.GetBlockType() == BlockType.dirt || block.GetBlockType() == BlockType.grass || block.GetBlockType() == BlockType.sand || block.GetBlockType() == BlockType.snow;
        }
    }
}
