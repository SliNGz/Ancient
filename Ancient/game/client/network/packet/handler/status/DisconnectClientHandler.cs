using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancient.game.client.gui;
using ancientlib.game.utils;
using ancientlib.game.network.packet.common.status;

namespace ancient.game.client.network.packet.handler.status
{
    class DisconnectClientHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketDisconnect dcPacket = (PacketDisconnect)packet;

            Ancient.ancient.netClient.Close();
            Ancient.ancient.guiManager.DisplayGui(new GuiDisconnect(Ancient.ancient.guiManager, dcPacket.GetMessage()));
        }
    }
}
