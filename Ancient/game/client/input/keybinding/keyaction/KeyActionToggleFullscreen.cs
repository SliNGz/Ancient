using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionToggleFullscreen : IKeyAction
    {
        public void UpdateHeld(EntityPlayer player)
        { }

        public void UpdatePressed(EntityPlayer player)
        {
            Ancient.ancient.ToggleFullscreen();
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
