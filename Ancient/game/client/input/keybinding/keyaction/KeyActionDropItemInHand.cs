using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancient.game.client.network;
using ancientlib.game.network.packet.client.player;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionDropItemInHand : IKeyAction
    {
        public void UpdateHeld(EntityPlayer player)
        { }

        public void UpdatePressed(EntityPlayer player)
        {
            if (!player.GetWorld().IsRemote())
                player.DropItemInHand();
            else
                NetClient.instance.SendPacket(new PacketPlayerDropItem());
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
