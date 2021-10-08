using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancient.game.entity.player;
using ancientlib.game.world;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.network.packet.common.player;
using ancient.game.entity;

namespace ancientlib.game.entity
{
    public abstract class EntityMount : EntityPet
    {
        protected Entity ridingEntity;

        public EntityMount(World world) : base(world)
        { }

        protected override bool ShouldInterpolate()
        {
            bool shouldInterp = base.ShouldInterpolate() && (ridingEntity == null || ridingEntity != world.GetMyPlayer());

            if(shouldInterp)
            {
                SetVelocity(Vector3.Zero);
                SetMovement(Vector3.Zero);
            }

            return shouldInterp;
        }

        public void SetRidingEntity(Entity ridingEntity)
        {
            this.ridingEntity = ridingEntity;
        }

        public Entity GetRidingEntity()
        {
            return this.ridingEntity;
        }

        public override void OnPlayerInteraction(EntityPlayer player)
        {
            if (player == owner)
            {
                if (player.GetMount() != this)
                    player.Mount(this);
                else
                    player.Dismount();
            }

            base.OnPlayerInteraction(player);
        }

        public abstract Vector3 GetMountOffset();

        public override void OnDeath()
        {
            base.OnDeath();

            if (ridingEntity != null)
                ridingEntity.Dismount();
        }

        public bool IsRiddenByOwner()
        {
            return HasOwner() && ridingEntity == owner;
        }

        public bool IsRidden()
        {
            return this.ridingEntity != null;
        }

        public override bool RenderHealthBar()
        {
            return !IsRiddenByOwner();
        }
    }
}
