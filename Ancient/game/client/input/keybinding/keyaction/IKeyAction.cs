using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.input.keybinding.keyaction
{
    public interface IKeyAction
    {
        void UpdateHeld(EntityPlayer player);

        void UpdatePressed(EntityPlayer player);

        void UpdateReleased(EntityPlayer player);

        void UpdateUp(EntityPlayer player);
    }
}
