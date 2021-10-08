using ancient.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.particle
{
    public class ParticleFlame : Particle
    {
        public static Color[] COLORS = new Color[] { Color.Red, Color.OrangeRed, Color.Orange, Color.Goldenrod };

        public ParticleFlame(World world) : base(world)
        {
            SetVelocity(new Vector3(0, 0.1F, 0));
            this.yawVelocity = 1.5F;
            this.pitchVelocity = 1.5F;
            this.color = COLORS[world.rand.Next(COLORS.Length)];
            this.startColor = color;
            this.endColor = Color.Black;
            this.lifeSpan = 512;
            this.interactWithBlocks = false;
        }
    }
}
