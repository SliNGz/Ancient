using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancient.game.renderers.model;
using ancientlib.game.entity;
using Microsoft.Xna.Framework;
using ancient.game.renderers.world;
using ancient.game.renderers.voxel;
using ancient.game.client.renderer.model;

namespace ancient.game.client.renderer.entity
{
    public class RenderEntityLiving : RenderEntity
    {
        private static ModelData healthBar = ModelDatabase.GetModelFromName("voxel");

        protected override void Draw(Entity entity, ModelData model, Vector3 scale)
        {
            base.Draw(entity, model, scale);

            EntityLiving living = (EntityLiving)entity;
            if (living.RenderHealthBar())
                DrawHealthBar(living);
        }

        private void DrawHealthBar(EntityLiving entity)
        {
            float healthPercentage = (entity.GetHealth() / (float)entity.GetMaxHealth());
            float width = healthPercentage * 1.5F;

            Color color = Color.Lerp(Color.Green, Color.Red, 1 - healthPercentage);
            GetColorAffectedByLight(entity, color);

            WorldRenderer.currentEffect.Parameters["MultiplyColor"].SetValue(color.ToVector3());
            VoxelRenderer.Draw(healthBar.GetVoxelRendererData(), entity.GetPosition() + new Vector3(0, 0.5F, 0), GetRotationCenter(healthBar), Ancient.ancient.player.GetHeadYaw(), 0, 0, width, 0.15F, 0.15F);
        }
    }
}
