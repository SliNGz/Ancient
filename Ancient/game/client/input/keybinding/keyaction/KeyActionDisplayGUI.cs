using ancient.game.client.gui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionDisplayGui : IKeyAction
    {
        private Gui gui;

        public KeyActionDisplayGui(Gui gui)
        {
            this.gui = gui;
        }

        public void UpdateHeld(EntityPlayer player)
        { }

        public void UpdatePressed(EntityPlayer player)
        {
            if (Ancient.ancient.guiManager.GetCurrentGui() != gui)
                Ancient.ancient.guiManager.DisplayGui(gui);
            else
                Ancient.ancient.guiManager.DisplayGui(gui.GetLastGui());
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
