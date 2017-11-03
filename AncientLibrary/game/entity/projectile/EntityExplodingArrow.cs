using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancient.game.world.block;
using Microsoft.Xna.Framework;
using ancientlib.game.item.projectile;
using ancient.game.entity;

namespace ancientlib.game.entity.projectile
{
    class EntityExplodingArrow : EntityArrow
    {
        private int explosionSize;

        public EntityExplodingArrow(World world) : base(world)
        {
            this.explosionSize = 5;
        }

        public EntityExplodingArrow(World world, EntityLiving shooter, ItemProjectile projectile) : base(world, shooter, projectile)
        {
            this.explosionSize = 5;
        }

        protected override void OnCollisionWithBlock(BoundingBox blockBB, Block block)
        {
            base.OnCollisionWithBlock(blockBB, block);

            if (!world.IsRemote())
            {
                if (block.IsSolid() || block is BlockWater)
                {
                    world.CreateExplosion((int)x, (int)y, (int)z, explosionSize, explosionSize, explosionSize);
                    world.DespawnEntity(this);
                }
            }
        }

        public int GetExplosionSize()
        {
            return this.explosionSize;
        }

        public void SetExplosionSize(int explosionSize)
        {
            this.explosionSize = explosionSize;
        }
    }
}
