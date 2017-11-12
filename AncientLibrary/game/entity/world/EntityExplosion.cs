using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancientlib.game.utils;
using ancient.game.utils;

namespace ancientlib.game.entity.world
{
    class EntityExplosion : Entity
    {
        private AttackInfo attackInfo;

        public EntityExplosion(World world) : base(world)
        {
            this.colorMultiply = new Color(0, 0, 0, 0);
            this.lifeSpan = 1;
            this.interactWithEntities = true;
            this.interactWithBlocks = false;
            SetModelState(null);
            SetDimensions(3, 3, 3);
        }

        public EntityExplosion(World world, AttackInfo attackInfo, float width, float height, float length) : this(world)
        {
            this.attackInfo = attackInfo;
            SetDimensions(width, height, length);
        }

        protected override void OnCollideWithEntity(Entity entity)
        {
            base.OnCollideWithEntity(entity);

            if (entity is EntityLiving)
            {
                Vector3 velocity = entity.GetBoundingBox().GetCenter() - boundingBox.GetCenter();
                velocity /= velocity.Length();
                velocity *= world.rand.Next(7, 13);
                entity.SetVelocity(velocity);

                if (attackInfo != null && attackInfo.GetAttacker() != entity)
                    ((EntityLiving)entity).Damage(attackInfo);
            }
        }

        public override Vector3 GetModelScale()
        {
            return Vector3.One;
        }

        public AttackInfo GetAttackInfo()
        {
            return this.attackInfo;
        }

        public void SetAttackInfo(AttackInfo attackInfo)
        {
            this.attackInfo = attackInfo;
        }

        protected override EntityModelState GetDefaultModelState()
        {
            return this.GetModelState();
        }
    }
}
