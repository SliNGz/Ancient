using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.renderer.font;
using ancient.game.renderers.world;
using Microsoft.Xna.Framework;
using ancient.game.client.utils;
using ancient.game.entity;
using ancientlib.game.entity;
using ancientlib.game.utils;

namespace ancient.game.client.gui.component
{
    class GuiText3D : GuiText
    {
        private Gui gui;

        private Vector3 velocity;
        private float sizeChange;

        private float z;
        private float maxDistance;

        private int ticksExisted;
        private int lifeSpan;

        private float hue;
        private float hueChange;

        public GuiText3D(Gui gui, string text, float x, float y, float z) : base(text)
        {
            this.gui = gui;

            this.x = x;
            this.y = y;
            this.z = z;

            this.maxDistance = 64;

            this.lifeSpan = 0;

            this.hue = -1;
        }

        public override void Update(MouseState mouseState)
        {
            this.ticksExisted++;
            SetPosition(GetPosition() + velocity / 128F);
            SetSize(this.size + sizeChange);

            UpdateHue();
            UpdateDeath();
        }

        private void UpdateHue()
        {
            hue += hueChange;

            if (hue != -1)
                this.color = Utils.HSVToRGB(hue, 1, 1);
        }

        private void UpdateDeath()
        {
            if (lifeSpan > 0)
            {
                this.color.A = (byte)(255 - (ticksExisted / (float)lifeSpan) * 255);

                if (ticksExisted >= lifeSpan)
                    gui.RemoveComponent(this);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, text, GetPosition(), color, outline, outlineColor, size, spacing, maxDistance);
        }

        public static void Draw(SpriteBatch spriteBatch, string text, Vector3 position, Color color, int outline, Color outlineColor, float size = 3, int spacing = 1, float maxDistance = -1)
        {
            float yaw = Ancient.ancient.player.GetHeadYaw();
            float pitch = Ancient.ancient.player.GetHeadPitch();

            Vector3 eyePosition = Ancient.ancient.player.GetEyePosition();
            Matrix worldMatrix = Matrix.CreateTranslation(position - eyePosition);
            Matrix view = WorldRenderer.camera.GetViewMatrix();
            Vector3 screenPosition = Ancient.ancient.GraphicsDevice.Viewport.Project(Vector3.Zero, WorldRenderer.camera.GetProjectionMatrix(), view, worldMatrix);

            if (!WorldRenderer.camera.InViewFrustum(position))
                return;

            float distance = Vector3.Distance(position, eyePosition);

            if (maxDistance != -1)
            {
                if (distance <= maxDistance)
                {
                    distance = (maxDistance - distance) / maxDistance;

                    size = (int)(size * distance * maxDistance) / maxDistance;

                    size = MathHelper.Clamp(size, 0.75F, 64);

                    float width = FontRenderer.MeasureGuiText(text, size, spacing);
                    FontRenderer.DrawString(spriteBatch, text, screenPosition.X - width / 2, screenPosition.Y, color, size, spacing, outline, outlineColor);
                }
            }
            else
            {
                float width = FontRenderer.MeasureGuiText(text, size, spacing);
                FontRenderer.DrawString(spriteBatch, text, screenPosition.X - width / 2, screenPosition.Y, color, size, spacing, outline, outlineColor);
            }
        }

        public Vector3 GetPosition()
        {
            return new Vector3(x, y, z);
        }

        public GuiText3D SetPosition(Vector3 position)
        {
            this.x = position.X;
            this.y = position.Y;
            this.z = position.Z;
            return this;
        }

        public float GetMaxDistance()
        {
            return this.maxDistance;
        }

        public GuiText3D SetMaxDistance(float maxDistance)
        {
            this.maxDistance = maxDistance;
            return this;
        }

        public int GetLifeSpan()
        {
            return this.lifeSpan;
        }

        public GuiText3D SetLifeSpan(int lifeSpan)
        {
            this.lifeSpan = lifeSpan;
            return this;
        }

        public Vector3 GetVelocity()
        {
            return this.velocity;
        }

        public GuiText3D SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
            return this;
        }

        public float GetSizeChange()
        {
            return this.sizeChange;
        }

        public GuiText3D SetSizeChange(float sizeChange)
        {
            this.sizeChange = sizeChange;
            return this;
        }

        public float GetHueChange()
        {
            return this.hueChange;
        }

        public GuiText3D SetHueChange(float hueChange)
        {
            this.hueChange = hueChange;
            return this;
        }
    }
}
