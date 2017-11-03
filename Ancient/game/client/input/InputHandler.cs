using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.input
{
    public class InputHandler
    {
        public static bool IsKeyPressed(Keys key)
        {
            return Ancient.ancient.oldKeyState.IsKeyUp(key) && Ancient.ancient.keyState.IsKeyDown(key);
        }

        public static bool IsKeyReleased(Keys key)
        {
            return Ancient.ancient.oldKeyState.IsKeyDown(key) && Ancient.ancient.keyState.IsKeyUp(key);
        }

        public static bool IsKeyHeld(Keys key)
        {
            return Ancient.ancient.oldKeyState.IsKeyDown(key) && Ancient.ancient.keyState.IsKeyDown(key);
        }

        public static bool IsKeyUp(Keys key)
        {
            return Ancient.ancient.oldKeyState.IsKeyUp(key) && Ancient.ancient.keyState.IsKeyUp(key);
        }

        public static bool IsLeftButtonPressed()
        {
            return Ancient.ancient.oldMouseState.LeftButton == ButtonState.Released && Ancient.ancient.mouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsLeftButtonReleased()
        {
            return Ancient.ancient.oldMouseState.LeftButton == ButtonState.Pressed && Ancient.ancient.mouseState.LeftButton == ButtonState.Released;
        }

        public static bool IsLeftButtonHeld()
        {
            return Ancient.ancient.oldMouseState.LeftButton == ButtonState.Pressed && Ancient.ancient.mouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsRightButtonPressed()
        {
            return Ancient.ancient.oldMouseState.RightButton == ButtonState.Released && Ancient.ancient.mouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsRightButtonReleased()
        {
            return Ancient.ancient.oldMouseState.RightButton == ButtonState.Pressed && Ancient.ancient.mouseState.RightButton == ButtonState.Released;
        }

        public static bool IsRightButtonHeld()
        {
            return Ancient.ancient.oldMouseState.RightButton == ButtonState.Pressed && Ancient.ancient.mouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsScrollingUp()
        {
            return Ancient.ancient.oldMouseState.ScrollWheelValue < Ancient.ancient.mouseState.ScrollWheelValue;
        }

        public static bool IsScrollingDown()
        {
            return Ancient.ancient.oldMouseState.ScrollWheelValue > Ancient.ancient.mouseState.ScrollWheelValue;
        }
    }
}