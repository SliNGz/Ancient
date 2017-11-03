using ancient.game.entity;
using ancientlib.game.entity;
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

        public NetEntityLiving(Entity entity) : base(entity)
        {
            this.living = (EntityLiving)entity;
            this.lastHealth = living.GetHealth();
        }

        public override void Update()
        {
            base.Update();

            if (ShouldSendDamage())
                UpdateAttackInfo();
            else
                UpdateHealth();
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
    }
}
