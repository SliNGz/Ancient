using ancient.game.renderers.world;
using ancient.game.world.generator.noise;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.world
{
    class SkyRenderer
    {
        WorldRenderer worldRenderer;

        private GraphicsDevice GraphicsDevice;
        private SpriteBatch spriteBatch;

        private QuadRenderer quadRenderer;
        private Effect skyEffect;

        private RenderTarget2D renderTarget;

        private Texture2D noiseTexture;
        SimplexNoise noise;

        public SkyRenderer(WorldRenderer worldRenderer)
        {
            this.worldRenderer = worldRenderer;
            GraphicsDevice = worldRenderer.GraphicsDevice;
            spriteBatch = worldRenderer.spriteBatch;

            this.quadRenderer = new QuadRenderer();
            this.skyEffect = Ancient.ancient.Content.Load<Effect>("effects/SkyClear");
            skyEffect.Parameters["Color1"].SetValue(Color.BlueViolet.ToVector4());
            skyEffect.Parameters["Color2"].SetValue(Color.MediumSeaGreen.ToVector4());
            this.renderTarget = new RenderTarget2D(Ancient.ancient.GraphicsDevice, Ancient.ancient.GraphicsDevice.Viewport.Width, Ancient.ancient.GraphicsDevice.Viewport.Height);

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            Texture2D texture = new Texture2D(GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight);

            Color[] data = new Color[texture.Width * texture.Height];
            for (int i = 0; i < texture.Width; i++)
            {
                for (int j = 0; j < texture.Height; j++)
                {
                    data[i + j * texture.Width] = new Color(255, 0, j / 255F, 1);
                }
            }
            texture.SetData(data);

            noise = worldRenderer.GetWorld().GetSimplexNoise();
            //skyEffect.Parameters["SkyTexture"].SetValue(texture);
            noiseTexture = new Texture2D(GraphicsDevice, 128, 128);

            Color[] noiseData = new Color[128 * 128];

            for (int i = 0; i < noiseTexture.Width; i++)
            {
                for (int j = 0; j < noiseTexture.Height; j++)
                {
                    noiseData[i + j * noiseTexture.Width] = Color.White * (float)noise.Evaluate(i / 512F, j / 512F);
                }
            }

            noiseTexture.SetData(noiseData);

            skyEffect.Parameters["NoiseTexture"].SetValue(noiseTexture);
        }

        public void Clear()
        {
            skyEffect.CurrentTechnique.Passes[0].Apply();
            Matrix world = Matrix.CreateWorld(Vector3.Zero, Vector3.Forward, Vector3.Up);
            Matrix view = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
            Matrix playerView = WorldRenderer.camera.GetViewMatrix();
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), Ancient.ancient.GraphicsDevice.DisplayMode.AspectRatio, 0.00001F, 1);
            skyEffect.Parameters["World"].SetValue(world);
            skyEffect.Parameters["View"].SetValue(view);
            skyEffect.Parameters["PlayerView"].SetValue(playerView);
            skyEffect.Parameters["Projection"].SetValue(projection);
            quadRenderer.Draw();

            if (ticks % 128 != 0)
                return;

            noise = new SimplexNoise(worldRenderer.GetWorld().rand.Next());

            Color[] noiseData = new Color[128 * 128];

            for (int i = 0; i < noiseTexture.Width; i++)
            {
                for (int j = 0; j < noiseTexture.Height; j++)
                {
                    noiseData[i + j * noiseTexture.Width] = Color.White * (float)noise.Evaluate(i / 32F, j / 32F);
                }
            }

            noiseTexture.SetData(noiseData);

            skyEffect.Parameters["NoiseTexture"].SetValue(noiseTexture);
        }

        int ticks = 0;

        public void Update()
        {
            ticks++;

            skyEffect.Parameters["Time"].SetValue(ticks / 64F);
        }
    }
}
