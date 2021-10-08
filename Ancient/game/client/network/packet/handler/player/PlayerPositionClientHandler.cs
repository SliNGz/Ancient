using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.common.player;
using ancient.game.entity.player;

namespace ancient.game.client.network.packet.handler.player
{
    class PlayerPositionClientHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerPosition playerPos = (PacketPlayerPosition)packet;

            EntityPlayer player = Ancient.ancient.player;

            player.SetPosition(playerPos.GetX(), playerPos.GetY(), playerPos.GetZ());

            if (player.IsRiding())
                player.GetMount().SetPosition(player.GetPosition() - player.GetMount().GetMountOffset());
        }
    }
}
