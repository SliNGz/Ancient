using ancient.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public class Utils
    {
        public static ObjectActivator CreateObjectFromType(Type type)
        {
            if (type == null)
            {
                throw new NullReferenceException("type");
            }

            ConstructorInfo emptyConstructor = type.GetConstructor(Type.EmptyTypes);
            var dynamicMethod = new DynamicMethod("CreateInstance", type, Type.EmptyTypes, true);
            ILGenerator ilGenerator = dynamicMethod.GetILGenerator();
            ilGenerator.Emit(OpCodes.Nop);
            ilGenerator.Emit(OpCodes.Newobj, emptyConstructor);
            ilGenerator.Emit(OpCodes.Ret);
            return (ObjectActivator)dynamicMethod.CreateDelegate(typeof(ObjectActivator));
        }

        public delegate object ObjectActivator();

        public static BoundingBox GetBlockBoundingBox(int x, int y, int z)
        {
            Vector3 min = new Vector3(x, y, z);
            Vector3 max = min + Vector3.One;

            return new BoundingBox(min, max);
        }

        public static Vector3 GetChunkPositionFromPosition(Vector3 position)
        {
            return new Vector3((int)Math.Floor(position.X / 16f), (int)Math.Floor(position.Y / 16f), (int)Math.Floor(position.Z / 16f));
        }

        public static BoundingBox GetBlockDownFace(int x, int y, int z)
        {
            BoundingBox boundingBox = GetBlockBoundingBox(x, y, z);
            return new BoundingBox(boundingBox.GetCorners()[3], boundingBox.GetCorners()[6]);
        }

        public static BoundingBox GetBlockUpFace(int x, int y, int z)
        {
            BoundingBox boundingBox = GetBlockBoundingBox(x, y, z);
            return new BoundingBox(boundingBox.GetCorners()[0], boundingBox.GetCorners()[5]);
        }

        public static BoundingBox GetBlockNorthFace(int x, int y, int z)
        {
            BoundingBox boundingBox = GetBlockBoundingBox(x, y, z);
            return new BoundingBox(boundingBox.GetCorners()[4], boundingBox.GetCorners()[6]);
        }

        public static BoundingBox GetBlockSouthFace(int x, int y, int z)
        {
            BoundingBox boundingBox = GetBlockBoundingBox(x, y, z);
            return new BoundingBox(boundingBox.GetCorners()[0], boundingBox.GetCorners()[2]);
        }

        public static BoundingBox GetBlockWestFace(int x, int y, int z)
        {
            BoundingBox boundingBox = GetBlockBoundingBox(x, y, z);
            return new BoundingBox(boundingBox.GetCorners()[7], boundingBox.GetCorners()[0]);
        }

        public static BoundingBox GetBlockEastFace(int x, int y, int z)
        {
            BoundingBox boundingBox = GetBlockBoundingBox(x, y, z);
            return new BoundingBox(boundingBox.GetCorners()[6], boundingBox.GetCorners()[1]);
        }

        public static int TicksInSecond
        {
            get { return 128; }
        }

        public static int TicksInMinute
        {
            get { return TicksInSecond * 60; }
        }

        public static int TicksInHour
        {
            get { return TicksInMinute * 60; }
        }

        public static int TicksInDay
        {
            get { return TicksInHour * 24; }
        }

        public static Color HSVToRGB(float hue, float saturation, float value)
        {
            int hi = (int)(Math.Floor(hue / 60)) % 6;
            float f = hue / 60 - (float)Math.Floor(hue / 60);

            value = value * 255;
            int v = (int)value;
            int p = (int)(value * (1 - saturation));
            int q = (int)(value * (1 - f * saturation));
            int t = (int)(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return new Color(v, t, p);
            else if (hi == 1)
                return new Color(q, v, p);
            else if (hi == 2)
                return new Color(p, v, t);
            else if (hi == 3)
                return new Color(p, q, v);
            else if (hi == 4)
                return new Color(t, p, v);
            else
                return new Color(v, p, q);
        }

        public static void RGBToHSV(Color color, out float h, out float s, out float v)
        {
            float r = color.R / 255F;
            float g = color.G / 255F;
            float b = color.B / 255F;

            float minVar, maxVar, delta;
            minVar = Math.Min(Math.Min(r, g), b);
            maxVar = Math.Max(Math.Max(r, g), b);
            v = maxVar;               // v
            delta = maxVar - minVar;
            if (maxVar != 0)
                s = delta / maxVar;       // s
            else
            {
                // r = g = b = 0		// s = 0, v is undefined
                s = 0;
                h = -1;
                return;
            }
            if (r == maxVar)
                h = (g - b) / delta;       // between yellow & magenta
            else if (g == maxVar)
                h = 2 + (b - r) / delta;   // between cyan & yellow
            else
                h = 4 + (r - g) / delta;   // between magenta & cyan
            h *= 60;               // degrees
            if (h < 0)
                h += 360;
        }

        public static Color GetColorAffectedByLight(World world, Color color, double x, double y, double z)
        {
            int light = GetLightValueAt(world, x, y, z);

            float lerp = 1 - (float)Math.Pow(0.8, 15 - light);
            Color affectedColor = Color.Lerp(color, Color.Black, lerp);
            affectedColor.A = color.A;

            return affectedColor;
        }

        public static Color GetColorAffectedByLight(Color color, int light)
        {
            float lerp = 1 - (float)Math.Pow(0.8, 15 - light);
            Color affectedColor = Color.Lerp(color, Color.Black, lerp);
            affectedColor.A = color.A;

            return affectedColor;
        }

        public static int GetLightValueAt(World world, double x, double y, double z)
        {
            int x1 = (int)Math.Round(x);
            int y1 = (int)Math.Round(y);
            int z1 = (int)Math.Round(z);

            int blocklight = world.GetBlocklight(x1, y1, z1);
            int sunlight = world.GetSunlight(x1, y1, z1);
            int light = Math.Max(blocklight, sunlight);

            return light;
        }
    }
}
