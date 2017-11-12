using ancient.game.entity;
using ancient.game.world;
using ancient.game.world.block;
using ancientlib.game.entity;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.particle
{
    public abstract class Particle : Entity
    {
        protected Vector3 scale;

        protected Color color;
        protected Color startColor;
        protected Color endColor;

        protected int modelNum;

        public Particle(World world) : base(world)
        {
            this.world = world;
            this.gravity = 0;
            this.scale = Vector3.One;
            this.color = Color.White;
            this.startColor = color;
            this.endColor = color;
            this.lifeSpan = 256;
            this.interactWithEntities = false;
            UpdateBoundingBox();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (onGround)
            {
                this.yawVelocity = 0;
                this.pitchVelocity = 0;
                this.rollVelocity = 0;
            }

            UpdateDeath();
            UpdateColor();
        }

        private void UpdateDeath()
        {
            if (ticksExisted >= lifeSpan)
                world.DespawnParticle(this);
        }

        private void UpdateColor()
        {
            this.color = Color.Lerp(startColor, endColor, ticksExisted / (float)lifeSpan);
            this.color.A = (byte)MathHelper.Clamp((255 - ((ticksExisted - (lifeSpan - 64)) / 64F) * 255), 0, 255);
        }

        public Vector3 GetScale()
        {
            return this.scale;
        }

        public void SetScale(Vector3 scale)
        {
            this.scale = scale;
        }

        public Color GetColor()
        {
            return this.color;
        }

        public void SetColor(Color color)
        {
            this.color = color;
            this.startColor = color;
        }

        public void SetEndColor(Color endColor)
        {
            this.endColor = endColor;
        }

        public abstract string GetModelName();

        protected override EntityModelState GetDefaultModelState()
        {
            return null;
        }

        public override Vector3 GetModelScale()
        {
            return this.scale;
        }

        protected override bool ShouldInterpolate()
        {
            return false;
        }
    }
}
