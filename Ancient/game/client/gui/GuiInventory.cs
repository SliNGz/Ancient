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

namespace ancient.game.client.gui
{
    public class GuiInventory : Gui
    {
        private EntityPlayer player;
        private Inventory inventory;

        private GuiTexture window;
        private GuiTexture slots;

        private float yaw;

        public GuiInventory(GuiManager guiManager) : base(guiManager, "inventory")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.lastGui = guiManager.ingame;

            this.window = new GuiTexture("inventory_window");
            this.window.SetWidth(window.GetWidth());
            this.window.SetHeight(window.GetHeight());
            this.window.SetX(0.8F);
            this.window.SetY(0.1F);
            //   this.components.Add(window);

            this.slots = new GuiTexture("inventory_slots");
            this.slots.SetWidth(slots.GetWidth());
            this.slots.SetHeight(slots.GetHeight());
            this.slots.SetX(window.GetX() + GuiUtils.GetRelativeXFromX(6));
            this.slots.SetY(window.GetY() + GuiUtils.GetRelativeYFromY(12));
            //    this.components.Add(slots);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            yaw += (float)Ancient.ancient.gameTime.ElapsedGameTime.TotalSeconds * 2.5F;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.End();

            Ancient.ancient.device.RasterizerState = Ancient.ancient.world.GetRenderer().rs;
            Ancient.ancient.device.BlendState = BlendState.Opaque;
            Ancient.ancient.device.DepthStencilState = DepthStencilState.Default;

            this.player = Ancient.ancient.player;
            this.inventory = player.GetInventory();

            ItemStack[] items = inventory.GetItems();

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                    continue;

                int x = GuiUtils.GetXFromRelativeX(slots.GetX()) + (i % inventory.GetLineSize()) * 34 + 5;
                int y = GuiUtils.GetYFromRelativeY(slots.GetY()) + (i / inventory.GetLineSize()) * 34 + 5;

                Vector2 screenPosition = new Vector2(x, y);

                Vector3 lookAt = Vector3.Transform(new Vector3(i / 32F - 0.1F, 0, -0.1f), Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), player.GetHeadPitch(), 0));
                Vector3 position = lookAt;

                // ItemRenderer.Draw(items[i].GetItem(), position, 0, -player.GetHeadYaw(), player.GetHeadPitch() + yaw, true);
                Item item = items[i].GetItem();
                Vector3 vertical = Vector3.Transform(Vector3.Forward, Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), player.GetHeadPitch() + MathHelper.PiOver2, 0));
                //  RenderUtils.DrawLine(player.GetPosition(), player.GetPosition() + player.GetLookAt(), Color.Yellow);
                //  RenderUtils.DrawLine(player.GetPosition(), player.GetPosition() + vertical, Color.Purple);

                ItemRenderer.Draw(item, lookAt, player.GetHeadYaw(), player.GetHeadPitch(), 0, item.GetModelScale().X * 0.1F, item.GetModelScale().Y * 0.1F, item.GetModelScale().Z * 0.1F, true);
            }

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointWrap, null, null, null, null); // Drawing cursor.
        }
    }
}
