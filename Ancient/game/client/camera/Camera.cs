using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ancientlib.game.utils.DirectionHelper;

namespace ancient.game.camera
{
    public class Camera
    {
        private Ancient ancient;

        private float nearPlane;
        private float farPlane;

        private Matrix projection;

        private int fov;

        public float distance;

        public Camera(int fov, float nearPlane, float farPlane)
        {
            this.ancient = Ancient.ancient;

            this.nearPlane = nearPlane;
            this.farPlane = farPlane;

            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), ancient.device.DisplayMode.AspectRatio, nearPlane, farPlane);

            this.fov = fov;

            this.distance = 0;
        }

        public Matrix GetViewMatrix(float yaw, float pitch)
        {
            if (distance == 0)
                return Matrix.CreateLookAt(Vector3.Zero, GetTargetVector(yaw, pitch), Vector3.Up);
            else
                return Matrix.CreateLookAt(GetTargetVector(yaw, pitch) * distance, Vector3.Zero, Vector3.Up);
        }

        private void UpdateProjection()
        {
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), ancient.device.DisplayMode.AspectRatio, nearPlane, farPlane);
        }

        public Matrix GetProjectionMatrix()
        {
            return this.projection;
        }

        public Vector3 GetTargetVector(float yaw, float pitch)
        {
            return Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(yaw, pitch, 0));
        }

        public float GetFarPlane()
        {
            return this.farPlane;
        }

        public void SetFarPlane(float farPlane)
        {
            this.farPlane = farPlane;
            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(fov), ancient.device.DisplayMode.AspectRatio, nearPlane, farPlane);
        }

        public BoundingFrustum GetViewingFrustum(float yaw, float pitch)
        {
            return new BoundingFrustum(GetViewMatrix(yaw, pitch) * projection);
        }

        public bool InViewFrustum(BoundingBox box, float yaw, float pitch)
        {
            box.Min -= ancient.player.GetEyePosition();
            box.Max -= ancient.player.GetEyePosition();

            return GetViewingFrustum(yaw, pitch).Contains(box) != ContainmentType.Disjoint;
        }

        public bool InViewFrustum(Vector3 vector, float yaw, float pitch)
        {
            vector -= ancient.player.GetEyePosition();

            return GetViewingFrustum(yaw, pitch).Contains(vector) != ContainmentType.Disjoint;
        }

        public Direction GetDirection(float yaw, float pitch)
        {
            return DirectionHelper.GetDirection(yaw, pitch);
        }

        public float GetDistance()
        {
            return this.distance;
        }

        public void SetDistance(float distance)
        {
            this.distance = distance;
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
    }
}