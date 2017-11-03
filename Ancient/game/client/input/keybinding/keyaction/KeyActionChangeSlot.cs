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
    class KeyActionChangeSlot : IKeyAction
    {
        private int slot;

        public KeyActionChangeSlot(int slot)
        {
            this.slot = slot;
        }

        public void UpdateHeld(EntityPlayer player)
        { }

        public void UpdatePressed(EntityPlayer player)
        {
            int newSlot = 9;

            if (slot != 0)
                newSlot = slot - 1;

            player.SetHandSlot(newSlot);

            if (Ancient.ancient.world.IsRemote())
                NetClient.instance.SendPacket(new PacketPlayerChangeSlot((byte)newSlot));
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
