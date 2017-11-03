using ancient.game.client.gui;
using ancient.game.client.input.keybinding.keyaction;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.input.keybinding
{
    public class KeyBindings
    {
        public static List<KeyBinding> keyBindings;

        public static void Initialize()
        {
            GuiManager guiManager = Ancient.ancient.guiManager;

            GuiIngame ingame = guiManager.ingame;

            keyBindings = new List<KeyBinding>();

            AddKeyBinding(Keys.W, KeyActions.MOVE_FORWARD, ingame);
            AddKeyBinding(Keys.A, KeyActions.MOVE_LEFT, ingame);
            AddKeyBinding(Keys.S, KeyActions.MOVE_BACKWARD, ingame);
            AddKeyBinding(Keys.D, KeyActions.MOVE_RIGHT, ingame);
            AddKeyBinding(Keys.Space, KeyActions.JUMP_FLY, ingame);
            AddKeyBinding(Keys.LeftControl, KeyActions.FLY_DOWN, ingame);
            AddKeyBinding(Keys.LeftShift, KeyActions.RUN, ingame);

            AddKeyBinding(Keys.C, KeyActions.TOGGLE_NO_CLIP, ingame);

            AddKeyBinding(Keys.V, KeyActions.CHANGE_PERSPECTIVE, ingame);

            AddKeyBinding(Keys.D0, KeyActions.SWITCH_SLOT_0, ingame);
            AddKeyBinding(Keys.D1, KeyActions.SWITCH_SLOT_1, ingame);
            AddKeyBinding(Keys.D2, KeyActions.SWITCH_SLOT_2, ingame);
            AddKeyBinding(Keys.D3, KeyActions.SWITCH_SLOT_3, ingame);
            AddKeyBinding(Keys.D4, KeyActions.SWITCH_SLOT_4, ingame);
            AddKeyBinding(Keys.D5, KeyActions.SWITCH_SLOT_5, ingame);
            AddKeyBinding(Keys.D6, KeyActions.SWITCH_SLOT_6, ingame);
            AddKeyBinding(Keys.D7, KeyActions.SWITCH_SLOT_7, ingame);
            AddKeyBinding(Keys.D8, KeyActions.SWITCH_SLOT_8, ingame);
            AddKeyBinding(Keys.D9, KeyActions.SWITCH_SLOT_9, ingame);

            AddKeyBinding(Keys.G, KeyActions.DROP_ITEM_IN_HAND, ingame);

            AddKeyBinding(Keys.I, KeyActions.OPEN_INVENTORY, ingame, guiManager.inventory);
            AddKeyBinding(Keys.M, KeyActions.OPEN_MAP, ingame, guiManager.map);
            AddKeyBinding(Keys.Enter, KeyActions.OPEN_CHAT, ingame, guiManager.chat);
            AddKeyBinding(Keys.F1, KeyActions.OPEN_DEBUG, ingame, guiManager.debug);

            AddKeyBinding(Keys.Escape, KeyActions.DISPLAY_LAST_GUI);

            AddKeyBinding(Keys.F11, KeyActions.TOGGLE_FULLSCREEN);

            AddKeyBinding(Keys.E, KeyActions.USE_SKILL_SLOT_E);
        }

        public static void AddKeyBinding(Keys key, IKeyAction keyAction, params object[] supportedGuis)
        {
            keyBindings.Add(new KeyBinding(key, keyAction, supportedGuis));
        }

        public static void SetKeyBinding(Keys key, IKeyAction keyAction)
        {
            KeyBinding keyBinding = keyBindings.First(x => x.GetKey() == key);

            if (keyBinding != null)
                keyBinding.SetKeyAction(keyAction);
        }
    }
}
