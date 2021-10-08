using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;
using ancientlib.game.init;
using ancient.game.world.block;

namespace ancientlib.game.particle
{
    class ParticleFallingLeaf : Particle
    {
        public ParticleFallingLeaf(World world) : base(world)
        {
            this.scale = new Vector3(0.35F, 0.35F, 0.35F);
            this.color = Blocks.leaves.GetColor();
            this.startColor = color;
            this.endColor = color;
            this.lifeSpan = 1280;
            this.interactWithBlocks = false;
        }

        public ParticleFallingLeaf(World world, Block block) : this(world)
        {
            this.color = block.GetColor();
            this.startColor = color;
            this.endColor = color;
        }

        protected override void UpdateVelocity()
        {
            if (onGround)
                SetVelocity(0, 0, 0);
        }

        protected override bool IsBlockGround(Block block)
        {
            return !(block is BlockLeaves);
        }
    }
}
