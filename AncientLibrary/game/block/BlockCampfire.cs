using ancient.game.world.block;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world.block.type;
using Microsoft.Xna.Framework;
using ancient.game.world;
using ancientlib.game.particle;

namespace ancientlib.game.block
{
    public class BlockCampfire : BlockLightSource, IBlockModel
    {
        public BlockCampfire() : base("Campfire", BlockType.wood)
        {
            this.lightEmission = 13;
            this.color = Color.Green;
            this.dimensions = new Vector3(1, 5 / 13F, 1);
            this.tickRate = 128;
        }

        public override Vector3 GetItemModelScale()
        {
            return base.GetItemModelScale() / 8F;
        }

        public override bool IsFullBlock()
        {
            return false;
        }

        public override void OnTick(World world, int x, int y, int z)
        {
            base.OnTick(world, x, y, z);

            ParticleFlame flame = new ParticleFlame(world);
            flame.SetPosition(new Vector3(x + (float)world.rand.NextDouble(), y + this.dimensions.Y + 0.4F, z + (float)world.rand.NextDouble()));
            flame.SetScale(Vector3.One * world.rand.Next(3, 13) / 100F);
            world.SpawnParticle(flame);
        }
    }
}
