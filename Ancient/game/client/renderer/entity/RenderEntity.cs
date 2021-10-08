using ancient.game.client.renderer.model;
using ancient.game.entity;
using ancient.game.renderers.model;
using ancient.game.renderers.voxel;
using ancient.game.renderers.world;
using ancient.game.utils;
using ancientlib.game.entity;
using ancientlib.game.entity.model;
using ancientlib.game.entity.monster;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.entity
{
    public class RenderEntity
    {
        protected void Draw(Entity entity, ModelData model, Vector3 position, float yaw, float pitch, float roll, float xScale, float yScale, float zScale)
        {
            // SetMultiplyColor(GetColorAffectedByLight(entity, entity.GetMultiplyColor()));
            SetMultiplyColor(entity.GetMultiplyColor());

            VoxelRenderer.Draw(model.GetVoxelRendererData(), position, GetRotationCenter(model) + model.GetOffset() / 2F, yaw, pitch, roll, xScale, yScale, zScale);
        }

        protected virtual void Draw(Entity entity, ModelData model, Vector3 scale)
        {
            Draw(entity, model, entity.GetPosition(), entity.GetYaw(), entity.GetPitch(), entity.GetRoll(), scale.X, scale.Y, scale.Z);
        }

        public void Draw(Entity entity)
        {
            ModelData model = GetModelOfEntity(entity);

            if (model == null)
                return;

            Draw(entity, model, entity.GetModelScale());
        }

        private ModelData GetModelOfEntity(Entity entity)
        {
            string modelName = entity.GetModelName();

            if (entity.GetModel() is EntityModelAnimation)
                modelName += "_" + entity.animationIndex;

            ModelData model = ModelDatabase.GetModelFromName(modelName);

            if (model == null)
                Console.WriteLine("Model (" + modelName + ") is null: " + entity);

            return model;
        }

        protected void SetMultiplyColor(Color color)
        {
            WorldRenderer.currentEffect.Parameters["MultiplyColor"].SetValue(color.ToVector4());
        }

        protected Color GetColorAffectedByLight(Entity entity, Color color)
        {
            if (!Ancient.ancient.guiManager.GetCurrentGui().ShouldDrawWorldBehind())
                return color;

            Vector3 center = new Vector3(entity.GetX() + entity.GetWidth() / 2F, entity.GetY() - entity.GetHeight() / 2F, entity.GetZ() + entity.GetLength() / 2F);
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
