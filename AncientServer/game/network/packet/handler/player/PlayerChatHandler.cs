using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.command;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerChatHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerChat chatPacket = (PacketPlayerChat)packet;

            string text = chatPacket.GetText();

            Console.WriteLine(netConnection.player.GetName() + ": " + text);

            if (text.StartsWith("/"))
                CommandHandler.HandleText(netConnection.player, text);
        }
    }
}
