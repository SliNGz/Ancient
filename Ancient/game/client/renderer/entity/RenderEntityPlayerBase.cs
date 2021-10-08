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
    public class RenderEntityPlayerBase : RenderEntityLiving
    {
        protected override void Draw(Entity entity, ModelData model, Vector3 scale)
        {
            EntityPlayerBase player = (EntityPlayerBase)entity;
            DrawBody(player, model, scale);
            // ModelData head = ModelDatabase.GetModelFromName(player.GetModelCollection().GetStandingModel().GetModelName() + "_head");
            //  DrawHead(player, model, head);
            //  DrawHair(player, model);
            //  DrawEyes(player, model);
        }

        private void DrawBody(EntityPlayerBase player, ModelData body, Vector3 scale)
        {
            /*    SetMultiplyColor(GetColorAffectedByLight(player, player.GetMultiplyColor()));

                standing = ModelDatabase.GetModelFromName(player.GetModelCollection().GetStandingModel().GetModelName());

                Vector3 scale = player.GetModelScale();
                float yDiff = (standing.GetHeight() - model.GetHeight()) / -2F;
                Vector3 position = player.GetEyePosition() + ((model.GetHeight() / -2F) + 1 + yDiff) * Vector3.UnitY * scale;
                Vector3 rotationCenter = GetRotationCenter(model) + model.GetOffset();

                VoxelRenderer.Draw(model.GetVoxelRendererData(), position, rotationCenter, player.GetYaw(), player.GetPitch(), 0, scale.X, scale.Y, scale.Z);*/

            //  SetMultiplyColor(GetColorAffectedByLight(player, player.GetMultiplyColor()));
            SetMultiplyColor(player.GetMultiplyColor());

            ModelData head = ModelDatabase.GetModelFromName("human_head");
            ModelData hand = ModelDatabase.GetModelFromName("human_hand");
            ModelData leg = ModelDatabase.GetModelFromName("human_leg");

         //   Vector3 scale = player.GetModelScale();
            Vector3 playerPosition = player.GetPosition();
            Vector3 position = playerPosition + player.GetEyesOffset() * player.GetModelScale() * Vector3.UnitY + ((-body.GetHeight() + leg.GetHeight()) * Vector3.UnitY * scale);

            float handPitch = player.handPitch;

            // Body
            VoxelRenderer.Draw(body.GetVoxelRendererData(), position, GetRotationCenter(body), player.GetYaw(), player.GetPitch(), 0, scale.X, scale.Y, scale.Z);

            // Hands
            if (ShouldDrawHands(player))
            {
                Vector3 handOffset = new Vector3((body.GetWidth() + hand.GetWidth()) / 2F, 0, 0);

                VoxelRenderer.Draw(hand.GetVoxelRendererData(), position, GetRotationCenter(hand) + handOffset, player.GetYaw(), handPitch, 0, scale.X, scale.Y, scale.Z);
                VoxelRenderer.Draw(hand.GetVoxelRendererData(), position, GetRotationCenter(hand) + handOffset * Vector3.Left, player.GetYaw(), MathHelper.TwoPi - handPitch, 0, scale.X, scale.Y, scale.Z);
            }

            if (ShouldDrawLegs(player))
            {
                float legPitch = handPitch / 3F;

                // Legs
                Vector3 legOffset = new Vector3(3, -body.GetHeight(), 0);

                VoxelRenderer.Draw(leg.GetVoxelRendererData(), position, GetRotationCenter(leg) + legOffset, player.GetYaw(), MathHelper.TwoPi - legPitch, 0, scale.X, scale.Y, scale.Z);
                VoxelRenderer.Draw(leg.GetVoxelRendererData(), position, GetRotationCenter(leg) + legOffset * new Vector3(-1, 1, 1), player.GetYaw(), legPitch, 0, scale.X, scale.Y, scale.Z);
            }

            // Head
            Vector3 headPosition = playerPosition - head.GetHeight() * scale.Y * Vector3.Up;
            Vector3 headRotationCenter = GetRotationCenter(head) + body.GetOffset() / 2F + head.GetHeight() * Vector3.Up;
            float headPitch = MathHelper.Clamp(player.GetHeadPitch(), -MathHelper.Pi / 2.5F, MathHelper.Pi / 2.5F);
            VoxelRenderer.Draw(head.GetVoxelRendererData(), headPosition, headRotationCenter, player.GetHeadYaw(), headPitch, 0, scale.X, scale.Y, scale.Z);

            // Hair
            //  SetMultiplyColor(GetColorAffectedByLight(player, player.GetHairColor()));
            SetMultiplyColor(player.GetHairColor());

            ModelData hair = ModelDatabase.GetModelFromName(player.GetHairModelName());
            Vector3 hairRotationCenter = new Vector3(0, head.GetHeight() + 1, (int)(((hair.GetLength() - head.GetLength()) - 2) / 2F));
            VoxelRenderer.Draw(hair.GetVoxelRendererData(), headPosition, GetRotationCenter(hair) + hairRotationCenter, player.GetHeadYaw(), headPitch, 0, scale.X, scale.Y, scale.Z);

            // Eyes
            // SetMultiplyColor(GetColorAffectedByLight(player, Color.White));
            SetMultiplyColor(Color.White);

            ModelData eyes = ModelDatabase.GetModelFromName(player.GetEyesModelName());
            Vector3 eyesRotationCenter = new Vector3(0, (float)Math.Round(head.GetHeight() / 2F), head.GetLength() / -2F - 0.01F);
            VoxelRenderer.Draw(eyes.GetVoxelRendererData(), headPosition, GetRotationCenter(eyes) * new Vector3(1, 0, 0) + eyesRotationCenter + eyes.GetOffset(), player.GetHeadYaw(), headPitch, 0, scale.X, scale.Y, scale.Z);

            if (eyes.GetAttachments().Count > 0)
            {
                ModelData pupils = eyes.GetAttachments()[0];

                //  SetMultiplyColor(GetColorAffectedByLight(player, player.GetEyesColor()));
                SetMultiplyColor(player.GetEyesColor());
                VoxelRenderer.Draw(pupils.GetVoxelRendererData(), headPosition, GetRotationCenter(pupils) * new Vector3(1, 0, 0) + eyesRotationCenter + pupils.GetOffset(), player.GetHeadYaw(), headPitch, 0, scale.X, scale.Y, scale.Z);
            }
        }

        private bool ShouldDrawHands(EntityPlayerBase player)
        {
            return player.GetModel() != player.GetModelCollection().GetSittingModel();
        }

        private bool ShouldDrawLegs(EntityPlayerBase player)
        {
            return player.GetModel() != player.GetModelCollection().GetSittingModel();
        }

        private void DrawHead(EntityPlayerBase player, ModelData model, ModelData head)
        {
            /*   Vector3 scale = player.GetModelScale();

               Vector3 diff = (model.GetDimensions() - standing.GetDimensions()) * new Vector3(1, 1, 1);

               if (model.GetName() == player.GetModelCollection().GetSittingModel().GetModelName())
                   diff.Y = 0;

               neckPosition = player.GetPosition() - head.GetHeight() * scale.Y * Vector3.Up + diff / 2F * scale;
               neckRC = GetRotationCenter(head) + model.GetOffset() / 2F + head.GetHeight() * Vector3.Up;
               headPitch = MathHelper.Clamp(player.GetHeadPitch(), -MathHelper.Pi / 3F, MathHelper.Pi / 3F);

               VoxelRenderer.Draw(head.GetVoxelRendererData(), neckPosition, neckRC, player.GetHeadYaw(), headPitch, 0, scale.X, scale.Y, scale.Z);*/
        }

        private void DrawHair(EntityPlayerBase player, ModelData model)
        {
            /*ModelData hair = ModelDatabase.GetModelFromName(player.GetHairModelName());
            Vector3 scale = player.GetModelScale();

            int modelLength = model.GetLength();

            SetMultiplyColor(GetColorAffectedByLight(player, player.GetHairColor()));

            Vector3 diff = model.GetDimensions();// - standing.GetDimensions();
            diff.Y = 0;

            Vector3 rotationCenter = GetRotationCenter(hair) + new Vector3(0, 1, (hair.GetLength() - modelLength) / 2F - (head.GetLength() - model.GetLength())) + hair.GetOffset() - diff / 2F;
            VoxelRenderer.Draw(hair.GetVoxelRendererData(), neckPosition, rotationCenter + neckRC * Vector3.UnitY, player.GetHeadYaw(), headPitch, player.GetRoll(), scale.X, scale.Y, scale.Z);

            ModelData hair = ModelDatabase.GetModelFromName(player.GetHairModelName());
            Vector3 scale = player.GetModelScale();*/

        }

        private void DrawEyes(EntityPlayerBase player, ModelData model)
        {
            /*      Vector3 scale = player.GetModelScale();
                  ModelData eyes = ModelDatabase.GetModelFromName(player.GetEyesModelName());

                  Vector3 diff = model.GetDimensions();// - standing.GetDimensions();
                  diff.Y = 0;

                  SetMultiplyColor(GetColorAffectedByLight(player, Color.White));

                  Vector3 eyesRotCenter = GetRotationCenter(eyes) + player.GetEyesOffset() + eyes.GetOffset();
                  VoxelRenderer.Draw(eyes.GetVoxelRendererData(), neckPosition, eyesRotCenter + neckRC * Vector3.UnitY, player.GetHeadYaw(), headPitch, player.GetRoll(), scale.X, scale.Y, scale.Z);

                  if (eyes.GetAttachments().Count > 0)
                  {
                      ModelData pupils = eyes.GetAttachments()[0];

                      SetMultiplyColor(GetColorAffectedByLight(player, player.GetEyesColor()));

                      Vector3 pupilsRotCenter = GetRotationCenter(pupils) + player.GetEyesOffset() + pupils.GetOffset();
                      VoxelRenderer.Draw(pupils.GetVoxelRendererData(), neckPosition, pupilsRotCenter + neckRC * Vector3.UnitY, player.GetHeadYaw(), headPitch, player.GetRoll(), scale.X, scale.Y, scale.Z);
                  }*/
        }
    }
}
