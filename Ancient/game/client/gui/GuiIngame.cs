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

namespace ancient.game.client.gui
{
    public class GuiIngame : Gui
    {
        private EntityPlayer player;

        private GuiBar healthBar;
        private GuiBar manaBar;
        private GuiBar expBar;
        private GuiText level;

        private GuiTexture crosshair;

        private GuiText itemInHand;
        private float itemInHandAlpha;

        public GuiIngame(GuiManager guiManager) : base(guiManager, "ingame")
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.player = Ancient.ancient.player;
            this.isCursorVisible = false;
            this.lastGui = guiManager.menu;

            // Level
            this.level = new GuiText("LV. 255").SetColor(Color.Gold).SetSize(2.5F).SetSpacing(1);
            this.level.SetOutline(1);
            this.level.SetX(0.005F);
            this.level.SetY(0.995F - GuiUtils.GetRelativeYFromY(level.GetHeight()));
            this.components.Add(level);

            // Health Bar
            this.healthBar = new GuiBar("health_bar");
            this.healthBar.SetX(level.GetX() + GuiUtils.GetRelativeXFromX(level.GetWidth()) + 0.0025F);
            this.healthBar.SetY(0.995F - GuiUtils.GetRelativeYFromY(healthBar.GetHeight()));

            GuiText healthText = new GuiText().SetColor(Color.Gold).SetSize(2);
            healthText.SetOutline(1);
            healthText.SetX(healthBar.GetX() + 0.005F);
            healthText.SetY(healthBar.GetY() - GuiUtils.GetRelativeYFromY(healthText.GetHeight()) - 0.005F);
            this.healthBar.SetGuiText(healthText);
            this.components.Add(healthBar);

            // Mana Bar
            this.manaBar = new GuiBar("mana_bar");
            this.manaBar.SetX(healthBar.GetX() + GuiUtils.GetRelativeXFromX(healthBar.GetWidth()) + 0.0025F);
            this.manaBar.SetY(healthBar.GetY());

            GuiText manaText = new GuiText().SetColor(Color.Gold).SetSize(2);
            manaText.SetOutline(1);
            manaText.SetX(manaBar.GetX() + 0.005F);
            manaText.SetY(manaBar.GetY() - GuiUtils.GetRelativeYFromY(manaText.GetHeight()) - 0.005F);
            this.manaBar.SetGuiText(manaText);
            this.components.Add(manaBar);

            // Exp Bar
            this.expBar = new GuiBar("exp_bar");
            this.expBar.SetX(manaBar.GetX() + GuiUtils.GetRelativeXFromX(manaBar.GetWidth()) + 0.0025F);
            this.expBar.SetY(healthBar.GetY());

            GuiText expText = new GuiText().SetColor(Color.Gold).SetSize(2);
            expText.SetOutline(1);
            expText.SetX(expBar.GetX() + 0.005F);
            expText.SetY(expBar.GetY() - GuiUtils.GetRelativeYFromY(expText.GetHeight()) - 0.005F);
            this.expBar.SetGuiText(expText);
            this.components.Add(expBar);

            //Skill slots
            GuiTexture useBar = new GuiTexture("use_bar");
            useBar.ScaleToMatchResolution();
            useBar.SetX(0.995F - GuiUtils.GetRelativeXFromX(useBar.GetWidth()));
            useBar.SetY(0.995F - GuiUtils.GetRelativeYFromY(useBar.GetHeight()));
            this.components.Add(useBar);

            GuiTexture inventoryBar = new GuiTexture("inventory_bar");
            inventoryBar.ScaleToMatchResolution();
            inventoryBar.SetX(0.995F - GuiUtils.GetRelativeXFromX(inventoryBar.GetWidth()));
            inventoryBar.SetY(useBar.GetY() - GuiUtils.GetRelativeYFromY(inventoryBar.GetHeight()) - 0.05F);
            this.components.Add(inventoryBar);

            this.crosshair = new GuiTexture("crosshair");
            this.crosshair.Centralize();
            this.components.Add(crosshair);

            this.itemInHand = new GuiText("", 3, 5);
            this.itemInHand.Centralize();
            this.itemInHand.SetY(healthText.GetY() - GuiUtils.GetRelativeYFromY(itemInHand.GetHeight()) - 0.02F);
            this.itemInHand.SetOutline(1);
            this.components.Add(itemInHand);
        }

        public override void Update(MouseState mouseState)
        {
            base.Update(mouseState);
            UpdateGui();
        }

        private void UpdateGui()
        {
            UpdateLevel();
            UpdateHealth();
            UpdateMana();
            UpdateExp();
            UpdateItemInHandText();
        }

        private void UpdateLevel()
        {
            this.level.SetText("LV. " + player.GetLevel());
        }

        private void UpdateHealth()
        {
            this.healthBar.SetMaxValue(player.GetMaxHealth());
            this.healthBar.SetValue(player.GetHealth());
            this.healthBar.GetGuiText().SetText("HP: " + healthBar.GetValue() + "/" + healthBar.GetMaxValue());
        }

        private void UpdateMana()
        {
            this.manaBar.SetMaxValue(player.GetMaxMana());
            this.manaBar.SetValue(player.GetMana());
            this.manaBar.GetGuiText().SetText("MP: " + manaBar.GetValue() + "/" + manaBar.GetMaxValue());
        }

        private void UpdateExp()
        {
            this.expBar.SetMaxValue(StatTable.GetExpToNextLevel(player.GetLevel()));
            this.expBar.SetValue(player.GetExp());
            this.expBar.GetGuiText().SetText("EXP: " + expBar.GetValue() + "/" + expBar.GetMaxValue());
        }

        private void UpdateItemInHandText()
        {
            itemInHandAlpha = MathHelper.Clamp(itemInHandAlpha - 1 / 256F, 0, 1);
            itemInHand.SetColor(Color.White * itemInHandAlpha);
            itemInHand.SetOutlineColor(Color.Black * itemInHandAlpha);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (player == null)
                return;

            base.Draw(spriteBatch);
            DrawEntitiesNames(spriteBatch);
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
