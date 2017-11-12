using ancient.game.camera;
using ancient.game.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.renderers.world;

namespace ancient.game.client.gui
{
    public class GuiCameraScroller : Gui
    {
        public float distance;

        protected float minDistance;
        protected float maxDistance;

        protected float scrollValue;

        protected Camera camera;

        public GuiCameraScroller(GuiManager guiManager, string name, float minDistance, float maxDistance, float scrollValue) : base(guiManager, name)
        {
            this.distance = (minDistance + maxDistance) / 2F;
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
            this.scrollValue = scrollValue;

            this.drawWorldBehind = false;

            this.camera = new Camera(70, 0.1f, 1000);
            camera.SetDistance(distance);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            UpdateCameraDistance();
        }

        public override void Draw3D()
        {
            WorldRenderer.effect.Parameters["FogEnabled"].SetValue(false);
            WorldRenderer.effect.Parameters["View"].SetValue(camera.GetViewMatrix(Ancient.ancient.player.GetHeadYaw(), Ancient.ancient.player.GetHeadPitch()));
            WorldRenderer.effect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
        }

        protected virtual void UpdateCameraDistance()
        {
            if (InputHandler.IsScrollingUp())
                this.distance = MathHelper.Clamp(distance - scrollValue, minDistance, maxDistance);
            else if (InputHandler.IsScrollingDown())
                this.distance = MathHelper.Clamp(distance + scrollValue, minDistance, maxDistance);

            camera.SetDistance(-distance);
        }
    }
}
