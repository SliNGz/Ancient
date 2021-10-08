using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block.type;
using ancient.game.world;
using ancientlib.game.init;
using Microsoft.Xna.Framework;

namespace ancientlib.game.block
{
    public abstract class BlockScenery : Block
    {
        public BlockScenery(string name, BlockType type) : base(name, type)
        { }

        public override bool IsSolid()
        {
            return false;
        }

        public override bool IsFullBlock()
        {
            return false;
        }

        public override bool CanBeDestroyed()
        {
            return true;
        }

        public override bool CanBePlaced(World world, int x, int y, int z)
        {
            Block ground = world.GetBlock(x, y - 1, z);
            return ground != null && (ground == Blocks.grass || ground == Blocks.dirt || ground == Blocks.snow);
        }

        protected override bool CanBlockStay(World world, int x, int y, int z)
        {
            return CanBePlaced(world, x, y, z);
        }

        public override void OnNeighborDestroyed(World world, int x, int y, int z, Block neighbor)
        {
            if (!CanBlockStay(world, x, y, z))
                world.DestroyBlock(x, y, z);
        }

        public override Vector3 GetItemModelScale()
        {
            return base.GetItemModelScale() / 8F;
        }
    }
}
