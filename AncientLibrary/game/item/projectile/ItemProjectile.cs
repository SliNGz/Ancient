using ancientlib.game.classes;
using ancientlib.game.entity;
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

        // ModelState - for the EntityProjectile creation based on item.
        private EntityModelState modelState;

        protected float gravity;

        public ItemProjectile(string name, Class _class, int damage, float speed, float width, float height, float length, float gravity) : base(name, _class, damage, 0)
        {
            this.maxItemStack = 64;
            this.speed = speed;

            modelState = new EntityModelState(GetModelName(), width, height, length);;

            this.gravity = gravity;
        }

        public abstract Type GetWeaponType();

        public float GetSpeed()
        {
            return this.speed;
        }

        public EntityModelState GetModelState()
        {
            return this.modelState;
        }

        public float GetWidth()
        {
            return this.modelState.GetWidth();
        }

        public float GetHeight()
        {
            return this.modelState.GetHeight();
        }

        public float GetLength()
        {
            return this.modelState.GetLength();
        }

        public float GetGravity()
        {
            return this.gravity;
        }
    }
}