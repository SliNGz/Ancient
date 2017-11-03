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
    class PlayerUseItemHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerUseItem itemPacket = (PacketPlayerUseItem)packet;

            EntityPlayerOnline player = (EntityPlayerOnline)netConnection.player;

            UseAction useAction = itemPacket.GetUseAction();

            if (useAction >= UseAction.PRESS_LEFT && useAction <= UseAction.HOLD_LEFT)
                player.useLeft = useAction;
            else
                player.useRight = useAction;
        }
    }
}
