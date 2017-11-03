using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.handshake;
using ancientlib.game.network.packet.handler;
using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.network
{
    class NetValidate
    {
        public static void ValidateClient(NetConnection netConnection)
        {
            Packet packet = netConnection.ReadPacket();

            if (packet is PacketHandshake)
                PacketHandlers.GetPacketHandlerFromPacket(packet).HandlePacket(packet, netConnection);
            else
                BlockUnauthorizedClient(netConnection);
        }

        private static void BlockUnauthorizedClient(NetConnection netConnection)
        {
            ConsoleExtensions.WriteLine(ConsoleColor.Red, "An unauthorized client tried to connect: " + netConnection);
            netConnection.Close();
        }
    }
}
