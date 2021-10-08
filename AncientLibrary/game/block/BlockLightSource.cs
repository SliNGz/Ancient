using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block.type;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancient.game.world.chunk;

namespace ancientlib.game.block
{
    public abstract class BlockLightSource : Block
    {
        public BlockLightSource(string name, BlockType type) : base(name, type)
        { }

        public override void OnPlace(World world, int x, int y, int z)
        {
            base.OnPlace(world, x, y, z);
           // world.lightingManager.AddLightSource(x, y, z, 15);
        }

        public override void OnDestroy(World world, int x, int y, int z)
        {
            base.OnDestroy(world, x, y, z);
        }
    }
}
