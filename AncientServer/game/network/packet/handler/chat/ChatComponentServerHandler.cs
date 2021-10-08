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
using ancientlib.game.network.packet.common.chat;
using ancientlib.game.world.entity;
using ancientlib.game.entity.player;
using ancientlib.game.world;
using ancientlib.game.utils.chat;

namespace ancientserver.game.network.packet.handler.chat
{
    class ChatComponentServerHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketChatComponent chatPacket = (PacketChatComponent)packet;

            ChatComponent chatComponent = chatPacket.GetChatComponent();

            if (chatComponent is ChatComponentText)
            {
                ChatComponentText textComponent = (ChatComponentText)chatComponent;
                string text = textComponent.GetText();
                string chatText = netConnection.player.GetName() + ": " + text;

                if (text.StartsWith("/"))
                    CommandHandler.HandleText(netConnection.player, text);
                else
                {
                    textComponent.SetText(chatText);
                    ((WorldServer)netConnection.player.GetWorld()).BroadcastPacket(chatPacket);
                }

                Console.WriteLine(chatText);
            }
        }
    }
}
