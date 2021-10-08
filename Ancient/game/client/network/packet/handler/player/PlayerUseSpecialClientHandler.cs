using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.common.player;
using ancient.game.client.network.packet.handler.entity;
using ancient.game.entity;
using ancientlib.game.network.packet.server.entity;
using ancient.game.entity.player;

namespace ancient.game.client.network.packet.handler.player
{
    class PlayerUseSpecialClientHandler : AbstractEntityHandler
    {
        public override void HandlePacket(Entity entity, PacketEntity packet, NetConnection netConnection)
        {
            PacketPlayerUseSpecial specialPacket = (PacketPlayerUseSpecial)packet;

            EntityPlayer player = (EntityPlayer)entity;
            player.usingSpecial = !player.usingSpecial;
        }
    }
}
