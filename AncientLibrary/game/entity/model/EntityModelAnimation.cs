using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.model
{
    public class EntityModelAnimation : EntityModel
    {
        private int animationCount;
        private int animationTickRate;

        public EntityModelAnimation(string modelName, float width, float height, float length, int animationCount, int animationTickRate) : base(modelName, width, height, length)
        {
            this.animationCount = animationCount;
            this.animationTickRate = animationTickRate;
        }

        public int GetAnimationCount()
        {
            return this.animationCount;
        }

        public int GetAnimationTickRate()
        {
            return this.animationTickRate;
        }
    }
}
