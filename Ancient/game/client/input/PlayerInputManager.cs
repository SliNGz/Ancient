using ancient.game.client.gui;
using ancient.game.client.input;
using ancient.game.client.input.keybinding;
using ancient.game.client.particle;
using ancient.game.entity.player;
using ancientlib.game.init;
using ancientlib.game.item;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;

namespace ancient.game.input
{
    public class PlayerInputManager
    {
        private EntityPlayer player;

        private Gui[] canUpdateGuis;

        public IPlayerInput playerInput;
        public OfflineInputHandler offlineInput;
        public OnlineInputHandler onlineInput;

        public PlayerInputManager()
        {
            this.player = Ancient.ancient.player;

            this.canUpdateGuis = new Gui[3];
            this.canUpdateGuis[0] = Ancient.ancient.guiManager.ingame;
            this.canUpdateGuis[1] = Ancient.ancient.guiManager.map;
            this.canUpdateGuis[2] = Ancient.ancient.guiManager.debug;

            this.offlineInput = new OfflineInputHandler();
            this.onlineInput = new OnlineInputHandler();
            this.playerInput = offlineInput;
        }

        public void Update(GameTime gameTime)
        {
            if (Ancient.ancient.IsActive)
            {
                UpdateMouseInput(gameTime);
                UpdateKeyboardInput();
            }
        }

        private void UpdateMouseInput(GameTime gameTime)
        {
            UpdateMouseCamera(gameTime);
            UpdateMouseGameplayInput();
        }

        public void UpdateMouseCamera(GameTime gameTime)
        {
            if (canUpdateGuis.Contains(Ancient.ancient.guiManager.GetCurrentGui()))
            {
                if (Mouse.GetState().Position.ToVector2() != Ancient.ancient.screenCenter)
                {
                    Vector3 delta = new Vector3(Ancient.ancient.screenCenter.X - Ancient.ancient.mouseState.Position.X, Ancient.ancient.screenCenter.Y - Ancient.ancient.mouseState.Position.Y, 0)
                        * Ancient.ancient.gameSettings.GetSensitivity() * (float)gameTime.ElapsedGameTime.TotalSeconds / 10;

                    float yaw = player.GetHeadYaw() + delta.X;
                    float pitch = player.GetHeadPitch() + delta.Y;

                    player.SetYaw(yaw);
                    player.SetHeadRotation(yaw, pitch);

                    Mouse.SetPosition((int)Ancient.ancient.screenCenter.X, (int)Ancient.ancient.screenCenter.Y);
                }
            }
        }

        private void UpdateMouseGameplayInput()
        {
            if (Ancient.ancient.IsIngame())
            {
                if (InputHandler.IsLeftButtonHeld())
                    playerInput.OnLeftButtonHeld();

                else if (InputHandler.IsLeftButtonPressed())
                    playerInput.OnLeftButtonPressed();

                else if (InputHandler.IsLeftButtonReleased())
                    playerInput.OnLeftButtonReleased();

                if (InputHandler.IsRightButtonHeld())
                    playerInput.OnRightButtonHeld();

                else if (InputHandler.IsRightButtonPressed())
                    playerInput.OnRightButtonPressed();

                else if (InputHandler.IsRightButtonReleased())
                    playerInput.OnRightButtonReleased();

                playerInput.Update();
            }
        }

        private void UpdateLeftButton()
        {
            if (player.GetItemInHand() != null && player.GetItemInHand().GetItem() is ItemWeapon)
            {
                if (InputHandler.IsLeftButtonHeld())
                    player.UseItemInHand();
            }
            else
            {
                if (InputHandler.IsLeftButtonPressed())
                    player.UseItemInHand();
            }
        }

        private void UpdateRightButton()
        {
            if (InputHandler.IsRightButtonPressed())
            {
                bool interacted = player.Interact();

                if (!interacted)
                    player.UseItemInHandRightClick();

                return;
            }
        }

        private void UpdateKeyboardInput()
        {
            foreach (KeyBinding keyBinding in KeyBindings.keyBindings)
                keyBinding.Update(player);
        }
    }
}