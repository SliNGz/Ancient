using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.common.player;
using ancientlib.game.world.entity;
using ancientlib.game.entity.player;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerUseSpecialServerHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerUseSpecial specialPacket = (PacketPlayerUseSpecial)packet;

            netConnection.player.usingSpecial = !netConnection.player.usingSpecial;

            specialPacket.SetID(netConnection.player.GetID());

            PlayerList players = netConnection.player.GetWorld().players;
            for (int i = 0; i < players.Count; i++)
            {
                EntityPlayerOnline player = (EntityPlayerOnline)players[i];

                if (player != netConnection.player)
                    player.netConnection.SendPacket(specialPacket);
            }
        }
    }
}
