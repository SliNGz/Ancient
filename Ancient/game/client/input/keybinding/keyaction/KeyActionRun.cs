using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancient.game.renderers.world;
using Microsoft.Xna.Framework;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionRun : IKeyAction
    {
        public static int FOV_ADD = 15;

        private int add;
        private bool shouldAdd;

        private int upTicks;

        public KeyActionRun()
        {
            this.add = 0;
            this.shouldAdd = true;
        }

        public void UpdateHeld(EntityPlayer player)
        {
            UpdatePressed(player);
        }

        public void UpdatePressed(EntityPlayer player)
        {
            if (player.inputVector.Z >= 0)
                UpdateUp(player);
            else
            {
                player.SetRunning(true);

                if (shouldAdd)
                {
                    add++;
                    WorldRenderer.camera.AddFov(1);

                    if (add == FOV_ADD)
                        shouldAdd = false;
                }
            }
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        {
            player.SetRunning(false);

            upTicks++;

            if (upTicks % 3 != 0)
                return;

            upTicks = 0;

            if (add > 0)
            {
                add--;
                WorldRenderer.camera.AddFov(-1);
            }
            else
                shouldAdd = true;
        }
    }
}
