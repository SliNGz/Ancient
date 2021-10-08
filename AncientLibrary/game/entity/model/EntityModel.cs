using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.model
{
    public class EntityModel
    {
        private string modelName;

        private float width;
        private float height;
        private float length;

        public EntityModel(string modelName, float width, float height, float length)
        {
            this.modelName = modelName;
            this.width = width;
            this.height = height;
            this.length = length;
        }

        public string GetModelName()
        {
            return this.modelName;
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
    }
}
