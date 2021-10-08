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
using ancient.game.client.camera;
using ancient.game.client.renderer.font;

namespace ancient.game.client.gui
{
    public class GuiInventory : Gui
    {
        private EntityPlayer player;
        private Inventory inventory;

        private CameraFOV camera;
        private List<RenderTarget2D> renderTargets;
        private RenderTarget2D renderTarget;

        private float yaw;

        private int xWindow;
        private int yWindow;
        private int xTarget;
        private int yTarget;
        private Texture2D inventorySlot;
        private GuiTexture inventorySlotTex;
        private GuiTexture inventoryWindow;
        private int space;
        private int slotSize;

        private GuiScroll scroll;

        public GuiInventory(GuiManager guiManager) : base(guiManager, "inventory")
        {
            this.player = Ancient.ancient.player;
            this.inventory = player.GetInventory();
            this.camera = new CameraFOV(70, 0.01F, 100);
            this.renderTargets = new List<RenderTarget2D>();
        }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.ingame;

            renderTargets.Clear();

            for (int i = 10; i < inventory.GetSlots(); i++)
                this.renderTargets.Add(new RenderTarget2D(Ancient.ancient.device, Ancient.ancient.width, Ancient.ancient.height,
                    false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8));

            this.renderTarget = new RenderTarget2D(Ancient.ancient.device, Ancient.ancient.width, Ancient.ancient.height, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);

            this.inventorySlot = TextureManager.GetTextureFromName("inventory_slot");
            this.inventorySlotTex = new GuiTexture("inventory_slot");
            this.inventorySlotTex.ScaleToMatchResolution();

            this.inventoryWindow = new GuiTexture("inventory_window");
            this.inventoryWindow.ScaleToMatchResolution();

            this.xWindow = (int)Math.Ceiling(Ancient.ancient.width / 2F - inventoryWindow.GetWidth() / 2F);
            this.yWindow = GuiUtils.GetXFromRelativeX(0.05F);

            this.xTarget = xWindow + GuiUtils.GetScaledX(8);
            this.yTarget = yWindow + GuiUtils.GetScaledY(8);

            this.space = GuiUtils.GetScaledX(2);
            this.slotSize = GuiUtils.GetScaledX(36);

            this.scroll = new GuiScroll();
            this.scroll.ScaleToMatchResolution();
            this.scroll.SetX(GuiUtils.GetRelativeXFromX(xWindow + inventoryWindow.GetWidth() - GuiUtils.GetScaledX(13)));
            this.scroll.SetY(GuiUtils.GetRelativeYFromY(yTarget));
            this.components.Add(scroll);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            yaw += (float)Ancient.ancient.gameTime.ElapsedGameTime.TotalSeconds * 2.5F;
            guiManager.ingame.hotbarAlpha = 1.5F;
        }

        public override void Draw3D()
        {
            base.Draw3D();

            WorldRenderer.currentEffect.Parameters["FogEnabled"].SetValue(false);
            WorldRenderer.currentEffect.Parameters["View"].SetValue(camera.GetViewMatrix());
            WorldRenderer.currentEffect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());

            ItemStack[] items = inventory.GetItems();

            for (int i = 0; i < renderTargets.Count; i++)
            {
                ItemStack itemStack = items[i + 10];

                if (itemStack == null)
                    continue;

                RenderTarget2D renderTarget = renderTargets[i];

                Item item = itemStack.GetItem();
                ItemRenderer.DrawToRenderTarget(renderTarget, item, yaw, MathHelper.PiOver4, 0,
                    item.GetHandScale().X * 0.25F, item.GetHandScale().Y * 0.25F, item.GetHandScale().Z * 0.25F);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(inventoryWindow.GetTexture(), new Rectangle(xWindow, yWindow, inventoryWindow.GetWidth(), inventoryWindow.GetHeight()), Color.White);

            spriteBatch.Draw(renderTarget, new Vector2(0, yTarget), new Rectangle(0, yTarget + (int)(scroll.GetValue() * (slotSize + GuiUtils.GetScaledY(2)) * 4), renderTarget.Width, slotSize * 4 + GuiUtils.GetScaledY(2) * 3), Color.White);

            base.Draw(spriteBatch);
        }

        public void DrawInventoryToRenderTarget(SpriteBatch spriteBatch)
        {
            Ancient.ancient.device.SetRenderTarget(renderTarget);
            Ancient.ancient.device.Clear(Color.Transparent);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            for (int i = 10; i < inventory.GetSlots(); i++)
            {
                int slot = i - 10;
                int yAdd = inventorySlotTex.GetHeight() + space;
                int y = yTarget + yAdd * (slot / inventory.GetLineSize());

                int x = xTarget + (slot % inventory.GetLineSize()) * (inventorySlotTex.GetWidth() + space);

                spriteBatch.Draw(inventorySlot, new Rectangle(x, y, inventorySlotTex.GetWidth(), inventorySlotTex.GetHeight()), Color.White);

                ItemStack itemStack = inventory.GetItemStackAt(i);

                if (itemStack == null)
                    continue;

                string amount = itemStack.GetAmount().ToString();
                int size = GuiUtils.GetScaledX(1);

                int xAdd = slotSize - (int)FontRenderer.MeasureGuiText(amount, size, 0) - GuiUtils.GetScaledX(4);
                x += xAdd;

                y += slotSize - 8 * size - GuiUtils.GetScaledY(3);

                FontRenderer.DrawString(spriteBatch, amount, x, y, Color.White, size, 0, 1, Color.Black);
            }

            for (int i = 0; i < renderTargets.Count; i++)
            {
                int xAdd = (i % 6) * (slotSize + space);
                int yAdd = (i / 6) * (slotSize + space);
                spriteBatch.Draw(renderTargets[i], new Vector2(-Ancient.ancient.width / 2F + xTarget + slotSize / 2F + xAdd, -Ancient.ancient.height / 2F + yTarget + slotSize / 2F + yAdd),
                    Color.White);
            }

            spriteBatch.End();

            Ancient.ancient.device.SetRenderTarget(null);
        }

        public override bool Draw3DFromGuiManager()
        {
            return false;
        }
    }
}
