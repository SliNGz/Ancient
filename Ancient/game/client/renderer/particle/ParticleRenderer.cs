using ancient.game.client.particle;
using ancient.game.client.renderer.model;
using ancient.game.client.world;
using ancient.game.renderers.model;
using ancient.game.renderers.voxel;
using ancient.game.renderers.world;
using ancientlib.game.particle;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.particle
{
    public class ParticleRenderer
    {
        private WorldClient world;

        public ParticleRenderer(WorldClient world)
        {
            this.world = world;
        }

        public void Draw()
        {
            WorldRenderer.effect.Parameters["MultiplyColorEnabled"].SetValue(true);

            List<Particle> particles = world.GetParticleManager().GetParticleList();

            for (int i = 0; i < particles.Count; i++)
            {
                Particle particle = particles[i];

                ModelData model = ModelDatabase.GetModelFromName(particle.GetModelName());
                Vector3 size = new Vector3(model.GetWidth(), model.GetHeight(), model.GetLength());
                Vector3 scale = particle.GetScale();

                Vector3 rotationCenter = new Vector3(size.X / -2F, size.Y / 2F, size.Z / -2F);

                WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(particle.GetColor().ToVector4());
                VoxelRenderer.Draw(model.GetVoxelRendererData(), particle.GetPosition(), rotationCenter, particle.GetYaw(), particle.GetPitch(), particle.GetRoll(), scale.X, scale.Y, scale.Z);
            }

            WorldRenderer.effect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }
    }
}
