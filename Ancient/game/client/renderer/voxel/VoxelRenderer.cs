using ancient.game.renderers.world;
using ancient.game.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ancient.game.renderers.voxel
{
    public class VoxelRenderer
    {
        public static void SetupEffect(Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale, float yScale, float zScale, int techniqueIndex, bool exactPosition)
        {
            WorldRenderer.effect.CurrentTechnique = WorldRenderer.effect.Techniques[techniqueIndex];

            Vector3 translation = position;
            if (!exactPosition)
                translation -= Ancient.ancient.player.GetEyePosition();

            WorldRenderer.effect.Parameters["World"].SetValue(
                Matrix.CreateWorld(rotationCenter, Vector3.Forward, Vector3.Up) *
                Matrix.CreateScale(xScale, yScale, zScale) *
                Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                Matrix.CreateTranslation(translation));

            foreach (EffectPass pass in WorldRenderer.effect.CurrentTechnique.Passes)
                pass.Apply();
        }

        public static void Draw(VoxelRendererData voxelData, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale = 1, float yScale = 1, float zScale = 1, int techniqueIndex = 0, bool exactPosition = false)
        {
            SetupEffect(position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, techniqueIndex, exactPosition);
            voxelData.DrawSolid();
            voxelData.DrawLiquid();
        }

        public static void DrawSolid(VoxelRendererData voxelData, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale = 1, float yScale = 1, float zScale = 1, int techniqueIndex = 0, bool exactPosition = false)
        {
            SetupEffect(position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, techniqueIndex, exactPosition);
            voxelData.DrawSolid();
        }

        public static void DrawLiquid(VoxelRendererData voxelData, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale = 1, float yScale = 1, float zScale = 1, int techniqueIndex = 0, bool exactPosition = false)
        {
            SetupEffect(position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, techniqueIndex, exactPosition);
            voxelData.DrawLiquid();
        }
    }
}