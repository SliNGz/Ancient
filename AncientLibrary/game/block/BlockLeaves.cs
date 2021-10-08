using ancient.game.world.block.type;
using ancientlib.game.particle;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.block
{
    public class BlockLeaves : Block
    {
        public BlockLeaves() : base("Leaves", BlockType.leaves)
        {
            this.color = new Color(4, 60, 0);
            this.secondaryColor = new Color(20, 43, 25);
            this.tickRate = 128;
        }

        public override void OnTick(World world, int x, int y, int z)
        {
            base.OnTick(world, x, y, z);

            if (world.rand.Next(1024) != 0)
                return;

            ParticleFallingLeaf voxel = new ParticleFallingLeaf(world, this);
            voxel.SetPosition(x, y, z);
            voxel.SetScale(Vector3.One * world.rand.Next(0.2F, 0.4F));
            voxel.SetPitchVelocity((float)world.rand.NextDouble());
            voxel.SetYawVelocity((float)world.rand.NextDouble());
            voxel.SetVelocity(new Vector3(0.5F, 0, 0.5F));
            voxel.gravity = world.rand.Next(0.07F, 0.15F);
            world.SpawnParticle(voxel);
        }

        public override Color GetColorAt(World world, int x, int y, int z)
        {
            float noise = (float)(world.GetSimplexNoise().Evaluate(x / 32.0, y / 2.0, z / 32.0));
            noise = (noise + 1) / 2;

            return Color.Lerp(color, secondaryColor, noise);
        }
    }
}
