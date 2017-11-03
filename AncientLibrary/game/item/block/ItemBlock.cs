using ancient.game.entity.player;
using ancient.game.world;
using ancient.game.world.block;
using ancientlib.game.entity;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item
{
    public class ItemBlock : Item
    {
        private Block block;

        public ItemBlock(string name, Block block) : base(name)
        {
            this.block = block;
            this.modelScale = block.GetItemModelScale();
            this.dropScale = modelScale * 2.5F;
        }

        public override void Use(EntityPlayer player)
        {
            base.Use(player);
            player.DestroyTargetedBlock();
        }

        public override void UseRightClick(EntityPlayer player)
        {
            base.UseRightClick(player);
            player.usingItemInHand = true;
            int? face = player.GetTargetedBlockFace();

            if (face.HasValue)
            {
                Vector3? position = player.GetTargetedBlockPosition();

                if (position.HasValue)
                {
                    int x = (int)position.Value.X;
                    int y = (int)position.Value.Y;
                    int z = (int)position.Value.Z;

                    World world = player.GetWorld();

                    switch (face)
                    {
                        case 0:
                            y -= 1;
                            break;
                        case 1:
                            y += 1;
                            break;
                        case 2:
                            z -= 1;
                            break;
                        case 3:
                            z += 1;
                            break;
                        case 4:
                            x -= 1;
                            break;
                        case 5:
                            x += 1;
                            break;
                    }

                    Block posBlock = world.GetBlock(x, y, z);

                    if (posBlock != null && !posBlock.IsSolid() && block.CanBePlaced(world, x, y, z))
                    {
                        world.SetBlock(block, x, y, z);

                        if (!player.HasNoClip())
                            player.RemoveItem(this, 1);
                    }
                }
            }
        }

        public void SetBlock(Block block)
        {
            this.block = block;
        }

        public Block GetBlock()
        {
            return this.block;
        }

        public override string GetModelName()
        {
            return block.GetModelName();
        }
    }
}
