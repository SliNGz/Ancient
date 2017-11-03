using ancient.game.world;
using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.world.structure
{
    class StructureCloud : Structure
    {
        public StructureCloud(string path) : base(path)
        { }

        public override void Generate(World world, int x, int y, int z)
        {
            foreach (KeyValuePair<Tuple<int, int, int>, Block> pair in blocks)
            {
                int wx = x + pair.Key.Item1;
                int wy = y + pair.Key.Item2;
                int wz = z + pair.Key.Item3;

                wx -= this.width / 2;
        //        wy -= this.height / 2;
                wz -= this.length / 2;

                Block block = world.GetBlock(wx, wy, wz);
                if (block == null || block != null && block is BlockAir)
                    world.SetBlockDirectly(pair.Value, wx, wy, wz);
            }
        }
    }
}
