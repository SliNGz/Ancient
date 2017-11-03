using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.utils
{
    public class MagicaVoxelImporter
    {
        private struct MagicaVoxelData
        {
            public byte x;
            public byte y;
            public byte z;
            public byte color;

            public MagicaVoxelData(BinaryReader stream, bool subsample)
            {
                x = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                y = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                z = (byte)(subsample ? stream.ReadByte() / 2 : stream.ReadByte());
                color = stream.ReadByte();
            }
        }

        public static Color[,,] FromMagica(BinaryReader stream)
        {
            Color[,,] data = null;
            Color[] colors = new Color[256];

            MagicaVoxelData[] voxelData = null;

            string magic = new string(stream.ReadChars(4));
            int version = stream.ReadInt32();

            if (magic == "VOX ")
            {
                int width = 0, length = 0, height = 0;
                bool subsample = false;

                while (stream.BaseStream.Position < stream.BaseStream.Length)
                {
                    char[] chunkId = stream.ReadChars(4);
                    int chunkSize = stream.ReadInt32();
                    int childChunks = stream.ReadInt32();
                    string chunkName = new string(chunkId);

                    if (chunkName == "SIZE")
                    {
                        width = stream.ReadInt32();
                        length = stream.ReadInt32();
                        height = stream.ReadInt32();

                        data = new Color[width, height, length];

                        stream.ReadBytes(chunkSize - 4 * 3);
                    }
                    else if (chunkName == "XYZI")
                    {
                        int numVoxels = stream.ReadInt32();
                        int div = (subsample ? 2 : 1);

                        voxelData = new MagicaVoxelData[numVoxels];
                        for (int i = 0; i < voxelData.Length; i++)
                            voxelData[i] = new MagicaVoxelData(stream, subsample);
                    }
                    else if (chunkName == "RGBA")
                    {
                        colors = new Color[256];

                        for (int i = 0; i < 256; i++)
                        {
                            byte r = stream.ReadByte();
                            byte g = stream.ReadByte();
                            byte b = stream.ReadByte();
                            byte a = stream.ReadByte();

                            colors[i] = new Color(r, g, b, a);
                        }
                    }
                    else stream.ReadBytes(chunkSize);
                }

                if (voxelData.Length == 0) return data;

                for (int i = 0; i < voxelData.Length; i++)
                {
                    if (voxelData[i].x > width - 1 || voxelData[i].y > length - 1 || voxelData[i].z > height - 1) continue;

                    int x = width - 1 - voxelData[i].x;
                    int y = voxelData[i].z;
                    int z = voxelData[i].y;

                    data[x, y, z] = colors[voxelData[i].color - 1];
                }
            }

            return data;
        }
    }
}
