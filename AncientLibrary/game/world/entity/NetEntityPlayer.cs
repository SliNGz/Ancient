using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancientlib.game.entity.player;
using ancientlib.game.entity;
using ancientlib.game.network.packet.server.player;

namespace ancientlib.game.world.entity
{
    class NetEntityPlayer : NetEntity
    {
        private EntityPlayerOnline player;

        private EntityMount lastMount;

        public NetEntityPlayer(Entity entity) : base(entity)
        {
            this.player = (EntityPlayerOnline)entity;
        }

        public override void Update()
        {
            base.Update();
            UpdateMount();
        }

        private void UpdateMount()
        {
            if (this.lastMount != player.GetMount())
            {
                MountAction mountAction = MountAction.MOUNT;

                if (!player.IsRiding())
                    mountAction = MountAction.DISMOUNT;

                player.netConnection.SendPacket(new PacketPlayerMountPet(player.GetMount(), mountAction));

                this.lastMount = player.GetMount();
            }
        }
    }
}
