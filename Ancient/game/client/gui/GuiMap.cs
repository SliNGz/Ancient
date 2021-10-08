using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.renderers.world;
using Microsoft.Xna.Framework.Input;
using ancient.game.entity.player;
using ancient.game.client.renderer.entity;
using Microsoft.Xna.Framework;
using ancient.game.input;
using ancient.game.renderers.voxel;
using ancient.game.client.camera;
using ancient.game.client.renderer.model;

namespace ancient.game.client.gui
{
    public class GuiMap : GuiCameraScroller
    {
        private static float MIN_DISTANCE = 20;
        private static float MAX_DISTANCE = 120;
        private static float SCROLL_VALUE = 2;

        public GuiMap(GuiManager guiManager) : base(guiManager, "map", MIN_DISTANCE, MAX_DISTANCE, SCROLL_VALUE)
        {
            this.backgroundColor = Color.BurlyWood;

            this.isCursorVisible = false;

            camera.SetPitch(MathHelper.PiOver4);
        }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.ingame;
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);

            this.backgroundColor = Ancient.ancient.world.skyColor;

            camera.SetYaw(Ancient.ancient.player.GetHeadYaw());
            //camera.SetPitch(MathHelper.Clamp(Ancient.ancient.player.GetHeadPitch(), 0, MathHelper.PiOver2));
        }

        public override void Draw3D()
        {
            base.Draw3D();
            WorldRenderer.currentEffect.Parameters["FogEnabled"].SetValue(false);
            WorldRenderer.currentEffect.Parameters["View"].SetValue(camera.GetViewMatrix());
            WorldRenderer.currentEffect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
            Ancient.ancient.world.GetRenderer().Technique = 0;
            WorldRenderer.currentEffect.Parameters["ShadowsEnabled"].SetValue(false);
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            EntityRenderers.renderPlayer.Draw(Ancient.ancient.player);
            Ancient.ancient.world.GetRenderer().particleRenderer.Draw();
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);
            DrawMap();
        }

        private void DrawMap()
        {
            Ancient.ancient.world.GetRenderer().GetChunkRenderer().DrawMap();
        }
    }
}
