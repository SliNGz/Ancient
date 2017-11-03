using ancientlib.game.network.packet.handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancientlib.game.network;
using ancientlib.game.network.packet;
using ancientlib.game.network.packet.server.player;
using ancientlib.game.item;

namespace ancient.game.client.network.packet.handler.player
{
    class PlayerItemActionHandler : IPacketHandler
    {
        public void HandlePacket(Packet packet, NetConnection netConnection)
        {
            PacketPlayerItemAction itemPacket = (PacketPlayerItemAction)packet;

            ItemStack itemStack = itemPacket.GetItemStack();
            ItemAction itemAction = itemPacket.GetItemAction();

            if (itemAction == ItemAction.ADD_ITEM)
                Ancient.ancient.player.GetInventory().AddItem(itemPacket.GetItemStack());
            else if (itemAction == ItemAction.REMOVE_ITEM)
                Ancient.ancient.player.GetInventory().RemoveItem(itemPacket.GetItemStack());
        }
    }
}
