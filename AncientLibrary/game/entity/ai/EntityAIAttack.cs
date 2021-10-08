using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using ancient.game.entity.player;
using ancientlib.game.utils;

namespace ancientlib.game.entity.ai
{
    class EntityAIAttack : EntityAI
    {
        private EntityLiving living;

        private EntityLiving target;
        private float distance;

        private float lookRange;
        private float chaseRange;
        private float attackRange;

        private int attackCooldown;
        private int attackTimer;

        public EntityAIAttack(EntityLiving living, int priority, float lookRange, float chaseRange, float attackRange, int attackCooldown) : base(priority)
        {
            this.living = living;
            this.lookRange = lookRange;
            this.chaseRange = chaseRange;
            this.attackRange = attackRange;
            this.attackCooldown = attackCooldown;
        }

        public override void Execute()
        {
            attackTimer = 0;
        }

        public override bool ShouldExecute()
        {
            if (IsTargetInRange())
                return false;

            if (living.IsAttacked())
            {
                this.target = living.GetLastAttacker();

                if (IsTargetInRange())
                    return true;
            }

            if (living.IsHostile())
            {
                this.target = living.GetNearestLiving(lookRange);

                if (this.target == null)
                    return false;

                return true;
            }

            return false;
        }

        public override bool ShouldUpdate()
        {
            return target != null && distance <= lookRange;
        }

        public override void Stop()
        {
            target = null;
            attackTimer = 0;
        }

        public override void Update(GameTime gameTime)
        {
            attackTimer--;
            distance = Vector3.Distance(living.GetPosition(), this.target.GetPosition());

            living.SetLookAt(target);

            if (distance <= chaseRange)
                living.SetMovement(Vector3.Forward);

            if (distance <= attackRange && attackTimer <= 0)
            {
                target.Damage(new AttackInfo(living, living.GetDamage()));
                attackTimer = attackCooldown;
            }
        }

        private bool IsTargetInRange()
        {
            return this.target != null && (distance = Vector3.Distance(living.GetPosition(), this.target.GetPosition())) <= lookRange;
        }
    }
}
