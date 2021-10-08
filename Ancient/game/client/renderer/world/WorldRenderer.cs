using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
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
using ancient.game.renderers.model;
using ancient.game.client.renderer.shadowmap;
using ancient.game.client.renderer.world;
using ancient.game.input;
using Microsoft.Xna.Framework.Input;
using ancient.game.client.camera;
using ancient.game.client.gui;
using System.IO;

namespace ancient.game.renderers.world
{
    public class WorldRenderer
    {
        private Ancient ancient;

        public GraphicsDevice GraphicsDevice;
        public SpriteBatch spriteBatch;

        private WorldClient world;
        public static CameraPlayerFOV camera;

        public RasterizerState rsDefault;
        public RasterizerState rsWire;

        private ShadowMapRenderer shadowMapRenderer;
        private SkyRenderer skyRenderer;
        public ChunkRenderer chunkRenderer;
        private EntityRenderer entityRenderer;
        private ItemRenderer itemRenderer;
        public ParticleRenderer particleRenderer;

        public static Effect currentEffect;
        public Effect effect;

        private bool drawChunksBoundingBox = false;
        private bool drawEntitiesPath = true;

        private Vector3 windDirection;

        private int ticks;

        private ModelData sun;

        private float roll;

        public bool underwater;

        private int technique;
        public int Technique
        {
            get
            {
                return technique;
            }
            set
            {
                currentEffect.CurrentTechnique = currentEffect.Techniques[value];
                technique = value;
            }
        }

        public float fogStart = 0.75F;

        public bool drawShadowDebug = false;

        public EntityPlayer characterPNG;

        private RenderTarget2D renderTarget;

        public bool screenshot;

        public WorldRenderer(WorldClient world)
        {
            this.ancient = Ancient.ancient;
            this.GraphicsDevice = ancient.GraphicsDevice;
            this.spriteBatch = ancient.spriteBatch;

            this.world = world;
            camera = new CameraPlayerFOV(ancient.player, 70, 0.01f, 1000);

            this.effect = Ancient.ancient.effect;
            currentEffect = effect;

            this.rsDefault = new RasterizerState() { CullMode = CullMode.CullCounterClockwiseFace, FillMode = FillMode.Solid, MultiSampleAntiAlias = true };
            this.rsWire = new RasterizerState() { CullMode = CullMode.CullCounterClockwiseFace, FillMode = FillMode.WireFrame, MultiSampleAntiAlias = true };

            this.shadowMapRenderer = new ShadowMapRenderer(this);
            this.skyRenderer = new SkyRenderer(this);
            this.chunkRenderer = new ChunkRenderer();
            this.entityRenderer = new EntityRenderer(world);
            this.itemRenderer = new ItemRenderer();
            this.particleRenderer = new ParticleRenderer(world);

            this.windDirection = new Vector3(0.5F, 0, 0.5F);

            this.sun = ModelDatabase.GetModelFromName("sun");

            Initialize();
            Technique = 0;
        }

        public void Initialize()
        {
            this.renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, false, SurfaceFormat.Color, DepthFormat.Depth24);
        }

        public void Update()
        {
            if (characterPNG != null)
                return;

            camera.Update();

            this.roll += 0.25F;
            this.ticks++;

            currentEffect.Parameters["WindStrength"].SetValue(0.25F);
            currentEffect.Parameters["WindDirection"].SetValue(windDirection);
            currentEffect.Parameters["time"].SetValue(ticks / 16F);

            skyRenderer.Update();
        }

        public void Draw()
        {
            if (characterPNG != null)
            {
                SaveCharacterPNG();
                characterPNG = null;
                return;
            }

            if (!Ancient.ancient.guiManager.GetCurrentGui().ShouldDrawWorldBehind())
                return;

            for (int i = 0; i < Ancient.ancient.guiManager.draw3DGuis.Count; i++)
                Ancient.ancient.guiManager.draw3DGuis[i].Draw3D();

            if (ancient.guiManager.GetCurrentGui() == ancient.guiManager.inventory)
                ancient.guiManager.inventory.DrawInventoryToRenderTarget(Ancient.ancient.spriteBatch);

            DrawShadowMap();

            GraphicsDevice.SetRenderTarget(renderTarget);
            ResetGraphics(world.GetSkyColor());

            SetupMatrices();
            SetupFog();

            //   DrawSun();
            //   DrawMoon();

            DrawUnderwaterFog();

            chunkRenderer.DrawSolid();

            entityRenderer.Draw();
            particleRenderer.Draw();

            chunkRenderer.DrawLiquid();

            DrawTargetedBlock();
            itemRenderer.Draw();

            GraphicsDevice.SetRenderTarget(null);

            ancient.device.RasterizerState = rsDefault;
            ancient.device.BlendState = BlendState.Opaque;
            ancient.device.DepthStencilState = DepthStencilState.Default;

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

            //     if (drawShadowDebug)
            //       shadowMapRenderer.DrawDebugShadowMap();

            spriteBatch.Begin();
            spriteBatch.Draw(renderTarget, new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height), Color.White);
            spriteBatch.End();

            if (screenshot)
            {
                DateTime date = DateTime.Now;
                string dateString = date.Year + "_" + date.Month + "_" + date.Day + "_" + date.Hour + "-" + date.Minute + "-" + date.Second;
                FileStream stream = File.Open(Environment.CurrentDirectory + "/screenshot" + dateString + ".png", FileMode.OpenOrCreate);
                renderTarget.SaveAsPng(stream, renderTarget.Width, renderTarget.Height);
                stream.Close();
                screenshot = false;
            }
        }

        public void DrawShadowMap()
        {
            ResetGraphics();

            shadowMapRenderer.UpdateLightViewProjection();

            shadowMapRenderer.SetShadowMap();

            chunkRenderer.DrawShadowSolid();
            entityRenderer.Draw();

            shadowMapRenderer.ResolveShadowMap();
        }

        private void DrawUnderwaterFog()
        {
            underwater = false;
            Vector3 position = Ancient.ancient.player.GetEyePosition() + camera.GetTargetVector() * -camera.GetDistance();
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
                        underwater = true;
                        Vector4 color = Blocks.water.GetColor().ToVector4();
                        color.W = 255;
                        currentEffect.Parameters["FogEnabled"].SetValue(true);
                        currentEffect.Parameters["FogColor"].SetValue(color);
                        currentEffect.Parameters["FogStart"].SetValue(0F);
                        currentEffect.Parameters["FogEnd"].SetValue(16F);
                    }
                }
            }
        }

        private void DrawSun()
        {
            ancient.device.DepthStencilState = DepthStencilState.None;

            currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            currentEffect.Parameters["MultiplyColor"].SetValue(Color.Yellow.ToVector3());
            currentEffect.Parameters["FogEnabled"].SetValue(false);

            Vector3 position = new Vector3(sun.GetWidth(), 0, sun.GetLength()) / -2F + camera.GetTargetVector() * -camera.GetDistance();
            currentEffect.Parameters["World"].SetValue(
               Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up)
               * Matrix.CreateScale(0.125F * 24F) * Matrix.CreateFromYawPitchRoll(0, 0, 0) * Matrix.CreateTranslation(Vector3.Up * 192) * Matrix.CreateFromYawPitchRoll(0, 0, MathHelper.ToRadians(roll)));

            foreach (EffectPass pass in currentEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                sun.GetVoxelRendererData().DrawSolid();
            }

            ancient.device.DepthStencilState = DepthStencilState.Default;
            currentEffect.Parameters["FogEnabled"].SetValue(true);
        }

        private void DrawMoon()
        {
            ancient.device.DepthStencilState = DepthStencilState.None;
            currentEffect.Parameters["MultiplyColor"].SetValue(Color.DarkRed.ToVector3());

            Vector3 position = new Vector3(sun.GetWidth(), 0, sun.GetLength()) / -2F + camera.GetTargetVector() * -camera.GetDistance();
            currentEffect.Parameters["World"].SetValue(
               Matrix.CreateWorld(position, Vector3.Forward, Vector3.Up)
               * Matrix.CreateScale(0.125F) * Matrix.CreateFromYawPitchRoll(0, 0, 0) * Matrix.CreateTranslation(Vector3.Up * 8) * Matrix.CreateFromYawPitchRoll(0, 0, MathHelper.ToRadians(180 + roll)));

            foreach (EffectPass pass in currentEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                sun.GetVoxelRendererData().DrawSolid();
            }
            ancient.device.DepthStencilState = DepthStencilState.Default;
            currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);
        }

        private void DrawTargetedBlock()
        {
            Vector3? targetedBlock = Ancient.ancient.player.GetTargetedBlockPosition();

            if (targetedBlock.HasValue)
            {
                Vector3 position = targetedBlock.Value;
                Block block = Ancient.ancient.world.GetBlock((int)position.X, (int)position.Y, (int)position.Z);

                ModelData model = ModelDatabase.GetModelFromName(block.GetModelName());

                Vector3 offset = Vector3.Zero;
                if (model != null)
                    offset = model.GetOffset();

                RenderUtils.DrawLineVoxel(position + offset, position + offset + block.GetDimensions(), Color.Black);

                currentEffect.Parameters["ShadowsEnabled"].SetValue(false);
                currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);

                Color color = Color.Lerp(Color.Transparent, Color.Black, ancient.player.destroyAnimation);
                currentEffect.Parameters["MultiplyColor"].SetValue(color.ToVector4());

                RenderUtils.DrawVoxel(position + offset, block.GetDimensions(), Color.White * ancient.player.destroyAnimation);

                currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);
            }
        }

        public void ResetGraphics(Color color)
        {
            GraphicsDevice.Clear(color);
            GraphicsDevice.RasterizerState = rsDefault;
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
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
            currentEffect.Parameters["View"].SetValue(camera.GetViewMatrix());
            currentEffect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());
        }

        public void SetupFog()
        {
            currentEffect.Parameters["FogEnabled"].SetValue(true);
            currentEffect.Parameters["FogColor"].SetValue(Ancient.ancient.world.GetSkyColor().ToVector4());
            currentEffect.Parameters["FogStart"].SetValue(Ancient.ancient.player.GetRenderDistance() * 16F * fogStart);
            currentEffect.Parameters["FogEnd"].SetValue(Ancient.ancient.player.GetRenderDistance() * 16F);
        }

        public WorldClient GetWorld()
        {
            return this.world;
        }

        public ShadowMapRenderer GetShadowMapRenderer()
        {
            return this.shadowMapRenderer;
        }

        private void SaveCharacterPNG()
        {
            int width = GraphicsDevice.Viewport.Width / 4;
            int height = GraphicsDevice.Viewport.Height / 4;
            RenderTarget2D renderTarget = new RenderTarget2D(GraphicsDevice, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24);

            Vector3 position = Ancient.ancient.player.GetPosition();
            Ancient.ancient.player.SetPosition(Vector3.Zero);

            CameraFOV camera = new CameraFOV(70, 0.1F, 1000, 0, characterPNG.GetHeadPitch(), 0);
            camera.SetTargetVector(Vector3.Backward);
            camera.SetPosition(new Vector3(0, -0.5F, -10));
            camera.Update();

            currentEffect.Parameters["FogEnabled"].SetValue(false);

            currentEffect.Parameters["View"].SetValue(camera.GetViewMatrix());
            currentEffect.Parameters["Projection"].SetValue(camera.GetProjectionMatrix());

            currentEffect.Parameters["MultiplyColorEnabled"].SetValue(true);
            currentEffect.Parameters["ShadowsEnabled"].SetValue(false);
            currentEffect.Parameters["EntityShadowMode"].SetValue(false);

            Technique = 0;

            GraphicsDevice.SetRenderTarget(renderTarget);
            ResetGraphics(Color.Transparent);

            VoxelRenderer.scale = 7;
            EntityRenderers.renderPlayer.Draw(characterPNG);
            VoxelRenderer.scale = 1;

            GraphicsDevice.SetRenderTarget(null);
            currentEffect.Parameters["MultiplyColorEnabled"].SetValue(false);

            int[] colorsArray = new int[renderTarget.Width * renderTarget.Height];
            renderTarget.GetData(colorsArray);

            Ancient.ancient.player.SetPosition(position);

            //Ancient.ancient.service.SetCharacterImage(characterPNG.GetName(), width, height, colorsArray);
        }
    }
}