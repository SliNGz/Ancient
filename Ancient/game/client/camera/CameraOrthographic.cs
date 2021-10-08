using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.camera
{
    public class CameraOrthographic : Camera
    {
        public CameraOrthographic(float width, float height, float nearPlane, float farPlane) : base(nearPlane, farPlane)
        {
            this.projection = Matrix.CreateOrthographic(width, height, nearPlane, farPlane);
            this.view = Matrix.CreateLookAt(position, target, Vector3.Forward);
        }

        public override void Update()
        {
            UpdateViewMatrix();
        }

        protected override void UpdateViewMatrix()
        {
            this.view = Matrix.CreateLookAt(position, target, Vector3.Forward);
        }
    }
}
