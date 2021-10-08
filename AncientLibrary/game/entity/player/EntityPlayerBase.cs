using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using ancientlib.game.entity.model;
using Microsoft.Xna.Framework;
using ancient.game.entity.player;

namespace ancientlib.game.entity.player
{
    public abstract class EntityPlayerBase : EntityDeveloping
    {
        protected Color skinColor;

        protected byte hairID;
        protected Color hairColor;

        protected byte eyesID;
        protected Color eyesColor;

        protected BoundingBox dropBoundingBox;

        public Vector3 inputVector;

        public float handPitch;

        public EntityPlayerBase(World world) : base(world)
        {
            this.skinColor = new Color(255, 218, 182);

            this.hairID = 0;
            this.hairColor = new Color(222, 222, 222);

            this.eyesID = 4;
            this.eyesColor = Color.DarkRed;

            this.interactWithEntities = false;

            this.hairID = 1;
        }

        public override void Update(GameTime gameTime)
        {
            SetMovement(inputVector);
            UpdateAnimation();
            base.Update(gameTime);
            inputVector = Vector3.Zero;
        }

        private void UpdateAnimation()
        {
            float num = inputVector.Z;
            float speed = this.speed;

            Vector2 position = new Vector2(x, z);
            Vector2 serverPosition = new Vector2(xServer, zServer);
            float distance = Vector2.Distance(position, serverPosition);

            if (world.IsRemote() && distance > 0.05F && this != world.GetMyPlayer())
            {
                num = -1;
                speed = GetBaseSpeed();
            }

            this.handPitch = (float)Math.Sin(ticksExisted / 32F * speed) * num;
        }

        public override Vector3 GetEyePosition()
        {
            return this.GetPosition() + GetEyesOffset() * GetModelScale() * Vector3.UnitY;
        }

        public Color GetSkinColor()
        {
            return this.skinColor;
        }

        public void SetSkinColor(Color skinColor)
        {
            this.skinColor = skinColor;
        }

        public byte GetHairID()
        {
            return this.hairID;
        }

        public void SetHairID(byte hairID)
        {
            this.hairID = hairID;
        }

        public Color GetHairColor()
        {
            return this.hairColor;
        }

        public void SetHairColor(Color hairColor)
        {
            this.hairColor = hairColor;
        }

        public string GetHairModelName()
        {
            return "hair_" + hairID;
        }

        public byte GetEyesID()
        {
            return this.eyesID;
        }

        public void SetEyesID(byte eyesID)
        {
            this.eyesID = eyesID;
        }

        public Color GetEyesColor()
        {
            return this.eyesColor;
        }

        public void SetEyesColor(Color eyesColor)
        {
            this.eyesColor = eyesColor;
        }

        public string GetEyesModelName()
        {
            return "eyes_" + eyesID;
        }

        public Vector3 GetEyesOffset()
        {
            return new Vector3(0, -5, (-length / 2) / GetModelScale().Z + 0.91F);
        }

        public override Vector3 GetModelScale()
        {
            return new Vector3(0.06F, 0.06F, 0.06F);
        }

        public override bool RenderHealthBar()
        {
            return false;
        }

        public override float GetBaseSpeed()
        {
            return 0.02F;
        }

        public override float GetBaseJumpSpeed()
        {
            return 5.0F;
        }

        public override bool IsAlive()
        {
            return this.health > 0;
        }

        public override void UpdateBoundingBox()
        {
            base.UpdateBoundingBox();

            dropBoundingBox.Min = GetPosition() + new Vector3(-width * 1.2F, -height - 0.001F, -length * 1.2F);
            dropBoundingBox.Max = GetPosition() + new Vector3(width * 1.2F, 0, length * 1.2F);
        }

        public BoundingBox GetDropBoundingBox()
        {
            return this.dropBoundingBox;
        }

        public override Color GetMultiplyColor()
        {
            return new Color(skinColor.ToVector4() * colorMultiply.ToVector4());
        }

        public override string GetRendererName()
        {
            return "player_base";
        }

        public override EntityModelCollection GetModelCollection()
        {
            return EntityModelCollection.human;
        }
    }
}
