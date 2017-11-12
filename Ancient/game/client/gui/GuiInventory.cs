using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using ancient.game.client.gui.component;
using ancient.game.client.utils;
using ancientlib.game.inventory;
using Microsoft.Xna.Framework.Input;
using ancientlib.game.item;
using ancient.game.client.renderer.texture;
using Microsoft.Xna.Framework;
using ancient.game.renderers.world;
using ancient.game.client.renderer.item;
using ancient.game.entity.player;
using ancient.game.utils;
using ancient.game.camera;

namespace ancient.game.client.gui
{
    public class GuiInventory : Gui
    {
        private EntityPlayer player;
        private Inventory inventory;

        private Camera camera;
        private RenderTarget2D renderTarget;

        private float yaw;

        public GuiInventory(GuiManager guiManager) : base(guiManager, "inventory")
        {
            this.player = Ancient.ancient.player;
            this.inventory = player.GetInventory();
            this.camera = new Camera(70, 0.01F, 100);
        }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.ingame;

            this.renderTarget = new RenderTarget2D(Ancient.ancient.device, Ancient.ancient.GraphicsDevice.Viewport.Width, Ancient.ancient.GraphicsDevice.Viewport.Height,
                false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            yaw += (float)Ancient.ancient.gameTime.ElapsedGameTime.TotalSeconds * 2.5F;
        }

        public override void Draw3D()
        {
            base.Draw3D();

            Ancient.ancient.device.SetRenderTarget(renderTarget);
            Ancient.ancient.world.GetRenderer().ResetGraphics(Color.Transparent);

            WorldRenderer.effect.Parameters["FogEnabled"].SetValue(false);
            WorldRenderer.effect.Parameters["View"].SetValue(camera.GetViewMatrix(0, 0));
            WorldRenderer.effect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
            ItemStack[] items = inventory.GetItems();

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                    continue;

                float y = (int)((-i / inventory.GetLineSize()) / 2F);
                float x = (i % inventory.GetLineSize()) * 0.5F;
                float xOffset = inventory.GetLineSize() * 0.5F / 2.5F;

                Item item = items[i].GetItem();
                ItemRenderer.Draw(item, new Vector3(x - xOffset, y, -2F), yaw, 0, 0, item.GetModelScale().X, item.GetModelScale().Y, item.GetModelScale().Z, true);
            }

            Ancient.ancient.device.SetRenderTarget(null);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(renderTarget, new Vector2(0, 0), Color.White);
        }
    }
}
