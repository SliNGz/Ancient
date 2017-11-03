using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.common.player;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerRespawnServerHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerRespawn respawnPacket = (PacketPlayerRespawn)packet;

            netConnection.player.Respawn();
        }
    }
}
