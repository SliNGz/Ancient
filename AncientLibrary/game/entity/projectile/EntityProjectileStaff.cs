using ancient.game.entity.player;
using ancient.game.world;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancientlib.game.utils;
using System.IO;
using ancientlib.game.entity.model;

namespace ancientlib.game.entity.projectile
{
    class EntityProjectileStaff : EntityProjectile
    {
        public EntityProjectileStaff(World world) : base(world)
        { }

        public EntityProjectileStaff(World world, EntityLiving shooter) : base(world, shooter, null)
        {
            SetVelocity(Vector3.Zero);

            this.gravity = 0;

            this.lifeSpan = Utils.TicksInSecond * 7;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            float hue = 175 + ((float)Math.Sin((ticksExisted / 128F)) * 25);
            this.colorMultiply = Utils.HSVToRGB(hue, 1, 1);
        }

        public override float GetBaseSpeed()
        {
            return 15;
        }

        public override EntityModelCollection GetModelCollection()
        {
            return EntityModelCollection.projectileBasic;
        }

        public override string GetEntityName()
        {
            return "staff_projectile";
        }

        public override void Read(BinaryReader reader)
        {
            base.Read(reader);
            this.gravity = 0;
            SetVelocity(Vector3.Zero);
        }
    }
}
