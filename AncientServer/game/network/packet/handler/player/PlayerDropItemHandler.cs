using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.player;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerDropItemHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerDropItem dropItemPacket = (PacketPlayerDropItem)packet;

            netConnection.player.DropItemInHand();
        }
    }
}
