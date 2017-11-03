using ancient.game.client.renderer.texture;
using ancient.game.renderers.world;
using ancientlib.game.entity.ai.pathfinding;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace ancient.game.utils
{
    public class RenderUtils
    {
        public static void SetupEffect()
        {
            WorldRenderer.effect.Parameters["World"].SetValue(Matrix.CreateTranslation(-Ancient.ancient.player.GetEyePosition()));
            WorldRenderer.effect.Parameters["View"].SetValue(WorldRenderer.camera.GetViewMatrix(Ancient.ancient.player.GetHeadYaw(), Ancient.ancient.player.GetHeadPitch()));
            WorldRenderer.effect.Parameters["Projection"].SetValue(WorldRenderer.camera.GetProjectionMatrix());

            foreach (EffectPass pass in WorldRenderer.effect.CurrentTechnique.Passes)
                pass.Apply();
        }

        public static void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            SetupEffect();

            VertexPositionColor[] vertices = new VertexPositionColor[2];
            vertices[0] = new VertexPositionColor(start, color);
            vertices[1] = new VertexPositionColor(end, color);

            Ancient.ancient.device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1);
        }

        public static void DrawLineVoxel(Vector3 min, Vector3 max, Color color)
        {
            SetupEffect();

            VertexPositionColor[] vertices = new VertexPositionColor[24];

            vertices[0] = new VertexPositionColor(min, color);
            vertices[1] = new VertexPositionColor(min + new Vector3(max.X - min.X, 0, 0), color);

            vertices[2] = new VertexPositionColor(min, color);
            vertices[3] = new VertexPositionColor(min + new Vector3(0, 0, max.Z - min.Z), color);

            vertices[4] = new VertexPositionColor(min + new Vector3(max.X - min.X, 0, 0), color);
            vertices[5] = new VertexPositionColor(min + new Vector3(max.X - min.X, 0, max.Z - min.Z), color);

            vertices[6] = new VertexPositionColor(min + new Vector3(0, 0, max.Z - min.Z), color);
            vertices[7] = new VertexPositionColor(min + new Vector3(max.X - min.X, 0, max.Z - min.Z), color);

            vertices[8] = new VertexPositionColor(min + new Vector3(0, max.Y - min.Y, 0), color);
            vertices[9] = new VertexPositionColor(min + new Vector3(max.X - min.X, max.Y - min.Y, 0), color);

            vertices[10] = new VertexPositionColor(min + new Vector3(0, max.Y - min.Y, 0), color);
            vertices[11] = new VertexPositionColor(min + new Vector3(0, max.Y - min.Y, max.Z - min.Z), color);

            vertices[12] = new VertexPositionColor(min + new Vector3(max.X - min.X, max.Y - min.Y, 0), color);
            vertices[13] = new VertexPositionColor(min + new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z), color);

            vertices[14] = new VertexPositionColor(min + new Vector3(0, max.Y - min.Y, max.Z - min.Z), color);
            vertices[15] = new VertexPositionColor(min + new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z), color);

            vertices[16] = new VertexPositionColor(min, color);
            vertices[17] = new VertexPositionColor(min + new Vector3(0, max.Y - min.Y, 0), color);

            vertices[18] = new VertexPositionColor(min + new Vector3(max.X - min.X, 0, 0), color);
            vertices[19] = new VertexPositionColor(min + new Vector3(max.X - min.X, max.Y - min.Y, 0), color);

            vertices[20] = new VertexPositionColor(min + new Vector3(0, 0, max.Z - min.Z), color);
            vertices[21] = new VertexPositionColor(min + new Vector3(0, max.Y - min.Y, max.Z - min.Z), color);

            vertices[22] = new VertexPositionColor(min + new Vector3(max.X - min.X, 0, max.Z - min.Z), color);
            vertices[23] = new VertexPositionColor(min + new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z), color);

            Ancient.ancient.device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 12);
        }

        public static void DrawVoxel(Vector3 position, float size, Color color)
        {
            SetupEffect();

            VertexPositionColor p1 = new VertexPositionColor(position, color);
            VertexPositionColor p2 = new VertexPositionColor(new Vector3(position.X, position.Y, position.Z + size), color);
            VertexPositionColor p3 = new VertexPositionColor(new Vector3(position.X + size, position.Y, position.Z), color);
            VertexPositionColor p4 = new VertexPositionColor(new Vector3(position.X + size, position.Y, position.Z + size), color);
            VertexPositionColor p5 = new VertexPositionColor(new Vector3(position.X, position.Y - size, position.Z), color);
            VertexPositionColor p6 = new VertexPositionColor(new Vector3(position.X, position.Y - size, position.Z + size), color);
            VertexPositionColor p7 = new VertexPositionColor(new Vector3(position.X + size, position.Y - size, position.Z), color);
            VertexPositionColor p8 = new VertexPositionColor(new Vector3(position.X + size, position.Y - size, position.Z + size), color);

            List<VertexPositionColor> vertices = new List<VertexPositionColor>();

            vertices.AddRange(new VertexPositionColor[] { p1, p2, p3, p2, p3, p4 });
            vertices.AddRange(new VertexPositionColor[] { p5, p6, p7, p6, p7, p8 });
            vertices.AddRange(new VertexPositionColor[] { p1, p3, p5, p3, p5, p7 });
            vertices.AddRange(new VertexPositionColor[] { p2, p4, p6, p4, p6, p8 });
            vertices.AddRange(new VertexPositionColor[] { p1, p2, p5, p2, p5, p6 });
            vertices.AddRange(new VertexPositionColor[] { p3, p4, p7, p4, p7, p8 });

            Ancient.ancient.device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, 12);
        }

        public static void DrawPath(List<PathNode> path)
        {
            if (path != null)
            {
                SetupEffect();
                foreach (PathNode pathNode in path)
                    DrawVoxel(pathNode.GetPosition(), 0.25f, Color.Red);
            }
        }

        public static void DrawBoundingBox(BoundingBox boundingBox, Color color)
        {
            DrawLineVoxel(boundingBox.Min, boundingBox.Max, color);
        }
    }
}
