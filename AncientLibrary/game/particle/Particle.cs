using ancient.game.entity;
using ancient.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.particle
{
 /*   public abstract class Particle
    {
        protected World world;

        protected Vector3 position;
        protected Vector3 velocity;
        protected Vector3 acceleration;
        protected Vector3 rotation;
        protected Vector3 rotationVelocity;
        protected Vector3 scale;

        protected Color color;
        protected Color startColor;
        protected Color endColor;

        protected int ticksExisted;
        protected int lifeSpan;

        protected int modelNum;

        public Particle(World world)
        {
            this.world = world;
            this.scale = Vector3.One;
            this.color = Color.White;
            this.startColor = color;
            this.endColor = color;
            this.lifeSpan = 128;
        }

        public virtual void Update()
        {
            ticksExisted++;

            this.position += velocity / 64F;
            this.velocity += acceleration / 64F;
            this.rotation += rotationVelocity / 64F;
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

        public Vector3 GetPosition()
        {
            return this.position;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        public Vector3 GetVelocity()
        {
            return this.velocity;
        }

        public void SetVelocity(Vector3 velocity)
        {
            this.velocity = velocity;
        }

        public Vector3 GetAcceleration()
        {
            return this.acceleration;
        }

        public void SetAcceleration(Vector3 acceleration)
        {
            this.acceleration = acceleration;
        }

        public Vector3 GetRotation()
        {
            return this.rotation;
        }

        public void SetRotation(Vector3 rotation)
        {
            this.rotation = rotation;
        }

        public Vector3 GetRotationVelocity()
        {
            return this.rotationVelocity;
        }

        public void SetRotationVelocity(Vector3 rotationVelocity)
        {
            this.rotationVelocity = rotationVelocity;
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
    }*/

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
            this.scale = Vector3.One;
            this.color = Color.White;
            this.startColor = color;
            this.endColor = color;
            this.lifeSpan = 256;
            this.interactWithEntities = false;
            this.interactWithBlocks = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if(onGround)
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

        public override Vector3 GetModelScale()
        {
            return this.scale;
        }
    }
}
