using ancientlib.game.classes;
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

        // Dimensions - for the EntityProjectile creation based on item.
        protected float width;
        protected float height;
        protected float length;

        protected float gravity;

        public ItemProjectile(string name, Class _class, int damage, float speed, float width, float height, float length, float gravity) : base(name, _class, damage, 0)
        {
            this.maxItemStack = 64;
            this.speed = speed;

            this.width = width;
            this.height = height;
            this.length = length;

            this.gravity = gravity;
        }

        public abstract Type GetWeaponType();

        public float GetSpeed()
        {
            return this.speed;
        }

        public float GetWidth()
        {
            return this.width;
        }

        public float GetHeight()
        {
            return this.height;
        }

        public float GetLength()
        {
            return this.length;
        }

        public float GetGravity()
        {
            return this.gravity;
        }
    }
}