using ancient.game.camera;
using ancient.game.client.renderer.model;
using ancient.game.client.world;
using ancient.game.entity.player;
using ancient.game.renderers.model;
using ancient.game.renderers.voxel;
using ancient.game.renderers.world;
using ancientlib.game.init;
using ancientlib.game.item;
using ancientlib.game.item.weapon;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.item
{
    class ItemRenderer
    {
        public ItemRenderer()
        { }

        public void Draw()
        {
            DrawItemInHand();
        }

        private void DrawItemInHand()
        {
            EntityPlayer player = Ancient.ancient.player;

            /* instead of Vector3.Forward - renders the item relative to the camera based on the new vector.
               lookAt.Z - how far is the object from the camera - MUST BE LARGER THAN NEARPLANE OF CAMERA - so it wont clip through the camera,
               MUST BE SMALLER THAN PLAYER WIDTH OR LENGTH - so it wont clip through other models*/

            WorldRenderer.effect.Parameters["MultiplyColorEnabled"].SetValue(true);

            if (WorldRenderer.camera.distance > 0 || player.GetItemInHand() == null)
                return;

            Item item = player.GetItemInHand().GetItem();

            if (item != null)
            {
                Vector3 lookAt = Vector3.Transform(item.GetModelOffset(), Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), player.GetHeadPitch(), 0));
                Vector3 position = lookAt * 0.25F;

                Draw(item, position, player.GetHeadYaw() + player.handYaw - MathHelper.PiOver2, player.handPitch, -player.GetHeadPitch() + player.handRoll, true);

                if (item is ItemTwoHandedDagger)
                {
                    lookAt = Vector3.Transform(item.GetModelOffset() * new Vector3(-1, 1, 1), Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), player.GetHeadPitch(), 0));
                    position = lookAt * 0.25F;
                    Draw(item, position, player.GetHeadYaw() + player.handYaw - MathHelper.PiOver2, player.handPitch, -player.GetHeadPitch() + player.handRoll, true);
                }
            }

            WorldRenderer.effect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }

        public static void Draw(Item item, Vector3 position, float yaw, float pitch, float roll, bool exactPosition = false)
        {
            Draw(item, position, yaw, pitch, roll, item.GetModelScale().X, item.GetModelScale().Y, item.GetModelScale().Z, exactPosition);
        }

        public static void Draw(Item item, Vector3 position, float yaw, float pitch, float roll, float xScale, float yScale, float zScale, bool exactPosition = false)
        {
            ModelData model = ModelDatabase.GetModelFromName(item.GetModelName());
            VoxelRendererData data = model.GetVoxelRendererData();

            EntityPlayer player = Ancient.ancient.player;
            Color color = Utils.GetColorAffectedByLight(Ancient.ancient.world, Color.White, player.GetX(), player.GetY(), player.GetZ());
            WorldRenderer.effect.Parameters["MultiplyColor"].SetValue(color.ToVector4());

            VoxelRenderer.Draw(data, position, GetRotationCenter(model), yaw, pitch, roll, xScale, yScale, zScale, 0, exactPosition);
        }

        private static Vector3 GetRotationCenter(ModelData modelData)
        {
            float x = -modelData.GetWidth() / 2F;
            float y = modelData.GetHeight() / 2F;
            float z = -modelData.GetLength() / 2F;

            return new Vector3(x, y, z);
        }
    }
}
