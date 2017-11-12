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
using ancient.game.camera;
using ancient.game.renderers.model;
using ancient.game.client.renderer.model;

namespace ancient.game.client.gui
{
    public class GuiIngame : Gui
    {
        private EntityPlayer player;
        private Inventory inventory;

        private int width;
        private int height;

        private Camera camera;
        private RenderTarget2D[] renderTargets;
        private float yaw;

        private GuiBar healthBar;
        private GuiBar manaBar;
        private GuiBar expBar;
        private GuiText level;
        private GuiText nickname;

        private GuiTexture inventoryBar;
        private GuiTexture selectedSlot;

        private int slotWidth;
        private float inventoryX;
        private float inventoryY;

        private GuiTexture crosshair;

        private GuiText itemInHand;
        private float itemInHandAlpha;

        public GuiIngame(GuiManager guiManager) : base(guiManager, "ingame")
        {
            this.camera = new Camera(70, 0.01F, 100);
            this.player = Ancient.ancient.player;
            this.inventory = player.GetInventory();
            this.renderTargets = new RenderTarget2D[10];
        }

        public override void Initialize()
        {
            base.Initialize();

            this.isCursorVisible = false;
            this.lastGui = guiManager.menu;

            this.width = Ancient.ancient.device.Viewport.Width;
            this.height = Ancient.ancient.device.Viewport.Height;

            // Level
            this.level = new GuiText("LV. 255").SetColor(Color.Gold).SetSize(2).SetSpacing(3);
            this.level.SetOutline(1);
            this.level.SetX(0.005F);
            this.level.SetY(1 - GuiUtils.GetRelativeYFromY(level.GetHeight() + 3));
            this.components.Add(level);

            this.nickname = new GuiText("SliNGy!").SetColor(Color.Gold).SetSize(2).SetSpacing(1);
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
                this.renderTargets[i] = new RenderTarget2D(Ancient.ancient.device, Ancient.ancient.device.Viewport.Width, Ancient.ancient.device.Viewport.Height,
                 false, SurfaceFormat.Color, DepthFormat.Depth24);
            }

            slotWidth = (int)(34 * Math.Round(width / (float)GuiUtils.DefaultWidth));
            inventoryX = -width / 2F + GuiUtils.GetXFromRelativeX(inventoryBar.GetX()) + (float)Math.Floor(width / (float)GuiUtils.DefaultWidth) + slotWidth / 2F;
            inventoryY = -height / 2F + GuiUtils.GetYFromRelativeY(inventoryBar.GetY()) + (float)Math.Floor(height / (float)GuiUtils.DefaultHeight) + slotWidth / 2F;
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            yaw += (float)Ancient.ancient.gameTime.ElapsedGameTime.TotalSeconds;
            UpdateGui();
        }

        private void UpdateGui()
        {
            UpdateLevel();
            UpdateHealth();
            UpdateMana();
            UpdateExp();
            UpdateItemInHandText();
            UpdateSelectedSlot();
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
            selectedSlot.SetX(inventoryBar.GetX() + GuiUtils.GetRelativeXFromX((float)-Math.Round(width / (float)GuiUtils.DefaultWidth) + slot * slotWidth));
        }

        public override void Draw3D()
        {
            base.Draw3D();

            WorldRenderer.effect.Parameters["FogEnabled"].SetValue(false);
            WorldRenderer.effect.Parameters["View"].SetValue(camera.GetViewMatrix(0, 0));
            WorldRenderer.effect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());

            for (int i = 0; i < 10; i++)
            {
                RenderTarget2D renderTarget = renderTargets[i];
                Ancient.ancient.device.SetRenderTarget(renderTarget);
                Ancient.ancient.world.GetRenderer().ResetGraphics(Color.Transparent);

                ItemStack[] items = inventory.GetItems();

                if (items[i] == null)
                    continue;

                Item item = items[i].GetItem();
                ItemRenderer.Draw(item, new Vector3(0, 0, -1F), yaw, MathHelper.PiOver4, 0, item.GetModelScale().X * 0.25F, item.GetModelScale().Y * 0.25F, item.GetModelScale().Z * 0.25F, true);
                Ancient.ancient.device.SetRenderTarget(null);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
                Item item = inventory.GetItemAt(i);

                if (item == null)
                    continue;

                spriteBatch.Draw(renderTargets[i], new Vector2(inventoryX + i * slotWidth, inventoryY), Color.White);
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
    }
}
