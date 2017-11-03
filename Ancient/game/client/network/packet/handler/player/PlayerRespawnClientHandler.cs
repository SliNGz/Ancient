using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.common.player;

namespace ancient.game.client.network.packet.handler.player
{
    class PlayerRespawnClientHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerRespawn respawnPacket = (PacketPlayerRespawn)packet;

            Ancient.ancient.player.Respawn();
            Ancient.ancient.guiManager.DisplayGui(Ancient.ancient.guiManager.ingame);
        }
    }
}
