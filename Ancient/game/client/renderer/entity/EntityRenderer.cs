using ancient.game.client.particle;
using ancient.game.client.renderer.model;
using ancient.game.client.world;
using ancient.game.entity;
using ancient.game.entity.player;
using ancient.game.renderers.model;
using ancient.game.renderers.voxel;
using ancient.game.renderers.world;
using ancient.game.utils;
using ancientlib.game.entity;
using ancientlib.game.entity.player;
using ancientlib.game.entity.projectile;
using ancientlib.game.entity.skill;
using ancientlib.game.entity.world;
using ancientlib.game.utils;
using ancientlib.game.world.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.entity
{
    public class EntityRenderer
    {
        private static bool drawBoundingBoxes = false;

        private WorldClient world;

        public EntityRenderer(WorldClient world)
        {
            this.world = world;
        }

        public void Draw()
        {
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (drawBoundingBoxes)
                {
                    WorldRenderer.currentEffect.Parameters["MultiplyColor"].SetValue(Color.White.ToVector4());
                    RenderUtils.DrawBoundingBox(entity.GetBoundingBox(), Color.Red);
                }

                if (entity is EntityPlayer)
                {
                    if (entity == Ancient.ancient.player && WorldRenderer.camera.GetDistance() == 0)
                        continue;
                }

                EntityRenderers.GetRenderEntityFromEntity(entity).Draw(entity);
            }
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }
    }
}
