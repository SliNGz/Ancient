using ancient.game.camera;
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
        private WorldClient world;

        private bool drawBoundingBoxes = false;

        private static ModelData healthBar;

        public EntityRenderer(WorldClient world)
        {
            this.world = world;

            healthBar = ModelDatabase.GetModelFromName("voxel");
        }

        public void Draw()
        {
            DrawEntities();
        }

        private void DrawEntities()
        {
            for (int i = 0; i < world.entityList.Count; i++)
            {
                Entity entity = world.entityList[i];

                if (drawBoundingBoxes)
                    RenderUtils.DrawBoundingBox(entity.GetBoundingBox(), Color.Red);

                if (entity is EntityPlayer)
                {
                    if (entity == Ancient.ancient.player && WorldRenderer.camera.GetDistance() == 0)
                        continue;
                }

                Draw(entity);
            }
        }

        public static void Draw(Entity entity, bool applyLighting = true)
        {
            WorldRenderer.effect.Parameters["MultiplyColorEnabled"].SetValue(true);
            double x = entity.GetX() - entity.GetWidth() / 2F;
            double y = entity.GetY() - entity.GetHeight() / 2F;
            double z = entity.GetZ() - entity.GetLength() / 2F;
            Color multiplyColor = entity.GetMultiplyColor();

            if (entity is EntityPlayerBase)
                multiplyColor = ((EntityPlayerBase)entity).GetSkinColor();

            if (applyLighting && entity.IsAffectedByLight())
                multiplyColor = Utils.GetColorAffectedByLight(entity.GetWorld(), multiplyColor, x, y, z);

            WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(multiplyColor.ToVector4());

            ModelData model = ModelDatabase.GetModelFromName(entity.GetModelName());
            VoxelRendererData voxelData = model.GetVoxelRendererData();
            Vector3 position = entity.GetPosition();
            Vector3 scale = entity.GetModelScale();

            if (entity is EntityDrop)
            {
                position = ((EntityDrop)entity).GetAnimationPosition();
                position.Y += model.GetHeight() * scale.Y - entity.GetHeight();
            }

            VoxelRenderer.Draw(voxelData, position, GetRotationCenter(model), entity.GetYaw(), entity.GetPitch(), entity.GetRoll(), scale.X, scale.Y, scale.Z);

            if (entity is EntityLiving)
            {
                EntityLiving living = ((EntityLiving)entity);

                if (living.RenderHealthBar())
                    DrawHealthBar(living);

                if (entity is EntityPlayerBase)
                {
                    EntityPlayerBase player = ((EntityPlayerBase)entity);
                    DrawHair(player, applyLighting);
                    DrawEyes(player, applyLighting);
                }
            }

            WorldRenderer.effect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }

        private static void DrawHealthBar(EntityLiving entity)
        {
            float healthPercentage = (entity.GetHealth() / (float)entity.GetMaxHealth());
            float width = healthPercentage * 1.5F;

            Color color = Color.Lerp(Color.Green, Color.Red, 1 - healthPercentage);
            color = Utils.GetColorAffectedByLight(entity.GetWorld(), color, entity.GetX() - entity.GetWidth() / 2F, entity.GetEyePosition().Y, entity.GetZ() - entity.GetLength() / 2F);

            WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(color.ToVector3());
            VoxelRenderer.Draw(healthBar.GetVoxelRendererData(), entity.GetPosition() + new Vector3(0, 0.5F, 0), GetRotationCenter(healthBar), entity.GetYaw(), 0, 0, width, 0.15F, 0.15F);
        }

        private static void DrawHair(EntityPlayerBase player, bool applyLighting = true)
        {
            ModelData hair = ModelDatabase.GetModelFromName(player.GetHairModelName());
            Vector3 scale = player.GetModelScale();

            double x = player.GetX() - player.GetWidth() / 2F;
            double y = player.GetY() - player.GetHeight() / 2F;
            double z = player.GetZ() - player.GetLength() / 2F;
            Color color = player.GetHairColor();

            if (applyLighting)
                color = Utils.GetColorAffectedByLight(player.GetWorld(), color, x, y, z);

            ModelData model = ModelDatabase.GetModelFromName(player.GetModelName());
            int modelLength = model.GetLength();

            WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(color.ToVector4());
            VoxelRenderer.Draw(hair.GetVoxelRendererData(), player.GetPosition(), GetRotationCenter(hair) + new Vector3(0, 2, (hair.GetLength() - modelLength) / 2 - 1), player.GetHeadYaw(), player.GetPitch(), player.GetRoll(), scale.X, scale.Y, scale.Z);
        }

        private static void DrawEyes(EntityPlayerBase player, bool applyLighting = true)
        {
            Vector3 scale = player.GetModelScale();

            double x = player.GetX() - player.GetWidth() / 2F;
            double y = player.GetY() - player.GetHeight() / 2F;
            double z = player.GetZ() - player.GetLength() / 2F;

            ModelData eyes = ModelDatabase.GetModelFromName(player.GetEyesModelName());

            Color eyesColor = Color.White;

            if (applyLighting)
                eyesColor = Utils.GetColorAffectedByLight(player.GetWorld(), eyesColor, x, y, z);

            WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(eyesColor.ToVector4());

            VoxelRenderer.Draw(eyes.GetVoxelRendererData(), player.GetPosition(), GetRotationCenter(eyes) + player.GetEyesOffset() + eyes.GetOffset(), player.GetHeadYaw(), player.GetPitch(), player.GetRoll(), scale.X, scale.Y, scale.Z);

            if (eyes.GetAttachments().Count > 0)
            {
                ModelData pupils = eyes.GetAttachments()[0];
                Color pupilsColor = player.GetEyesColor();

                if (applyLighting)
                    pupilsColor = Utils.GetColorAffectedByLight(player.GetWorld(), pupilsColor, x, y, z);

                WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(pupilsColor.ToVector4());

                VoxelRenderer.Draw(pupils.GetVoxelRendererData(), player.GetPosition(), GetRotationCenter(pupils) + player.GetEyesOffset() + pupils.GetOffset(), player.GetHeadYaw(), player.GetPitch(), player.GetRoll(), scale.X, scale.Y, scale.Z);
            }
        }

        private static Vector3 GetRotationCenter(ModelData model)
        {
            float x = -model.GetWidth() / 2F;
            float z = -model.GetLength() / 2F;

            return new Vector3(x, 0, z);
        }
    }
}
