using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancient.game.world.block;
using ancientlib.game.init;

namespace ancientlib.game.world.structure
{
    public class StructureTree : Structure
    {
        public StructureTree(string path) : base(path)
        { }

        public override void Generate(World world, int x, int y, int z)
        {
            foreach (KeyValuePair<Tuple<int, int, int>, Block> pair in blocks)
            {
                int wx = x + pair.Key.Item1;
                int wy = y + pair.Key.Item2;
                int wz = z + pair.Key.Item3;
                world.SetBlockDirectly(pair.Value, wx - this.width / 2, wy, wz - this.length / 2);
            }
        }
    }
}
