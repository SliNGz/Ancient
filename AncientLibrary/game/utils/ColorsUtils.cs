using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public static class ColorsUtils
    {
        public static int PackedValueInt(this Color color)
        {
            int packedValue = 0;

            packedValue |= color.R;
            packedValue <<= 8;

            packedValue |= color.G;
            packedValue <<= 8;

            packedValue |= color.B;
            packedValue <<= 8;

            packedValue |= color.A;

            return packedValue;
        }

        public static Color Color(this int packedValue)
        {
            int a = packedValue & 0xFF;
            packedValue >>= 8;

            int b = packedValue & 0xFF;
            packedValue >>= 8;

            int g = packedValue & 0xFF;
            packedValue >>= 8;

            int r = packedValue & 0xFF;
            packedValue >>= 8;

            return new Color(r, g, b, a);
        }
    }
}
