using ancientlib.game.particle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.particle
{
    public class ParticleManager
    {
        private List<Particle> particles;

        public ParticleManager()
        {
            this.particles = new List<Particle>();
        }

        public void Update()
        {
            for (int i = 0; i < particles.Count; i++)
                particles[i].Update(Ancient.ancient.gameTime);
        }

        public void AddParticle(Particle particle)
        {
            this.particles.Add(particle);
        }

        public void RemoveParticle(Particle particle)
        {
            this.particles.Remove(particle);
        }

        public List<Particle> GetParticleList()
        {
            return this.particles;
        }
    }
}
