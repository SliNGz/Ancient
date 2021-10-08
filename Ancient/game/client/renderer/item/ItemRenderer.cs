using ancient.game.client.renderer.model;
using ancient.game.client.world;
using ancient.game.entity.player;
using ancient.game.renderers.model;
using ancient.game.renderers.voxel;
using ancient.game.renderers.world;
using ancient.game.utils;
using ancient.game.world.block;
using ancientlib.game.init;
using ancientlib.game.inventory;
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

            if (WorldRenderer.camera.GetDistance() != 0 || player.GetItemInHand() == null)
                return;

            Ancient.ancient.device.DepthStencilState = DepthStencilState.None;

            Item item = player.GetItemInHand().GetItem();

            if (item != null)
            {
                Vector3 lookAt = Vector3.Transform(item.GetHandOffset(), Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), player.GetHeadPitch(), 0));
                Vector3 position = lookAt * 0.25F;

                Draw(item, position, player.GetHeadYaw() + player.handRenderYaw - MathHelper.PiOver2, player.handRenderPitch, -player.GetHeadPitch() + player.handRenderRoll, true);

                if (item is ItemTwoHandedDagger)
                {
                    lookAt = Vector3.Transform(item.GetHandOffset() * new Vector3(-1, 1, 1), Matrix.CreateFromYawPitchRoll(player.GetHeadYaw(), player.GetHeadPitch(), 0));
                    position = lookAt * 0.25F;
                    Draw(item, position, player.GetHeadYaw() + player.handRenderYaw - MathHelper.PiOver2, player.handRenderPitch, -player.GetHeadPitch() + player.handRenderRoll, true);
                }
            }
        }

        public static void Draw(Item item, Vector3 position, float yaw, float pitch, float roll, bool exactPosition = false)
        {
            Draw(item, position, yaw, pitch, roll, item.GetHandScale().X, item.GetHandScale().Y, item.GetHandScale().Z, exactPosition);
        }

        public static void Draw(Item item, Vector3 position, float yaw, float pitch, float roll, float xScale, float yScale, float zScale, bool exactPosition = false)
        {
            ModelData model = ModelDatabase.GetModelFromName(item.GetModelName());

            Draw(model, position, GetRotationCenter(model), yaw, pitch, roll, xScale, yScale, zScale, exactPosition);
        }

        public static void Draw(ModelData model, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale, float yScale, float zScale, bool exactPosition = false)
        {
            VoxelRendererData data = model.GetVoxelRendererData();

            EntityPlayer player = Ancient.ancient.player;
            Color color = Utils.GetColorAffectedByLight(Ancient.ancient.world, Color.White, player.GetX(), player.GetY(), player.GetZ());
            WorldRenderer.currentEffect.Parameters["MultiplyColor"].SetValue(color.ToVector4());

            VoxelRenderer.Draw(data, position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, exactPosition);
        }

        public static void DrawToRenderTarget(RenderTarget2D renderTarget, Item item, float yaw, float pitch, float roll, float xScale, float yScale, float zScale)
        {
            Ancient.ancient.device.SetRenderTarget(renderTarget);
            Ancient.ancient.world.GetRenderer().ResetGraphics(Color.Transparent);

            Draw(item, new Vector3(0, 0, -1F), yaw, pitch, roll, xScale, yScale, zScale, true);

            Ancient.ancient.device.SetRenderTarget(null);
        }

        public static Vector3 GetRotationCenter(ModelData modelData)
        {
            float x = -modelData.GetWidth() / 2F;
            float y = modelData.GetHeight() / 2F;
            float z = -modelData.GetLength() / 2F;

            return new Vector3(x, y, z);
        }
    }
}
