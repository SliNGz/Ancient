using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using static ancientlib.game.utils.DirectionHelper;
using ancientlib.game.utils;

namespace ancient.game.client.camera
{
    public class CameraFOV : Camera
    {
        protected int fov;

        protected float distance;

        protected float yaw;
        protected float pitch;

        protected Direction direction;

        public CameraFOV(int fov, float nearPlane, float farPlane, float yaw, float pitch, float distance) : base(nearPlane, farPlane)
        {
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), Ancient.ancient.device.DisplayMode.AspectRatio, nearPlane, farPlane);

            this.fov = fov;

            this.distance = distance;

            this.yaw = yaw;
            this.pitch = pitch;

            Update();
        }

        public CameraFOV(int fov, float nearPlane, float farPlane) : this(fov, nearPlane, farPlane, 0, 0, 0)
        { }

        public override void Update()
        {
            UpdateTargetVector();
            UpdateViewMatrix();
            UpdateViewFrustum();
            UpdateDirection();
        }

        private void UpdateTargetVector()
        {
            this.target = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(this.yaw, this.pitch, 0));
        }

        protected override void UpdateViewMatrix()
        {
            if (distance == 0)
                this.view = Matrix.CreateLookAt(position, target, Vector3.Up);
            else
                this.view = Matrix.CreateLookAt(target * -distance, position, Vector3.Up);
        }

        private void UpdateProjection()
        {
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), Ancient.ancient.device.DisplayMode.AspectRatio, nearPlane, farPlane);
        }

        private void UpdateDirection()
        {
            this.direction = DirectionHelper.GetDirection(yaw, pitch);
        }

        public void SetFov(int fov)
        {
            this.fov = fov;
            UpdateProjection();
        }

        public void AddFov(int add)
        {
            SetFov(this.fov + add);
        }

        public int GetFov()
        {
            return this.fov;
        }

        public float GetDistance()
        {
            return this.distance;
        }

        public void SetDistance(float distance)
        {
            this.distance = distance;
        }

        public void SetYaw(float yaw)
        {
            this.yaw = yaw;
        }

        public float GetYaw()
        {
            return this.yaw;
        }

        public void SetPitch(float pitch)
        {
            this.pitch = pitch;
        }

        public float GetPitch()
        {
            return this.pitch;
        }

        public Direction GetDirection()
        {
            return this.direction;
        }
    }
}
