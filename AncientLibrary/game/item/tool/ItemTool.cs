using ancient.game.entity.player;
using ancient.game.world;
using ancient.game.world.block;
using ancientlib.game.item.statbased;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item.tool
{
    public abstract class ItemTool : ItemStatDependent
    {
        public ItemTool(string name) : base(name)
        {
            this.maxItemStack = 1;
        }

        public override void Use(EntityPlayer player, ItemStack itemStack)
        {
            base.Use(player, itemStack);

            World world = player.GetWorld();
            Vector3? blockPosition = player.GetTargetedBlockPosition();

            if (blockPosition.HasValue)
            {
                if (player.targetBlockPos != blockPosition.Value)
                    itemStack.ticksUsed = 0;
                else
                {
                    int x = (int)player.targetBlockPos.X;
                    int y = (int)player.targetBlockPos.Y;
                    int z = (int)player.targetBlockPos.Z;

                    Block block = world.GetBlock(x, y, z);

                    if (CanDestroyBlock(block))
                    {
                        player.destroyAnimation = itemStack.ticksUsed / 128F;

                        if (itemStack.ticksUsed >= 128)
                        {
                            itemStack.ticksUsed = 0;
                            world.DestroyBlock(x, y, z);
                        }
                    }
                }
            }

            if (itemStack.ticksUsed == 0)
            {
                player.destroyAnimation = 0;

                if (blockPosition.HasValue)
                    player.targetBlockPos = blockPosition.Value;
            }
        }

        public override void OnUseFinish(EntityPlayer player, ItemStack itemStack)
        {
            base.OnUseFinish(player, itemStack);
            player.destroyAnimation = 0;
            itemStack.ticksUsed = 0;
        }

        protected abstract bool CanDestroyBlock(Block block);

        public override bool CanBeSpammed()
        {
            return true;
        }
    }
}
