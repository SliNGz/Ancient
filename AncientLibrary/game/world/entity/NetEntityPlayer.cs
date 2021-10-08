using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancientlib.game.entity.player;
using ancientlib.game.entity;
using ancientlib.game.network.packet.server.player;
using Microsoft.Xna.Framework;
using ancientlib.game.network.packet.server.entity;
using ancientlib.game.classes;

namespace ancientlib.game.world.entity
{
    class NetEntityPlayer : NetEntityLiving
    {
        private EntityPlayerOnline player;

        private EntityMount lastMount;

        private int lastExp;
        private int lastLevel;
        private int lastMaxHealth;

        private Class lastClass;

        public NetEntityPlayer(Entity entity) : base(entity)
        {
            this.player = (EntityPlayerOnline)entity;
            this.lastExp = player.GetExp();
            this.lastLevel = player.GetLevel();
            this.lastMaxHealth = player.GetMaxHealth();
            this.lastClass = player.GetClass();
        }

        public override void Update()
        {
            base.Update();
            UpdateMount();
            UpdateLevel();
            UpdateExp();
            UpdateMaxHealth();
            UpdateClass();
        }

        private void UpdateMount()
        {
            if (this.lastMount != player.GetMount())
            {
                MountAction mountAction = MountAction.MOUNT;

                if (!player.IsRiding())
                    mountAction = MountAction.DISMOUNT;

                ((WorldServer)player.GetWorld()).BroadcastPacket(new PacketPlayerMountPet(player, player.GetMount(), mountAction));

                this.lastMount = player.GetMount();
            }
        }

        private void UpdateLevel()
        {
            if (player.GetLevel() != lastLevel)
            {
                player.netConnection.SendPacket(new PacketPlayerChangeLevel(player.GetLevel()));
                lastLevel = player.GetLevel();
            }
        }

        private void UpdateExp()
        {
            if (player.GetExp() != lastExp)
            {
                player.netConnection.SendPacket(new PacketPlayerChangeExp(player.GetExp()));
                lastExp = player.GetExp();
            }
        }

        private void UpdateMaxHealth()
        {
            if (player.GetMaxHealth() != lastMaxHealth)
            {
                player.netConnection.SendPacket(new PacketPlayerMaxHealth(player.GetMaxHealth()));
                lastMaxHealth = player.GetMaxHealth();
            }
        }

        private void UpdateClass()
        {
            if (player.GetClass() != lastClass)
            {
                player.netConnection.SendPacket(new PacketPlayerChangeClass(player.GetClass()));
                lastClass = player.GetClass();
            }
        }
    }
}
