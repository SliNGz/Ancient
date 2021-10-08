using ancient.game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancientlib.game.utils;
using ancientlib.game.item.projectile;
using ancientlib.game.init;
using ancient.game.world.block;
using System.IO;
using ancientlib.game.item;
using ancient.game.entity.player;
using ancientlib.game.entity.model;

namespace ancientlib.game.entity.projectile
{
    public abstract class EntityProjectile : Entity
    {
        protected EntityLiving shooter;

        protected ItemProjectile projectile;
        protected int damage;

        public EntityProjectile(World world) : base(world)
        {
            this.projectile = Items.steelArrow;
            this.damage = Items.steelArrow.GetDamage();
            this.lifeSpan = 2560;
        }

        public EntityProjectile(World world, EntityLiving shooter, ItemProjectile projectile) : this(world)
        {
            this.shooter = shooter;
            this.projectile = projectile;

            this.x = shooter.GetX();
            this.y = shooter.GetEyePosition().Y;
            this.z = shooter.GetZ();

            AddPosition(shooter.GetLookAt() * 0.1F);
            SetVelocity(shooter.GetTotalVelocity());

            if (projectile != null)
                SetProjectile(projectile);

            this.damage = shooter.GetDamage() + (projectile == null ? 0 : projectile.GetDamage());

            this.yaw = shooter.GetHeadYaw();
            this.pitch = shooter.GetHeadPitch();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            SetMovement(Vector3.Forward);
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

        protected override void OnCollisionWithBlock(BoundingBox blockBB, Block block)
        {
            if (!world.IsRemote())
            {
                if (block.IsSolid())
                    world.DespawnEntity(this);
            }
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
            if (entity == shooter || world.IsRemote())
                return;

            if (entity is EntityLiving)
            {
                ((EntityLiving)entity).Damage(new AttackInfo(shooter, damage));
                world.DespawnEntity(this);
            }
        }

        public override float GetPitch()
        {
            Vector3 totalVelocity = GetTotalVelocity();
            return (float)Math.Atan2(totalVelocity.Y, Math.Sqrt(totalVelocity.X * totalVelocity.X + totalVelocity.Z * totalVelocity.Z));
        }

        public EntityLiving GetShooter()
        {
            return this.shooter;
        }

        public void SetShooter(EntityLiving shooter)
        {
            this.shooter = shooter;
        }

        public ItemProjectile GetProjectile()
        {
            return this.projectile;
        }

        public void SetProjectile(ItemProjectile projectile)
        {
            this.projectile = projectile;
            this.gravity = projectile.GetGravity();
        }

        public int GetDamage()
        {
            return this.damage;
        }

        public virtual void SetDamage(int damage)
        {
            this.damage = damage;
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(0.1F, 0.1F, 0.1F);
        }

        public override float GetBaseSpeed()
        {
            return projectile.GetSpeed();
        }

        public override EntityModelCollection GetModelCollection()
        {
            if (projectile != null)
                return projectile.GetModelCollection();

            return null;
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.pitch = reader.ReadSingle();
            SetVelocity(reader.ReadVector3());

            int projectileID = reader.ReadInt32();
            Item item = Items.GetItemFromID(projectileID);

            if (item is ItemProjectile)
            {
                ItemProjectile projectile = ((ItemProjectile)item);
                SetProjectile(projectile);
            }
        }

        public override void Write(BinaryWriter writer)
        {
            base.Write(writer);
            writer.Write(pitch);
            writer.Write(GetVelocity());
            writer.Write(Items.GetIDFromItem(projectile));
        }
    }
}