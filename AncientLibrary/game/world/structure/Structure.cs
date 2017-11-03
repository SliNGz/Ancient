using ancient.game.utils;
using ancient.game.world;
using ancient.game.world.block;
using ancient.game.world.chunk;
using ancient.game.world.generator.metadata;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.structure
{
    public class Structure
    {
        protected Dictionary<Tuple<int, int, int>, Block> blocks;

        protected int width;
        protected int height;
        protected int length;

        public Structure(string path)
        {
            Color[,,] voxels = MagicaVoxelImporter.FromMagica(new BinaryReader(File.OpenRead(Structures.basePath + path + ".vox")));

            this.width = voxels.GetLength(0);
            this.height = voxels.GetLength(1);
            this.length = voxels.GetLength(2);

            blocks = new Dictionary<Tuple<int, int, int>, Block>();

            for (int x = 0; x < this.width; x++)
            {
                for (int y = 0; y < this.height; y++)
                {
                    for (int z = 0; z < this.length; z++)
                    {
                        Color color = voxels[x, y, z];
                        int id = color.R * 256 * 256 + color.G * 256 + color.B;
                        Block block = Blocks.GetBlockFromID(id);

                        if (block == null)
                            throw new NullReferenceException("Could not find block with id: " + id);

                        if (block != Blocks.air)
                            blocks.Add(new Tuple<int, int, int>(x, y, z), block);
                    }
                }
            }
        }

        public virtual void Generate(World world, int x, int y, int z)
        {
            foreach (KeyValuePair<Tuple<int, int, int>, Block> pair in blocks)
                world.SetBlockDirectly(pair.Value, x + pair.Key.Item1, y + pair.Key.Item2, z + pair.Key.Item3);
        }
    }
}
