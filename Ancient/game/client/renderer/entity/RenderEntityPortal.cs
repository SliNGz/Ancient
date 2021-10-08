using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancient.game.renderers.model;
using Microsoft.Xna.Framework;
using ancient.game.renderers.voxel;
using ancient.game.client.renderer.model;
using ancient.game.renderers.world;
using ancientlib.game.utils;

namespace ancient.game.client.renderer.entity
{
    public class RenderEntityPortal : RenderEntity
    {
        protected override void Draw(Entity entity, ModelData model, Vector3 scale)
        {
            base.Draw(entity, model, scale);

            ModelData inner = ModelDatabase.GetModelFromName("portal_inner");

            Vector3 offset = new Vector3(3, -3, 1);
            Color color = Color.Lerp(new Color(0, 255, 221), new Color(255, 238, 166), (float)Math.Sin(entity.GetTicksExisted() / 64F));
            //  color = Utils.HSVToRGB(entity.GetTicksExisted(), 1 , 1);
            //WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            WorldRenderer.currentEffect.Parameters["ShadowsEnabled"].SetValue(false);
            WorldRenderer.currentEffect.Parameters["MultiplyColor"].SetValue(color.ToVector4());
            VoxelRenderer.Draw(inner.GetVoxelRendererData(), entity.GetPosition(), GetRotationCenter(model) + offset, 0, 0, 0, scale.X, scale.Y, scale.Z);
            WorldRenderer.currentEffect.Parameters["ShadowsEnabled"].SetValue(true);
            //WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }
    }
}
