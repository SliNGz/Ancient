using ancient.game.entity.player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.camera
{
    public class CameraPlayerFOV : CameraFOV
    {
        private EntityPlayer player;

        public CameraPlayerFOV(EntityPlayer player, int fov, float nearPlane, float farPlane) : base(fov, nearPlane, farPlane)
        {
            this.player = player;
        }

        public override void Update()
        {
            UpdateRotation();
            base.Update();
        }

        private void UpdateRotation()
        {
            if (player == null)
                return;

            this.yaw = player.GetHeadYaw();
            this.pitch = player.GetHeadPitch();
        }
    }
}
