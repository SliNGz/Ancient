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
using ancientlib.game.entity.model;
using ancient.game.world.block;
using System.IO;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.entity;

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
            SetDimensions(3, 3, 3);
        }

        public EntityExplosion(World world, AttackInfo attackInfo, float width, float height, float length) : this(world)
        {
            this.attackInfo = attackInfo;
            SetDimensions(width, height, length);
        }

        public override void Update(GameTime gameTime)
        {
            UpdateBoundingBox();
            base.Update(gameTime);
        }

        protected override bool CanCollideWithBlockBoundingBox(Block block)
        {
            return false;
        }

        protected override bool CanCollideWithEntityBoundingBox(Entity entity)
        {
            return false;
        }

        protected override void OnCollideWithEntity(Entity entity)
        {
            base.OnCollideWithEntity(entity);
            Vector3 velocity = entity.GetBoundingBox().GetCenter() - boundingBox.GetCenter();
            velocity /= velocity.Length();
            velocity *= world.rand.Next(7, 13);
            entity.SetVelocity(velocity);

            if (!world.IsRemote())
            {
                if (entity is EntityLiving)
                {
                    if (attackInfo != null && attackInfo.GetAttacker() != entity)
                        ((EntityLiving)entity).Damage(attackInfo);
                }
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

        public override EntityModelCollection GetModelCollection()
        {
            return null;
        }

        protected override bool ShouldInterpolate()
        {
            return false;
        }

        public override bool ShouldSendPosition()
        {
            return false;
        }

        public override bool ShouldSendRotation()
        {
            return false;
        }

        public override string GetEntityName()
        {
            return "explosion";
        }

        public override bool IsMultiplayerSupported()
        {
            return true;
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write((int)width);
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.width = reader.ReadInt32();
            this.height = width;
            this.length = width;
        }
    }
}
