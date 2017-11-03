using ancient.game.entity.player;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.client.input.keybinding.keyaction;
using ancient.game.input;
using ancient.game.client.gui;

namespace ancient.game.client.input.keybinding
{
    public class KeyBinding
    {
        private Keys key;
        private IKeyAction keyAction;
        private List<object> supportedGuis; // Guis in which this keybinding can be activated, if supportedGuis is null - keybinding can be activated anywhere.

        public KeyBinding(Keys key, IKeyAction keyAction, params object[] supportedGuis)
        {
            this.key = key;
            this.keyAction = keyAction;
            this.supportedGuis = supportedGuis.ToList();
        }

        public void Update(EntityPlayer player)
        {
            Gui currentGui = Ancient.ancient.guiManager.GetCurrentGui();
            if (currentGui != Ancient.ancient.guiManager.debug && (supportedGuis.Count > 0 && !supportedGuis.Contains(currentGui) || (supportedGuis.Contains(currentGui) && !currentGui.CanClose())))
                return;

            if (keyAction != null)
            {
                if (IsHeld())
                    keyAction.UpdateHeld(player);

                if (IsPressed())
                    keyAction.UpdatePressed(player);

                if (IsReleased())
                    keyAction.UpdateReleased(player);

                if (IsUp())
                    keyAction.UpdateUp(player);
            }
        }

        public bool IsHeld()
        {
            return InputHandler.IsKeyHeld(key);
        }

        public bool IsPressed()
        {
            return InputHandler.IsKeyPressed(key);
        }

        public bool IsReleased()
        {
            return InputHandler.IsKeyReleased(key);
        }

        public bool IsUp()
        {
            return InputHandler.IsKeyUp(key);
        }

        public Keys GetKey()
        {
            return this.key;
        }

        public void SetKey(Keys key)
        {
            this.key = key;
        }

        public IKeyAction GetKeyAction()
        {
            return this.keyAction;
        }

        public void SetKeyAction(IKeyAction keyAction)
        {
            this.keyAction = keyAction;
        }
    }
}
