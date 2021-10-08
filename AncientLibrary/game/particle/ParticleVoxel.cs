using ancient.game.world;
using ancientlib.game.init;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.particle
{
    public class ParticleVoxel : Particle
    {
        public ParticleVoxel(World world) : base(world)
        {
            this.scale = new Vector3(0.2F, 0.2F, 0.2F);
            this.color = Blocks.dirt.GetColor();
            this.startColor = color;
            this.endColor = Color.Black;
            this.lifeSpan = 192;
        }
    }
}
