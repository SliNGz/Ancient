using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancient.game.client.network;
using ancientlib.game.network.packet.common.player;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionUseSpecial : IKeyAction
    {
        public void UpdateHeld(EntityPlayer player)
        {
            player.GetInventory().UseSpecial();
        }

        public void UpdatePressed(EntityPlayer player)
        {
            player.GetInventory().UseSpecial();

            if (!player.usingSpecial)
            {
                if (player.GetWorld().IsRemote())
                    NetClient.instance.SendPacket(new PacketPlayerUseSpecial());
            }

            player.usingSpecial = true;
        }

        public void UpdateReleased(EntityPlayer player)
        {
            if (player.usingSpecial)
            {
                if (player.GetWorld().IsRemote())
                    NetClient.instance.SendPacket(new PacketPlayerUseSpecial());
                }

            player.usingSpecial = false;
        }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
