using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionJumpFly : IKeyAction
    {
        public void UpdateHeld(EntityPlayer player)
        {
            player.SetJumping(false);

            if (player.HasNoClip())
                player.inputVector += Vector3.Up;
            else
            {
                if (player.InWater())
                {
                    player.Jump();
                    player.SetJumping(true);
                }
            }
        }

        public void UpdatePressed(EntityPlayer player)
        {
            player.SetJumping(true);

            if (player.HasNoClip())
                player.inputVector += Vector3.Up;
            else
                player.Jump();
        }

        public void UpdateReleased(EntityPlayer player)
        {
            player.SetJumping(false);
        }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
