using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.client.player;
using ancientlib.game.entity.player;

namespace ancientserver.game.network.packet.handler.player
{
    class PlayerChangeSlotHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerChangeSlot slotPacket = (PacketPlayerChangeSlot)packet;

            EntityPlayerOnline player = (EntityPlayerOnline)netConnection.player;
            netConnection.player.SetHandSlot(slotPacket.GetSlot());
            player.useLeft = UseAction.RELEASE_LEFT;
            player.useRight = UseAction.RELEASE_RIGHT;
        }
    }
}
