using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionToggleNoClip : IKeyAction
    {
        public void UpdateHeld(EntityPlayer player)
        { }

        public void UpdatePressed(EntityPlayer player)
        {
            player.SetNoClip(!player.HasNoClip());
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
