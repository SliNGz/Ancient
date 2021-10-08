using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagicaVoxelContentExtension
{
    public class MagicaVoxelModelData
    {
        public Color[] voxels;

        public int width;
        public int height;
        public int length;

        public Vector3 scale;
        public Vector3 offset;

        public float alpha;

        public MagicaVoxelModelData()
        { }

        public MagicaVoxelModelData(Color[,,] colors, Vector3 scale, Vector3 offset, float alpha)
        {
            this.width = colors.GetLength(0);
            this.height = colors.GetLength(1);
            this.length = colors.GetLength(2);
            this.voxels = GetFlatArray(colors);
            this.scale = scale;
            this.offset = offset;
            this.alpha = alpha;
        }

        private Color[] GetFlatArray(Color[,,] colors)
        {
            Color[] flatArray = new Color[width * height * length];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        flatArray[x + y * width + z * width * height] = colors[x, y, z];
                    }
                }
            }

            return flatArray;
        }

        public Color[,,] GetVoxels()
        {
            Color[,,] colors = new Color[width, height, length];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        colors[x, y, z] = this.voxels[x + y * width + z * width * height];
                    }
                }
            }

            return colors;
        }
    }
}
