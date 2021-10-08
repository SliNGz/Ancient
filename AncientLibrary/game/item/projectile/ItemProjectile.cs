using ancientlib.game.classes;
using ancientlib.game.entity;
using ancientlib.game.entity.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.item.projectile
{
    public abstract class ItemProjectile : ItemWeapon
    {
        protected float speed;
        protected float gravity;

        public ItemProjectile(string name, Class _class, int damage, float speed, float width, float height, float length, float gravity) : base(name, _class, damage, 0)
        {
            this.maxItemStack = 64;
            this.speed = speed;

            this.gravity = gravity;
            this.modelCollection = new EntityModelCollection(GetModelName(), width, height, length);
        }

        public abstract Type GetWeaponType();

        public float GetSpeed()
        {
            return this.speed;
        }

        public float GetGravity()
        {
            return this.gravity;
        }
    }
}