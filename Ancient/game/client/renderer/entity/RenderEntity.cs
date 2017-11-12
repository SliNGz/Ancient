using ancient.game.client.renderer.model;
using ancient.game.entity;
using ancient.game.renderers.model;
using ancient.game.renderers.voxel;
using ancient.game.renderers.world;
using ancient.game.utils;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.entity
{
    class RenderEntity
    {
        public RenderEntity()
        { }

        protected virtual void Draw(Entity entity, ModelData model)
        {
            WorldRenderer.effect.Parameters["MultiplyColorEnabled"].SetValue(true);
            SetMultiplyColor(GetColorAffectedByLight(entity, entity.GetMultiplyColor()));

            VoxelRendererData voxelData = model.GetVoxelRendererData();
            Vector3 position = entity.GetPosition();
            Vector3 scale = entity.GetModelScale();

            VoxelRenderer.Draw(voxelData, position, GetRotationCenter(model), entity.GetYaw(), entity.GetPitch(), entity.GetRoll(), scale.X, scale.Y, scale.Z);
        }

        public virtual void Draw(Entity entity)
        {
            Draw(entity, ModelDatabase.GetModelFromName(entity.GetModelState().GetModelName()));
        }

        protected void SetMultiplyColor(Color color)
        {
            WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(color.ToVector4());
        }

        protected Color GetColorAffectedByLight(Entity entity, Color color)
        {
            if (Ancient.ancient.guiManager.GetCurrentGui() != Ancient.ancient.guiManager.ingame)
                return color;

            Vector3 center = entity.GetBoundingBox().GetCenter();
            return Utils.GetColorAffectedByLight(entity.GetWorld(), color, center.X, center.Y, center.Z);
        }

        protected Vector3 GetRotationCenter(ModelData model)
        {
            float x = -model.GetWidth() / 2F;
            float z = -model.GetLength() / 2F;

            return new Vector3(x, 0, z);
        }
    }
}
