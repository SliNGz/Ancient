using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ancientlib.game.entity.ai
{
    class EntityAIFollowOwner : EntityAI
    {
        private EntityPet pet;
        private float minDistance;
        private float teleportDistance;

        public EntityAIFollowOwner(EntityPet pet, int priority, float minDistance, float teleportDistance = 16) : base(priority)
        {
            this.pet = pet;
            this.minDistance = minDistance;
            this.teleportDistance = teleportDistance;
        }

        public override void Execute()
        {
            if (Vector3.Distance(pet.GetPosition(), pet.GetOwner().GetPosition()) >= teleportDistance)
                pet.SetPosition(pet.GetOwner().GetPosition() + new Vector3(pet.GetWidth() / 2, 0, pet.GetLength() / 2));
        }

        public override bool ShouldExecute()
        {
            if (!pet.HasOwner() || (pet is EntityMount && ((EntityMount)pet).IsRiddenByOwner()))
                return false;

            return Vector3.Distance(pet.GetFootPosition(), pet.GetOwner().GetFootPosition()) >= minDistance;
        }

        public override bool ShouldUpdate()
        {
            float distance = Vector3.Distance(pet.GetPosition(), pet.GetOwner().GetPosition());
            return distance > minDistance && distance < teleportDistance;
        }

        public override void Stop()
        { }

        public override void Update(GameTime gameTime)
        {
            Vector3 ownerPosition = pet.GetOwner().GetFootPosition();

            pet.SetLookAt(ownerPosition.X, ownerPosition.Y, ownerPosition.Z);
            pet.SetMovement(Vector3.Forward);

            if (ownerPosition.Y > pet.GetFootPosition().Y)
                pet.Jump();
        }
    }
}
