using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ancient.game.camera;
using ancient.game.world.block;
using ancient.game.client.world;
using ancient.game.client.renderer.entity;
using ancient.game.client.renderer.item;
using ancient.game.client.particle;
using ancient.game.renderers.voxel;
using ancientlib.game.init;
using ancient.game.entity.player;
using ancient.game.utils;
using ancient.game.client.renderer.model;
using ancientlib.game.entity;
using System.Linq;
using ancient.game.entity;
using ancient.game.client.renderer.chunk;
using System.Collections.Generic;
using ancient.game.client.renderer.particle;

namespace ancient.game.renderers.world
{
    public class WorldRenderer
    {
        private Ancient ancient;

        private WorldClient world;
        public static Camera camera;

        public RasterizerState rs;

        private ChunkRenderer chunkRenderer;
        private EntityRenderer entityRenderer;
        private ItemRenderer itemRenderer;
        private ParticleRenderer particleRenderer;

        public static Effect effect;

        private bool drawChunksBoundingBox = false;
        private bool drawEntitiesPath = false;

        public WorldRenderer(WorldClient world)
        {
            this.ancient = Ancient.ancient;

            this.world = world;
            camera = new Camera(70, 0.01f, Ancient.ancient.gameSettings.GetRenderDistance() * 16 + 16);

            this.rs = RasterizerState.CullClockwise;

            effect = Ancient.ancient.effect;

            this.chunkRenderer = new ChunkRenderer();
            this.entityRenderer = new EntityRenderer(world);
            this.itemRenderer = new ItemRenderer();
            this.particleRenderer = new ParticleRenderer(world);
        }

        public void Draw()
        {
            if (!Ancient.ancient.guiManager.GetCurrentGui().ShouldDrawWorldBehind())
                return;

            ResetGraphics(world.GetSkyColor());

            SetupMatrices();
            SetupFog();

            UpdateUnderwaterFog();
            chunkRenderer.DrawSolid();

            entityRenderer.Draw();
            particleRenderer.Draw();
            itemRenderer.Draw();

            SetupMatrices();
            SetupFog();

            chunkRenderer.DrawLiquid();

            if (drawChunksBoundingBox)
            {
                lock (world.GetChunkManager().GetChunksDictionary())
                {
                    foreach (Vector3 index in world.GetChunkManager().GetChunksDictionary().Keys)
                        RenderUtils.DrawBoundingBox(world.GetChunkBoundingBox((int)index.X, (int)index.Y, (int)index.Z), Color.Red);
                }
            }

            if (drawEntitiesPath)
            {
                List<EntityLiving> living = world.entityList.OfType<EntityLiving>().ToList();

                for (int i = 0; i < living.Count; i++)
                {
                    EntityLiving entity = living[i];

                    if (entity.GetPathFinder().HasPath())
                        RenderUtils.DrawPath(entity.GetPathFinder().GetPath());
                }
            }
        }

        private void UpdateUnderwaterFog()
        {
            Vector3 position = Ancient.ancient.player.GetEyePosition() + camera.GetTargetVector(Ancient.ancient.player.GetHeadYaw(), Ancient.ancient.player.GetHeadPitch()) * -camera.distance;
            Block block = world.GetBlock((int)Math.Floor(position.X), (int)Math.Floor(position.Y), (int)Math.Floor(position.Z));
            Block blockAbove = world.GetBlock((int)Math.Floor(position.X), (int)Math.Floor(position.Y) + 1, (int)Math.Floor(position.Z));

            if (block != null)
            {
                BoundingBox boundingBox = world.GetBlockBoundingBox(block, (int)Math.Floor(position.X), (int)Math.Floor(position.Y), (int)Math.Floor(position.Z));
                float y = boundingBox.Max.Y;

                if (block is BlockWater)
                {
                    if (blockAbove != null && blockAbove is BlockWater) // Water block's ySize is 0.8F, needs to be 1F so player will "be underwater" all the way.
                        y = (float)Math.Ceiling(y);

                    if (position.Y <= y)
                    {
                        Vector4 color = Blocks.water.GetColor().ToVector4();
                        color.W = 255;
                        effect.Parameters["FogEnabled"].SetValue(true);
                        effect.Parameters["FogColor"].SetValue(color);
                        effect.Parameters["FogStart"].SetValue(0F);
                        effect.Parameters["FogEnd"].SetValue(16F);
                    }
                }
            }
        }

        public void ResetGraphics(Color color)
        {
            ancient.device.Clear(color);
            ancient.device.RasterizerState = rs;
            ancient.device.BlendState = BlendState.Opaque;
            ancient.device.DepthStencilState = DepthStencilState.Default;
        }

        public void ResetGraphics()
        {
            ResetGraphics(world.GetSkyColor());
        }

        public ChunkRenderer GetChunkRenderer()
        {
            return this.chunkRenderer;
        }

        public void SetupMatrices()
        {
            effect.Parameters["View"].SetValue(camera.GetViewMatrix(Ancient.ancient.player.GetHeadYaw(), Ancient.ancient.player.GetHeadPitch()));
            effect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
        }

        public void SetupFog()
        {
            effect.Parameters["FogEnabled"].SetValue(true);
            effect.Parameters["FogColor"].SetValue(Ancient.ancient.world.GetSkyColor().ToVector4());
            effect.Parameters["FogStart"].SetValue(Ancient.ancient.player.GetRenderDistance() * 16F * 0.75F);
            effect.Parameters["FogEnd"].SetValue(Ancient.ancient.player.GetRenderDistance() * 16F);
        }

        public WorldClient GetWorld()
        {
            return this.world;
        }
    }
}