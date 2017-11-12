using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.camera;
using ancient.game.renderers.world;
using Microsoft.Xna.Framework.Input;
using ancient.game.entity.player;
using ancient.game.client.renderer.entity;
using Microsoft.Xna.Framework;
using ancient.game.input;
using ancient.game.renderers.voxel;

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
            Ancient.ancient.player.SetHeadPitch(MathHelper.Clamp(Ancient.ancient.player.GetHeadPitch(), 0, MathHelper.PiOver2));
        }

        public override void Draw3D()
        {
            base.Draw3D();
            EntityRenderers.GetRenderEntityFromEntity(Ancient.ancient.player).Draw(Ancient.ancient.player);
            Ancient.ancient.world.GetRenderer().particleRenderer.Draw();
            DrawMap();
        }

        private void DrawMap()
        {
            Ancient.ancient.world.GetRenderer().GetChunkRenderer().DrawMap();
        }
    }
}
