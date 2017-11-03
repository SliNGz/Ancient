using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.common.status;
using ancientlib.game.network.packet.handler;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientserver.game.network.packet.handler.status
{
    class DisconnectServerHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketDisconnect dcPacket = (PacketDisconnect)packet;

            if (netConnection.stage == ConnectionStage.INGAME)
            {
                ConsoleExtensions.WriteLine(ConsoleColor.Yellow, netConnection.player.GetName() + " disconnected.");
                AncientServer.ancientServer.world.DespawnEntity(netConnection.player);
            }
            else
                ConsoleExtensions.WriteLine(ConsoleColor.Yellow, netConnection.GetIPAddress() + " disconnected in " + netConnection.stage + " stage.");

            netConnection.Close();
        }
    }
}
