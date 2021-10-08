using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block.type;
using Microsoft.Xna.Framework;
using ancient.game.world;

namespace ancientlib.game.block
{
    public class BlockSnowLayer : BlockScenery
    {
        public BlockSnowLayer() : base("Snow Layer", BlockType.snow)
        {
            this.type = BlockType.snow;
            this.dimensions.Y = 0F;
            this.color = Color.White;
            this.secondaryColor = Color.White;
        }

        public override Vector3 GetRenderDimensions(World world, int x, int y, int z)
        {
            return new Vector3(1, 0.1F, 1);
        }

        public override Vector3 GetItemModelScale()
        {
            return base.GetItemModelScale() * new Vector3(8, 1, 8);
        }
    }
}
