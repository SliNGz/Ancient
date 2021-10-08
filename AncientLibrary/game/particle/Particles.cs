using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.particle
{
    class Particles
    {
        private static Dictionary<int, Type> particles = new Dictionary<int, Type>();

        public static void Initialize()
        {
            AddParticle(0, typeof(ParticleFlame));
            AddParticle(1, typeof(ParticleSnow));
            AddParticle(2, typeof(ParticleFallingLeaf));
        }

        private static void AddParticle(int id, Type type)
        {
            particles.Add(id, type);
        }

        public static Particle CreateParticleFromID(int id, params object[] paramArray)
        {
            Type type = null;

            if (particles.TryGetValue(id, out type))
                return (Particle)Activator.CreateInstance(type, paramArray);

            return null;
        }
    }
}
