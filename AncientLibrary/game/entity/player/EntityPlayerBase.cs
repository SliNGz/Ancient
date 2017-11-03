using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ancient.game.world;
using Microsoft.Xna.Framework;

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

        public EntityPlayerBase(World world) : base(world)
        {
            SetDimensions(0.65F, 1.5F, 0.65F);

            this.skinColor = new Color(255, 218, 182);

            this.hairID = 0;
            this.hairColor = new Color(222, 222, 222);

            this.eyesID = 4;
            this.eyesColor = Color.DarkRed;

            this.interactWithEntities = false;
        }

        public override Vector3 GetEyePosition()
        {
            return this.GetPosition() + GetEyesOffset() * GetModelScale() * Vector3.UnitY;
        }

        public override string GetModelName()
        {
            return "human";
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

        public override double GetBaseSpeed()
        {
            return 2.2;
        }

        public override double GetBaseJumpSpeed()
        {
            return 5.0;
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
    }
}
