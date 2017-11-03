using ancient.game.client.gui.component;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.utils
{
    class GuiUtils
    {
        public static int DefaultWidth
        {
            get { return 800; }
        }

        public static int DefaultHeight
        {
            get { return 480; }
        }

        public static int GetXFromRelativeX(float x)
        {
            return (int)Math.Round(Ancient.ancient.GraphicsDevice.Viewport.Width * x);
        }

        public static int GetYFromRelativeY(float y)
        {
            return (int)Math.Round(Ancient.ancient.GraphicsDevice.Viewport.Height * y);
        }

        public static float GetRelativeXFromX(float x)
        {
            return x / Ancient.ancient.GraphicsDevice.Viewport.Width;
        }

        public static float GetRelativeYFromY(float y)
        {
            return y / Ancient.ancient.GraphicsDevice.Viewport.Height;
        }
    }
}
