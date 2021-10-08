using ancient.game.client.renderer.entity;
using ancient.game.client.renderer.texture;
using ancient.game.entity.player;
using ancient.game.renderers.voxel;
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
            WorldRenderer.currentEffect.Parameters["World"].SetValue(Matrix.CreateTranslation(-Ancient.ancient.player.GetEyePosition()));
            WorldRenderer.currentEffect.Parameters["View"].SetValue(WorldRenderer.camera.GetViewMatrix());
            WorldRenderer.currentEffect.Parameters["Projection"].SetValue(WorldRenderer.camera.GetProjectionMatrix());

            foreach (EffectPass pass in WorldRenderer.currentEffect.CurrentTechnique.Passes)
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

            VertexPositionColorNormal[] vertices = new VertexPositionColorNormal[24];

            vertices[0] = new VertexPositionColorNormal(min, color);
            vertices[1] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, 0, 0), color);

            vertices[2] = new VertexPositionColorNormal(min, color);
            vertices[3] = new VertexPositionColorNormal(min + new Vector3(0, 0, max.Z - min.Z), color);

            vertices[4] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, 0, 0), color);
            vertices[5] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, 0, max.Z - min.Z), color);

            vertices[6] = new VertexPositionColorNormal(min + new Vector3(0, 0, max.Z - min.Z), color);
            vertices[7] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, 0, max.Z - min.Z), color);

            vertices[8] = new VertexPositionColorNormal(min + new Vector3(0, max.Y - min.Y, 0), color);
            vertices[9] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, max.Y - min.Y, 0), color);

            vertices[10] = new VertexPositionColorNormal(min + new Vector3(0, max.Y - min.Y, 0), color);
            vertices[11] = new VertexPositionColorNormal(min + new Vector3(0, max.Y - min.Y, max.Z - min.Z), color);

            vertices[12] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, max.Y - min.Y, 0), color);
            vertices[13] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z), color);

            vertices[14] = new VertexPositionColorNormal(min + new Vector3(0, max.Y - min.Y, max.Z - min.Z), color);
            vertices[15] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z), color);

            vertices[16] = new VertexPositionColorNormal(min, color);
            vertices[17] = new VertexPositionColorNormal(min + new Vector3(0, max.Y - min.Y, 0), color);

            vertices[18] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, 0, 0), color);
            vertices[19] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, max.Y - min.Y, 0), color);

            vertices[20] = new VertexPositionColorNormal(min + new Vector3(0, 0, max.Z - min.Z), color);
            vertices[21] = new VertexPositionColorNormal(min + new Vector3(0, max.Y - min.Y, max.Z - min.Z), color);

            vertices[22] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, 0, max.Z - min.Z), color);
            vertices[23] = new VertexPositionColorNormal(min + new Vector3(max.X - min.X, max.Y - min.Y, max.Z - min.Z), color);

            Ancient.ancient.device.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 12);
        }

        public static void DrawVoxel(Vector3 position, Vector3 size, Color color)
        {
            SetupEffect();

            Color downColor = color * 0.7F;
            downColor.A = color.A;

            Color zColor = color * 0.8F;
            zColor.A = color.A;

            Color xColor = color * 0.9F;
            xColor.A = color.A;

            VertexPositionColorNormal d1 = new VertexPositionColorNormal(position, downColor);
            VertexPositionColorNormal d2 = new VertexPositionColorNormal(new Vector3(position.X, position.Y, position.Z + size.Z), downColor);
            VertexPositionColorNormal d3 = new VertexPositionColorNormal(new Vector3(position.X + size.X, position.Y, position.Z), downColor);
            VertexPositionColorNormal d4 = new VertexPositionColorNormal(new Vector3(position.X + size.X, position.Y, position.Z + size.Z), downColor);

            VertexPositionColorNormal u1 = new VertexPositionColorNormal(d1.Position + Vector3.Up * size.Y, color);
            VertexPositionColorNormal u2 = new VertexPositionColorNormal(d2.Position + Vector3.Up * size.Y, color);
            VertexPositionColorNormal u3 = new VertexPositionColorNormal(d3.Position + Vector3.Up * size.Y, color);
            VertexPositionColorNormal u4 = new VertexPositionColorNormal(d4.Position + Vector3.Up * size.Y, color);

            VertexPositionColorNormal n1 = new VertexPositionColorNormal(d1.Position, zColor);
            VertexPositionColorNormal n2 = new VertexPositionColorNormal(u1.Position, zColor);
            VertexPositionColorNormal n3 = new VertexPositionColorNormal(d3.Position, zColor);
            VertexPositionColorNormal n4 = new VertexPositionColorNormal(u3.Position, zColor);

            VertexPositionColorNormal s1 = new VertexPositionColorNormal(d2.Position, zColor);
            VertexPositionColorNormal s2 = new VertexPositionColorNormal(d4.Position, zColor);
            VertexPositionColorNormal s3 = new VertexPositionColorNormal(u2.Position, zColor);
            VertexPositionColorNormal s4 = new VertexPositionColorNormal(u4.Position, zColor);

            VertexPositionColorNormal w1 = new VertexPositionColorNormal(d1.Position, xColor);
            VertexPositionColorNormal w2 = new VertexPositionColorNormal(d2.Position, xColor);
            VertexPositionColorNormal w3 = new VertexPositionColorNormal(u1.Position, xColor);
            VertexPositionColorNormal w4 = new VertexPositionColorNormal(u2.Position, xColor);

            VertexPositionColorNormal e1 = new VertexPositionColorNormal(d3.Position, xColor);
            VertexPositionColorNormal e2 = new VertexPositionColorNormal(u3.Position, xColor);
            VertexPositionColorNormal e3 = new VertexPositionColorNormal(d4.Position, xColor);
            VertexPositionColorNormal e4 = new VertexPositionColorNormal(u4.Position, xColor);

            List<VertexPositionColorNormal> vertices = new List<VertexPositionColorNormal>();

            vertices.AddRange(new VertexPositionColorNormal[] { d1, d2, d3, d3, d2, d4 });
            vertices.AddRange(new VertexPositionColorNormal[] { u1, u3, u2, u2, u3, u4 });
            vertices.AddRange(new VertexPositionColorNormal[] { n1, n3, n2, n2, n3, n4 });
            vertices.AddRange(new VertexPositionColorNormal[] { s1, s3, s2, s2, s3, s4 });
            vertices.AddRange(new VertexPositionColorNormal[] { w1, w3, w2, w2, w3, w4 });
            vertices.AddRange(new VertexPositionColorNormal[] { e1, e3, e2, e2, e3, e4 });

            Ancient.ancient.device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices.ToArray(), 0, 12);
        }

        public static void DrawPath(List<PathNode> path)
        {
            if (path != null)
            {
                foreach (PathNode pathNode in path)
                    DrawVoxel(pathNode.GetPosition() + new Vector3(1, 0, 1) / 2F, Vector3.One * 0.25F, Color.Red);
            }
        }

        public static void DrawBoundingBox(BoundingBox boundingBox, Color color)
        {
            DrawLineVoxel(boundingBox.Min, boundingBox.Max, color);
        }

        public static Texture2D GetCharacterImage(EntityPlayer player)
        {
            GraphicsDevice GraphicsDevice = Ancient.ancient.GraphicsDevice;
            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            GraphicsDevice.SetRenderTarget(renderTarget);

            Ancient.ancient.world.GetRenderer().ResetGraphics(Color.Transparent);

            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            EntityRenderers.renderPlayer.Draw(player);
            WorldRenderer.currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);

            GraphicsDevice.SetRenderTarget(null);

            return renderTarget;
        }
    }
}
