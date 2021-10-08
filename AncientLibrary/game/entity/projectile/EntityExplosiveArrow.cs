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
using ancientlib.game.entity.world;
using ancientlib.game.utils;

namespace ancientlib.game.entity.projectile
{
    class EntityExplosiveArrow : EntityArrow
    {
        private int explosionSize;
        private EntityExplosion explosionEntity;

        public EntityExplosiveArrow(World world) : base(world)
        {
            this.explosionSize = 3;
            this.explosionEntity = new EntityExplosion(world);
        }

        public EntityExplosiveArrow(World world, EntityLiving shooter, ItemArrow arrow) : base(world, shooter, arrow)
        {
            this.explosionSize = 3;
            this.explosionEntity = new EntityExplosion(world, new AttackInfo(shooter, shooter.GetDamage() + arrow.GetDamage()), explosionSize * 2, explosionSize * 2, explosionSize * 2);
        }

        public override void SetDamage(int damage)
        {
            base.SetDamage(damage);
            this.explosionEntity.GetAttackInfo().SetDamage((int)(damage / 2F));
        }

        protected override void OnCollisionWithBlock(BoundingBox blockBB, Block block)
        {
            if (!world.IsRemote())
            {
                if (block.IsSolid() || block is BlockWater)
                {
                    if (explosionEntity != null)
                    {
                        world.CreateExplosion((int)x, (int)y, (int)z, explosionSize, explosionSize, explosionSize);
                        explosionEntity.SetPosition(GetPosition() + new Vector3(0, explosionEntity.GetHeight() / 2F, 0));
                        explosionEntity.UpdateBoundingBox();
                        world.SpawnEntity(explosionEntity);
                        world.DespawnEntity(this);
                        explosionEntity = null;
                    }
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
            this.explosionEntity.SetDimensions(Vector3.One * explosionSize * 2);
        }
    }
}
