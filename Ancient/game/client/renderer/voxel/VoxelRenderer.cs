using ancient.game.client.renderer;
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
        public static float LINE_THICKNESS = 1.001F;

        public static bool round = true;

        public static float scale = 1;

        public static void SetupEffect(Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale, float yScale, float zScale, bool exactPosition)
        {
            Vector3 translation = position;

            if (!exactPosition)
                translation -= Ancient.ancient.player.GetEyePosition();

            Matrix world = Matrix.CreateWorld(rotationCenter, Vector3.Forward, Vector3.Up) *
                Matrix.CreateScale(xScale * scale, yScale * scale, zScale * scale) *
                Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                Matrix.CreateTranslation(translation);

            WorldRenderer.currentEffect.Parameters["World"].SetValue(world);

            if (round)
            {
                translation.X = (float)Math.Round(translation.X);
                translation.Z = (float)Math.Round(translation.Z);
            }

            Matrix lightWorld = Matrix.CreateWorld(rotationCenter, Vector3.Forward, Vector3.Up) *
                Matrix.CreateScale(xScale * scale, yScale * scale, zScale * scale) *
                Matrix.CreateFromYawPitchRoll(yaw, pitch, roll) *
                Matrix.CreateTranslation(translation);

            WorldRenderer.currentEffect.Parameters["LightWorld"].SetValue(lightWorld);
        }

        public static void Draw(VoxelRendererData voxelData, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale = 1, float yScale = 1, float zScale = 1,
            bool exactPosition = false, DrawType drawType = DrawType.ALL)
        {
            EffectPassCollection passes = WorldRenderer.currentEffect.CurrentTechnique.Passes;

            for (int i = 0; i < passes.Count; i++)
            {
                EffectPass pass = passes[i];

                bool shouldDraw = false;

                if (WorldRenderer.currentEffect.CurrentTechnique.Name == "OutlineTechnique")
                    DrawOutlineTechnique(pass, position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, exactPosition, drawType, out shouldDraw);
                else
                    DrawRegularTechnique(pass, position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, exactPosition, drawType, out shouldDraw);

                if (shouldDraw == false)
                {
                    Console.WriteLine("NOT DRAWING");
                    continue;
                }

                pass.Apply();
                Draw(voxelData, drawType);
            }
        }

        public static void DrawSolid(VoxelRendererData voxelData, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale = 1, float yScale = 1,
            float zScale = 1, bool exactPosition = false)
        {
            Draw(voxelData, position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, exactPosition, DrawType.SOLID);
        }

        public static void DrawLiquid(VoxelRendererData voxelData, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale = 1, float yScale = 1,
            float zScale = 1, bool exactPosition = false)
        {
            Draw(voxelData, position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, exactPosition, DrawType.LIQUID);
        }

        private static void Draw(VoxelRendererData voxelData, DrawType drawType)
        {
            if (drawType == DrawType.SOLID)
                voxelData.DrawSolid();
            else if (drawType == DrawType.LIQUID)
                voxelData.DrawLiquid();
            else if (drawType == DrawType.ALL)
                voxelData.Draw();
        }

        private static void DrawRegularTechnique(EffectPass pass, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale, float yScale,
            float zScale, bool exactPosition, DrawType drawType, out bool shouldDraw)
        {
            shouldDraw = true;
            SetupEffect(position, rotationCenter, yaw, pitch, roll, xScale, yScale, zScale, exactPosition);
        }

        private static void DrawOutlineTechnique(EffectPass pass, Vector3 position, Vector3 rotationCenter, float yaw, float pitch, float roll, float xScale, float yScale,
            float zScale, bool exactPosition, DrawType drawType, out bool shouldDraw)
        {
            shouldDraw = true;

            float lineThickness = 1;

            if (pass.Name == "OutlinePass")
            {
                if (drawType == DrawType.LIQUID)
                {
                    shouldDraw = false;
                    return;
                }

                lineThickness = LINE_THICKNESS;
            }

            SetupEffect(position, rotationCenter, yaw, pitch, roll, xScale * lineThickness, yScale * lineThickness, zScale * lineThickness, exactPosition);
        }
    }

    public enum DrawType
    {
        SOLID,
        LIQUID,
        ALL
    }
}