using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ancientlib.game.utils.DirectionHelper;

namespace ancient.game.client.camera
{
    /*    public class Camera
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
                    return Matrix.CreateLookAt(GetTargetVector(yaw, pitch) * -distance, Vector3.Zero, Vector3.Up);
            }

            public void UpdateProjection()
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

            public float GetNearPlane()
            {
                return this.nearPlane;
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
        }*/

    public abstract class Camera
    {
        protected Vector3 position;
        protected Vector3 target;

        protected Matrix view;
        protected Matrix projection;

        protected float nearPlane;
        protected float farPlane;

        private BoundingFrustum viewFrustum;

        protected Camera(float nearPlane, float farPlane)
        {
            this.position = Vector3.Zero;
            this.target = Vector3.Forward;

            this.projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(70), Ancient.ancient.device.DisplayMode.AspectRatio, nearPlane, farPlane);
            this.view = Matrix.CreateLookAt(position, target, Vector3.Up);

            this.nearPlane = nearPlane;
            this.farPlane = farPlane;

            UpdateViewFrustum();
        }

        public abstract void Update();

        protected abstract void UpdateViewMatrix();

        protected void UpdateViewFrustum()
        {
            this.viewFrustum = new BoundingFrustum(view * projection);
        }

        public Vector3 GetPosition()
        {
            return this.position;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        public Vector3 GetTargetVector()
        {
            return this.target;
        }

        public void SetTargetVector(Vector3 target)
        {
            this.target = target;
        }

        public Matrix GetViewMatrix()
        {
            return this.view;
        }

        public Matrix GetProjectionMatrix()
        {
            return this.projection;
        }

        public bool InViewFrustum(BoundingBox box)
        {
            box.Min -= Ancient.ancient.player.GetEyePosition();
            box.Max -= Ancient.ancient.player.GetEyePosition();

            return viewFrustum.Contains(box) != ContainmentType.Disjoint;
        }

        public bool InViewFrustum(Vector3 vector)
        {
            vector -= Ancient.ancient.player.GetEyePosition();

            return viewFrustum.Contains(vector) != ContainmentType.Disjoint;
        }
    }
}