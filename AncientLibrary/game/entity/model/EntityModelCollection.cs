using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.model
{
    public class EntityModelCollection
    {
        /* Entity */
        public static EntityModelCollection human = new EntityModelCollection("human_body", 0.65F, 1.5F, 0.65F).SetSittingModel("human_sitting", 0.65F, 1.26F, 0.65F)
            .SetWalkingModel("human_walking", 0.65F, 1.5F, 0.65F, 5, 19);
        public static EntityModelCollection tortoise = new EntityModelCollection("tortoise", 1.4f, 1.1f, 1.4f).SetSleepingModel("tortoise_sleeping", 1.4f, 0.9F, 1.4f)
            .SetWalkingModel("tortoise_walking", 1.4f, 1.1f, 1.4f, 5, 20);
        public static EntityModelCollection slime = new EntityModelCollection("slime", 0.85F, 0.85F, 0.85F);
        public static EntityModelCollection projectileBasic = new EntityModelCollection("voxel", 0.25F, 0.25F, 0.25F);
        public static EntityModelCollection nymu = new EntityModelCollection("nymu", 1, 16 / 19F, 1);
        public static EntityModelCollection bee = new EntityModelCollection("bee", 0.375F, 0.375F, 0.375F);

        /* World */
        public static EntityModelCollection portal = new EntityModelCollection("portal", 4, 4.5F, 0.75F);

        /* Skill */
        public static EntityModelCollection flashJump = new EntityModelCollection("flash_jump", 0, 0, 0);

        private EntityModel standing;
        private EntityModel sitting;
        private EntityModel sleeping;
        private EntityModelAnimation walking;

        public EntityModelCollection(string modelName, float width, float height, float length)
        {
            this.standing = CreateModel(modelName, width, height, length);
        }

        private static EntityModel CreateModel(string modelName, float width, float height, float length)
        {
            return new EntityModel(modelName, width, height, length);
        }

        public EntityModel GetStandingModel()
        {
            return this.standing;
        }

        protected EntityModelCollection SetStandingModel(string modelName, float width, float height, float length)
        {
            this.standing = CreateModel(modelName, width, height, length);
            return this;
        }

        public EntityModel GetSittingModel()
        {
            return this.sitting;
        }

        protected EntityModelCollection SetSittingModel(string modelName, float width, float height, float length)
        {
            this.sitting = CreateModel(modelName, width, height, length);
            return this;
        }

        public EntityModel GetSleepingModel()
        {
            return this.sleeping;
        }

        protected EntityModelCollection SetSleepingModel(string modelName, float width, float height, float length)
        {
            this.sleeping = CreateModel(modelName, width, height, length);
            return this;
        }

        public EntityModelAnimation GetWalkingAnimationModel()
        {
            return this.walking;
        }

        protected EntityModelCollection SetWalkingModel(string modelName, float width, float height, float length, int animationCount, int animationTickRate)
        {
            this.walking = new EntityModelAnimation(modelName, width, height, length, animationCount, animationTickRate);
            return this;
        }
    }
}
