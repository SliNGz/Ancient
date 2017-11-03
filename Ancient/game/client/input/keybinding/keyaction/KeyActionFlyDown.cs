using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionFlyDown : IKeyAction
    {
        public void UpdateHeld(EntityPlayer player)
        {
            UpdatePressed(player);
        }

        public void UpdatePressed(EntityPlayer player)
        {
            if (player.HasNoClip())
                player.inputVector += Vector3.Down;
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
