using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.world;

namespace ancientlib.game.particle
{
    public class ParticleSnow : Particle
    {
        public ParticleSnow(World world) : base(world)
        {
            SetVelocity(new Vector3(0, -0.7F, 0));
            SetRotationVelocity(Vector3.One * 1.5F);
            this.scale = Vector3.One * 0.025F;
            this.lifeSpan = 832;
            this.modelNum = world.rand.Next(4);
        }

        public override string GetModelName()
        {
            return "particle_snow_" + modelNum;
        }
    }
}
