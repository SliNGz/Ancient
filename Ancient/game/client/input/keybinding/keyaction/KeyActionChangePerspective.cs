using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity.player;
using ancient.game.renderers.world;
using ancient.game.client.camera;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActionChangePerspective : IKeyAction
    {
        public void UpdateHeld(EntityPlayer player)
        { }

        public void UpdatePressed(EntityPlayer player)
        {
            CameraFOV camera = WorldRenderer.camera;

            if (camera.GetDistance() == 0)
                camera.SetDistance(5F);
            else if (camera.GetDistance() > 0)
                camera.SetDistance(-5F);
            else
                camera.SetDistance(0);
        }

        public void UpdateReleased(EntityPlayer player)
        { }

        public void UpdateUp(EntityPlayer player)
        { }
    }
}
