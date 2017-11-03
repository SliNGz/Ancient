using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionMove : IKeyAction
    {
        private Vector3 movement;

        public KeyActionMove(Vector3 movement)
        {
            this.movement = movement;
        }

        public void UpdateHeld(EntityPlayer player)
        {
            UpdatePressed(player);
        }

        public void UpdatePressed(EntityPlayer player)
        {
            player.inputVector += movement;
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
