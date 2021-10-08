using ancient.game.input;
using ancient.game.renderers.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.client.renderer.shadowmap
{
    public class ShadowMapRenderer
    {
        private WorldRenderer worldRenderer;

        private GraphicsDevice GraphicsDevice;
        private SpriteBatch spriteBatch;

        private RenderTarget2D shadowMapRT;

        private Vector3 lightPosition = new Vector3(0, 256.1F, 0);
        private Vector3 lightDirection = new Vector3(0, -1, 0);

        public Matrix lightView;
        public Matrix lightProjection;

        private int shadowResolution = 12;
        private int detailRadius = 8;

        public ShadowMapRenderer(WorldRenderer worldRenderer)
        {
            this.worldRenderer = worldRenderer;
            this.GraphicsDevice = worldRenderer.GraphicsDevice;
            this.spriteBatch = worldRenderer.spriteBatch;

            Initialize();
        }

        public void Initialize()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            int shadowResolution = (int)Math.Pow(2, this.shadowResolution);
            this.shadowMapRT = new RenderTarget2D(GraphicsDevice, shadowResolution, shadowResolution, false, SurfaceFormat.Single, DepthFormat.Depth24);
        }

        public void UpdateLightViewProjection()
        {
            Vector3 eyePosition = Ancient.ancient.player.GetEyePosition();

            lightView = Matrix.CreateLookAt(lightPosition - Vector3.Up * eyePosition.Y, lightPosition - Vector3.Up * eyePosition.Y + lightDirection, Vector3.Forward);
            lightProjection = Matrix.CreateOrthographic(16 * detailRadius * 2, 16 * detailRadius * 2, 0.1f, 256);

            WorldRenderer.currentEffect.Parameters["LightViewProjection"].SetValue(lightView * lightProjection); 
        }

        public void SetShadowMap()
        {
            worldRenderer.Technique = 2;
            WorldRenderer.currentEffect.Parameters["EntityShadowMode"].SetValue(false);
            WorldRenderer.currentEffect.Parameters["ShadowsEnabled"].SetValue(true);
            GraphicsDevice.SetRenderTarget(shadowMapRT);
            GraphicsDevice.Clear(Color.Black);
        }

        public void ResolveShadowMap()
        {
            GraphicsDevice.SetRenderTarget(null);
            worldRenderer.Technique = 0;
            WorldRenderer.currentEffect.Parameters["ShadowMapTexture"].SetValue(shadowMapRT);
        }

        public void DrawDebugShadowMap()
        {
            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.PointClamp);
            spriteBatch.Draw(shadowMapRT, new Rectangle(0, 0, pp.BackBufferWidth, pp.BackBufferHeight), Color.White);
            spriteBatch.End();
        }
    }
}
