using ancient.game.entity;
using ancient.game.entity.player;
using ancientlib.game.entity;
using ancientlib.game.entity.player;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.entity
{
    public class NetEntityLiving : NetEntity
    {
        protected EntityLiving living;

        protected int lastHealth;

        protected AttackInfo attackInfo;

        private float lastHeadYaw;
        private float lastHeadPitch;

        public NetEntityLiving(Entity entity) : base(entity)
        {
            this.living = (EntityLiving)entity;
            this.lastHealth = living.GetHealth();
            this.lastHeadYaw = living.GetHeadYaw();
            this.lastHeadPitch = living.GetHeadPitch();
        }

        public override void Update()
        {
            base.Update();

            if (ShouldSendDamage())
                UpdateAttackInfo();
            else
                UpdateHealth();

            UpdateHeadRotation();
        }

        private void UpdateHealth()
        {
            if (living.GetHealth() != lastHealth)
            {
                world.BroadcastPacket(new PacketEntityHealth(living));
                this.lastHealth = living.GetHealth();
            }
        }

        private void UpdateAttackInfo()
        {
            world.BroadcastPacket(new PacketDamageEntity(living, attackInfo));
            this.attackInfo = null;
            this.lastHealth = living.GetHealth();
        }

        public void SetAttackInfo(AttackInfo attackInfo)
        {
            this.attackInfo = attackInfo;
        }

        private bool ShouldSendDamage()
        {
            return this.attackInfo != null;
        }

        private void UpdateHeadRotation()
        {
            if (ShouldSendHeadRotation())
            {
                world.BroadcastPacket(new PacketEntityHeadRotation(living), living is EntityPlayer ? (EntityPlayer)living : null);

                this.lastHeadYaw = living.GetHeadYaw();
                this.lastHeadPitch = living.GetHeadPitch();
            }
        }

        private bool ShouldSendHeadRotation()
        {
            return this.lastHeadYaw != living.GetHeadYaw() || this.lastHeadPitch != living.GetHeadPitch();
        }
    }
}
