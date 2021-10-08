using ancient.game.client.gui.component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using ancient.game.entity.player;
using ancient.game.client.utils;
using Microsoft.Xna.Framework.Graphics;
using ancientlib.game.stats;
using ancient.game.world.chunk;
using ancientlib.game.entity;
using ancient.game.world.generator;
using ancientlib.game.world.biome;
using ancient.game.renderers.world;
using ancient.game.entity;
using ancient.game.client.renderer.texture;
using ancient.game.client.renderer.font;
using ancientlib.game.item;
using ancientlib.game.inventory;
using ancient.game.client.renderer.item;
using ancient.game.renderers.model;
using ancient.game.client.renderer.model;
using ancient.game.client.camera;

namespace ancient.game.client.gui
{
    public class GuiIngame : Gui
    {
        private EntityPlayer player;
        private Inventory inventory;

        private int width;
        private int height;

        private CameraFOV camera;
        private RenderTarget2D[] renderTargets;
        private float yaw;

        private GuiBar healthBar;
        private GuiBar manaBar;
        private GuiBar expBar;
        private GuiText level;
        private GuiText nickname;

        private GuiTexture inventoryBar;
        private GuiTexture selectedSlot;
        private int lastSlot;
        public float hotbarAlpha = 1.5F;

        private int slotSize;
        private float inventoryX;
        private float inventoryY;

        private GuiTexture crosshair;

        public GuiText itemInHand;
        private float itemInHandAlpha;

        private GuiChat chat;

        public GuiIngame(GuiManager guiManager) : base(guiManager, "ingame")
        {
            this.camera = new CameraFOV(70, 0.01F, 100);
            this.renderTargets = new RenderTarget2D[10];
        }

        public override void Initialize()
        {
            base.Initialize();

            this.isCursorVisible = false;
            this.lastGui = guiManager.menu;

            this.chat = guiManager.chat;

            this.width = Ancient.ancient.width;
            this.height = Ancient.ancient.height;

            this.player = Ancient.ancient.player;
            this.inventory = player.GetInventory();

            // Level
            this.level = new GuiText("LV. 255").SetColor(Color.Gold).SetSize(2).SetSpacing(3);
            this.level.SetOutline(1);
            this.level.SetX(0.005F);
            this.level.SetY(1 - GuiUtils.GetRelativeYFromY(level.GetHeight() + 3));
            this.components.Add(level);

            this.nickname = new GuiText(player.GetName()).SetColor(Color.Gold).SetSize(2).SetSpacing(1);
            this.nickname.SetOutline(1);
            this.nickname.SetX(level.GetX());
            this.nickname.SetY(level.GetY() - GuiUtils.GetRelativeYFromY(nickname.GetHeight() + 3));
            this.components.Add(nickname);

            this.inventoryBar = new GuiTexture("inventory_bar");
            this.inventoryBar.ScaleToMatchResolution();
            this.inventoryBar.SetX(0.5F - GuiUtils.GetRelativeXFromX(inventoryBar.GetWidth() / 2F));
            this.inventoryBar.SetY(1 - GuiUtils.GetRelativeYFromY(inventoryBar.GetHeight() + 50));
            this.components.Add(inventoryBar);

            this.selectedSlot = new GuiTexture("selected_slot");
            this.selectedSlot.ScaleToMatchResolution();
            this.selectedSlot.SetX(inventoryBar.GetX() - GuiUtils.GetRelativeXFromX((float)Math.Round(width / (float)GuiUtils.DefaultWidth)));
            this.selectedSlot.SetY(inventoryBar.GetY() - GuiUtils.GetRelativeYFromY((float)Math.Round(height / (float)GuiUtils.DefaultHeight)));
            this.components.Add(selectedSlot);

            // Exp Bar
            GuiText expText = new GuiText("EXP.", 2, 1).SetOutline(1);
            expText.SetX(inventoryBar.GetX() - GuiUtils.GetRelativeXFromX(inventoryBar.GetWidth() / 4F));
            expText.SetY(1 - GuiUtils.GetRelativeYFromY(expText.GetHeight() + 3));
            this.components.Add(expText);

            this.expBar = new GuiBar("exp_bar");
            this.expBar.SetWidth(470);
            expBar.ScaleToMatchResolution();
            this.expBar.SetHeight(20);
            this.expBar.SetX(expText.GetX() + GuiUtils.GetRelativeXFromX(expText.GetWidth() + 3));
            this.expBar.SetY(expText.GetY() + GuiUtils.GetRelativeYFromY(expText.GetHeight() - expBar.GetHeight()));

            GuiText expValue = new GuiText().SetColor(Color.White).SetSize(2);
            expValue.SetOutline(1);
            expValue.SetX(expBar.GetX() + 0.005F);
            expValue.SetY(expBar.GetY() - GuiUtils.GetRelativeYFromY(expValue.GetHeight()) - 0.005F);
            this.expBar.SetGuiText(expValue);
            this.components.Add(expBar);

            // Health Bar
            GuiText healthText = new GuiText("HP.", 2, 1).SetOutline(1);
            healthText.SetX(expText.GetX());
            healthText.SetY(expText.GetY() - GuiUtils.GetRelativeYFromY(healthText.GetHeight() + 7));
            this.components.Add(healthText);

            this.healthBar = new GuiBar("health_bar");
            this.healthBar.SetWidth((int)(expBar.GetWidth() / 2F - 5));
            this.healthBar.SetX(expBar.GetX());
            this.healthBar.SetY(healthText.GetY() + GuiUtils.GetRelativeYFromY(healthText.GetHeight() - healthBar.GetHeight()));

            GuiText healthValue = new GuiText("", 2, 1).SetOutline(1);
            this.healthBar.SetGuiText(healthValue);
            this.components.Add(healthBar);

            // Mana Bar
            this.manaBar = new GuiBar("mana_bar");
            this.manaBar.SetWidth(healthBar.GetWidth());
            this.manaBar.SetX(healthBar.GetX() + GuiUtils.GetRelativeXFromX(healthBar.GetWidth() + 10));
            this.manaBar.SetY(healthBar.GetY());

            GuiText manaValue = new GuiText("", 2, 1).SetOutline(1);
            this.manaBar.SetGuiText(manaValue);
            this.components.Add(manaBar);

            //Skill slots
            GuiTexture useBar = new GuiTexture("use_bar");
            useBar.SetX(1 - GuiUtils.GetRelativeXFromX(useBar.GetWidth()));
            useBar.SetY(1 - GuiUtils.GetRelativeYFromY(useBar.GetHeight() + 3));
            this.components.Add(useBar);

            this.crosshair = new GuiTexture("crosshair");
            this.crosshair.Centralize();
            this.components.Add(crosshair);

            this.itemInHand = new GuiText("", 3, 5);
            this.itemInHand.Centralize();
            this.itemInHand.SetY(inventoryBar.GetY() - GuiUtils.GetRelativeYFromY(itemInHand.GetHeight()) - 0.02F);
            this.itemInHand.SetOutline(1);
            this.components.Add(itemInHand);

            for (int i = 0; i < 10; i++)
            {
                this.renderTargets[i] = new RenderTarget2D(Ancient.ancient.device, Ancient.ancient.width, Ancient.ancient.height,
                 false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
            }

            slotSize = (int)(34 * Math.Round(width / (float)GuiUtils.DefaultWidth));
            inventoryX = -width / 2F + GuiUtils.GetXFromRelativeX(inventoryBar.GetX()) + (float)Math.Floor(width / (float)GuiUtils.DefaultWidth) + slotSize / 2F;
            inventoryY = -height / 2F + GuiUtils.GetYFromRelativeY(inventoryBar.GetY()) + (float)Math.Floor(height / (float)GuiUtils.DefaultHeight) + slotSize / 2F;
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);

            this.player = Ancient.ancient.player;
            this.inventory = player.GetInventory();

            yaw += (float)Ancient.ancient.gameTime.ElapsedGameTime.TotalSeconds;
            UpdateGui();
            chat.Update(mouseState);
        }

        private void UpdateGui()
        {
            UpdateLevel();
            UpdateHealth();
            UpdateMana();
            UpdateExp();
            UpdateItemInHandText();
            UpdateSelectedSlot();
            this.nickname.SetText(player.GetName());
        }

        private void UpdateLevel()
        {
            this.level.SetText("LV. " + player.GetLevel());
        }

        private void UpdateHealth()
        {
            this.healthBar.SetMaxValue(player.GetMaxHealth());
            this.healthBar.SetValue(player.GetHealth());
            this.healthBar.GetGuiText().SetText(healthBar.GetValue() + "/" + healthBar.GetMaxValue());
            this.healthBar.CentralizeText();
        }

        private void UpdateMana()
        {
            this.manaBar.SetMaxValue(player.GetMaxMana());
            this.manaBar.SetValue(player.GetMana());
            this.manaBar.GetGuiText().SetText(manaBar.GetValue() + "/" + manaBar.GetMaxValue());
            this.manaBar.CentralizeText();
        }

        private void UpdateExp()
        {
            this.expBar.SetMaxValue(StatTable.GetExpToNextLevel(player.GetLevel()));
            this.expBar.SetValue(player.GetExp());
            this.expBar.GetGuiText().SetText(expBar.GetValue() + "/" + expBar.GetMaxValue());
            this.expBar.CentralizeText();
        }

        private void UpdateItemInHandText()
        {
            itemInHandAlpha = MathHelper.Clamp(itemInHandAlpha - 1 / 256F, 0, 1);
            itemInHand.SetColor(Color.White * itemInHandAlpha);
            itemInHand.SetOutlineColor(Color.Black * itemInHandAlpha);
        }

        private void UpdateSelectedSlot()
        {
            int slot = player.GetHandSlot();

            if (lastSlot != slot)
            {
                selectedSlot.SetX(inventoryBar.GetX() + GuiUtils.GetRelativeXFromX((float)-Math.Round(width / (float)GuiUtils.DefaultWidth) + slot * slotSize));
                hotbarAlpha = 1.5F;
            }

            if (hotbarAlpha > 0)
            {
                hotbarAlpha = MathHelper.Clamp(hotbarAlpha - 0.0025F, 0.5F, 1.5F);
                inventoryBar.SetColor(Color.White * hotbarAlpha);
                selectedSlot.SetColor(Color.White * hotbarAlpha);
            }

            this.lastSlot = slot;
        }

        public override void Draw3D()
        {
            base.Draw3D();

            WorldRenderer.currentEffect.Parameters["FogEnabled"].SetValue(false);
            WorldRenderer.currentEffect.Parameters["View"].SetValue(camera.GetViewMatrix());
            WorldRenderer.currentEffect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());

            ItemStack[] items = inventory.GetItems();

            for (int i = 0; i < 10; i++)
            {
                if (items[i] == null)
                    continue;

                RenderTarget2D renderTarget = renderTargets[i];

                Item item = items[i].GetItem();
                ItemRenderer.DrawToRenderTarget(renderTarget, item, yaw, MathHelper.PiOver4, 0,
                    item.GetHandScale().X * 0.25F, item.GetHandScale().Y * 0.25F, item.GetHandScale().Z * 0.25F);
            }

            chat.Draw3D();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            chat.Draw(spriteBatch);
            DrawEntitiesNames(spriteBatch);
            DrawInventoryHotbar(spriteBatch);
        }

        private void DrawEntitiesNames(SpriteBatch spriteBatch)
        {
            List<Entity> entityList = player.GetWorld().entityList;
            for (int i = 0; i < entityList.Count; i++)
            {
                Entity entity = entityList[i];

                if (entity is EntityPlayer)
                {
                    EntityPlayer player = (EntityPlayer)entity;

                    if (player != this.player)
                        GuiText3D.Draw(spriteBatch, player.GetName(), player.GetPosition() + new Vector3(0, 0.3F, 0), Color.White, 1, Color.Black, 3, 2, 16);
                }

                if (entity is EntityPet)
                {
                    EntityPet pet = (EntityPet)entity;

                    if (pet.HasOwner())
                        GuiText3D.Draw(spriteBatch, pet.GetName(), pet.GetPosition(), pet.GetNameColor(), 1, Color.Black, 3, 2, 16);
                }
            }
        }

        private void DrawInventoryHotbar(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < 10; i++)
            {
                ItemStack itemStack = inventory.GetItemStackAt(i);

                if (itemStack == null)
                    continue;

                spriteBatch.Draw(renderTargets[i], new Vector2(inventoryX + i * slotSize, inventoryY), Color.White * hotbarAlpha);

                int size = GuiUtils.GetScaledX(1);
                string amount = itemStack.GetAmount().ToString();
                float x = Ancient.ancient.screenCenter.X + inventoryX + i * slotSize + slotSize / 2 - (int)FontRenderer.MeasureGuiText(amount, size, 0) - GuiUtils.GetScaledX(3);
                float y = Ancient.ancient.screenCenter.Y + inventoryY + slotSize / 2 - 8 * size - GuiUtils.GetScaledY(2);
                FontRenderer.DrawString(spriteBatch, amount, x, y, Color.White, size, 0, 1, Color.Black);
            }
        }

        public void OnPlayerChangeItemInHand(ItemStack hand)
        {
            if (hand != null)
            {
                itemInHand.SetText(hand.GetItem().GetName());
                float y = itemInHand.GetY();
                itemInHand.Centralize();
                itemInHand.SetY(y);

                itemInHandAlpha = 1;
            }
            else
                itemInHand.SetText("");
        }

        public override bool Draw3DFromGuiManager()
        {
            return false;
        }
    }
}
