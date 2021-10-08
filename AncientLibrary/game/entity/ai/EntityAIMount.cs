using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.entity;

namespace ancientlib.game.entity.ai
{
    class EntityAIMount : EntityAI
    {
        private EntityMount mount;

        public EntityAIMount(EntityMount mount, int priority) : base(priority)
        {
            this.mount = mount;
        }

        public override void Execute()
        { }

        public override bool ShouldExecute()
        {
            return mount.IsRiddenByOwner();
        }

        public override bool ShouldUpdate()
        {
            return mount.IsRiddenByOwner();
        }

        public override void Stop()
        {
            if (mount.IsRidden())
                mount.GetRidingEntity().Dismount();
        }

        public override void Update(GameTime gameTime)
        {
            Entity ridingEntity = mount.GetRidingEntity();

            if (ridingEntity is EntityLiving)
            {
                EntityLiving living = (EntityLiving)ridingEntity;
                mount.SetYaw(living.GetHeadYaw());
                mount.SetPitch(living.GetPitch());
            }

            ridingEntity.SetPosition(mount.GetPosition() + mount.GetMountOffset());
            ridingEntity.SetVelocity(mount.GetTotalVelocity());
        }
    }
}
