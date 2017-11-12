using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.entity;
using ancient.game.renderers.model;
using ancientlib.game.entity.player;
using Microsoft.Xna.Framework;
using ancient.game.client.renderer.model;
using ancientlib.game.utils;
using ancient.game.renderers.world;
using ancient.game.renderers.voxel;

namespace ancient.game.client.renderer.entity
{
    class RenderEntityPlayerBase : RenderEntity
    {
        public RenderEntityPlayerBase()
        { }

        protected override void Draw(Entity entity, ModelData model)
        {
            EntityPlayerBase player = (EntityPlayerBase)entity;
            base.Draw(player, model);
            DrawHair(player, model);
            DrawEyes(player, model);
        }

        private void DrawHair(EntityPlayerBase player, ModelData model)
        {
            ModelData hair = ModelDatabase.GetModelFromName(player.GetHairModelName());
            Vector3 scale = player.GetModelScale();

            int modelLength = model.GetLength();

            SetMultiplyColor(GetColorAffectedByLight(player, player.GetHairColor()));

            Vector3 rotationCenter = GetRotationCenter(hair) + new Vector3(0, 2, (hair.GetLength() - modelLength) / 2 - 1) + model.GetOffset();
            VoxelRenderer.Draw(hair.GetVoxelRendererData(), player.GetPosition(), rotationCenter, player.GetHeadYaw(), player.GetPitch(), player.GetRoll(), scale.X, scale.Y, scale.Z);
        }

        private void DrawEyes(EntityPlayerBase player, ModelData model)
        {
            Vector3 scale = player.GetModelScale();
            ModelData eyes = ModelDatabase.GetModelFromName(player.GetEyesModelName());

            SetMultiplyColor(GetColorAffectedByLight(player, Color.White));

            Vector3 eyesRotCenter = GetRotationCenter(eyes) + player.GetEyesOffset() + eyes.GetOffset() + model.GetOffset() / 2F;
            VoxelRenderer.Draw(eyes.GetVoxelRendererData(), player.GetPosition(), eyesRotCenter, player.GetHeadYaw(), player.GetPitch(), player.GetRoll(), scale.X, scale.Y, scale.Z);

            if (eyes.GetAttachments().Count > 0)
            {
                ModelData pupils = eyes.GetAttachments()[0];

                SetMultiplyColor(GetColorAffectedByLight(player, player.GetEyesColor()));

                Vector3 pupilsRotCenter = GetRotationCenter(pupils) + player.GetEyesOffset() + pupils.GetOffset() + model.GetOffset() / 2F;
                VoxelRenderer.Draw(pupils.GetVoxelRendererData(), player.GetPosition(), pupilsRotCenter, player.GetHeadYaw(), player.GetPitch(), player.GetRoll(), scale.X, scale.Y, scale.Z);
            }
        }
    }
}
