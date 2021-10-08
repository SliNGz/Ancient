using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.input.keybinding.keyaction
{
    class KeyActions
    {
        public static IKeyAction MOVE_FORWARD = new KeyActionMove(Vector3.Forward);
        public static IKeyAction MOVE_BACKWARD = new KeyActionMove(Vector3.Backward);
        public static IKeyAction MOVE_RIGHT = new KeyActionMove(Vector3.Right);
        public static IKeyAction MOVE_LEFT = new KeyActionMove(Vector3.Left);
        public static IKeyAction FLY_DOWN = new KeyActionFlyDown();
        public static IKeyAction JUMP_FLY = new KeyActionJumpFly();
        public static IKeyAction RUN = new KeyActionRun();

        public static IKeyAction TOGGLE_NO_CLIP = new KeyActionToggleNoClip();
        public static IKeyAction CHANGE_PERSPECTIVE = new KeyActionChangePerspective();

        public static IKeyAction TOGGLE_FULLSCREEN = new KeyActionToggleFullscreen();

        public static IKeyAction DISPLAY_LAST_GUI = new KeyActionDisplayLastGui();
        public static IKeyAction OPEN_MAP = new KeyActionDisplayGui(Ancient.ancient.guiManager.map);
        public static IKeyAction OPEN_INVENTORY = new KeyActionDisplayGui(Ancient.ancient.guiManager.inventory);
        public static IKeyAction OPEN_CHAT = new KeyActionDisplayGui(Ancient.ancient.guiManager.chatInput);
        public static IKeyAction OPEN_DEBUG = new KeyActionDisplayGui(Ancient.ancient.guiManager.debug);

        public static IKeyAction SWITCH_SLOT_0 = new KeyActionChangeSlot(0);
        public static IKeyAction SWITCH_SLOT_1 = new KeyActionChangeSlot(1);
        public static IKeyAction SWITCH_SLOT_2 = new KeyActionChangeSlot(2);
        public static IKeyAction SWITCH_SLOT_3 = new KeyActionChangeSlot(3);
        public static IKeyAction SWITCH_SLOT_4 = new KeyActionChangeSlot(4);
        public static IKeyAction SWITCH_SLOT_5 = new KeyActionChangeSlot(5);
        public static IKeyAction SWITCH_SLOT_6 = new KeyActionChangeSlot(6);
        public static IKeyAction SWITCH_SLOT_7 = new KeyActionChangeSlot(7);
        public static IKeyAction SWITCH_SLOT_8 = new KeyActionChangeSlot(8);
        public static IKeyAction SWITCH_SLOT_9 = new KeyActionChangeSlot(9);
        public static IKeyAction DROP_ITEM_IN_HAND = new KeyActionDropItemInHand();

        public static IKeyAction USE_SKILL_SLOT_E = new KeyActionUseSkillSlot(0);
        public static IKeyAction USE_SKILL_SLOT_R = new KeyActionUseSkillSlot(1);
        public static IKeyAction USE_SKILL_SLOT_T = new KeyActionUseSkillSlot(2);
        public static IKeyAction USE_SKILL_SLOT_F = new KeyActionUseSkillSlot(3);

        public static IKeyAction USE_SPECIAL = new KeyActionUseSpecial();

        public static IKeyAction SCREENSHOT = new KeyActionScreenshot();
    }
}