using ancient.game.entity;
using ancient.game.entity.player;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public static class NetExtensions
    {
        public static void Write(this BinaryWriter writer, int x, int y, int z)
        {
            writer.Write(x);
            writer.Write(y);
            writer.Write(z);
        }

        public static void ReadPositionInt(this BinaryReader reader, out int x, out int y, out int z)
        {
            x = reader.ReadInt32();
            y = reader.ReadInt32();
            z = reader.ReadInt32();
        }

        public static void WriteColorRGB(this BinaryWriter writer, Color color)
        {
            writer.Write(color.R);
            writer.Write(color.G);
            writer.Write(color.B);
        }

        public static Color ReadColorRGB(this BinaryReader reader)
        {
            return new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public static void WriteColorRGBA(this BinaryWriter writer, Color color)
        {
            WriteColorRGB(writer, color);
            writer.Write(color.A);
        }

        public static Color ReadColorRGBA(this BinaryReader reader)
        {
            return new Color(reader.ReadByte(), reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
        }

        public static void Write(this BinaryWriter writer, Vector3 vector)
        {
            writer.Write(vector.X);
            writer.Write(vector.Y);
            writer.Write(vector.Z);
        }

        public static Vector3 ReadVector3(this BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }
    }
}
