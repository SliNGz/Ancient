using ancient.game.entity;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.utils
{
    public static class BBExtensions
    {
        public static Vector3 GetCenter(this BoundingBox boundingBox)
        {
            Vector3 dimensions = boundingBox.Max - boundingBox.Min;
            return boundingBox.Max - dimensions / 2;
        }

        public static Vector3 GetPenetration(this BoundingBox box, BoundingBox box2)
        {
            float x = Math.Min(box.Max.X - box2.Min.X, box2.Max.X - box.Min.X);
            float y = Math.Min(box.Max.Y - box2.Min.Y, box2.Max.Y - box.Min.Y);
            float z = Math.Min(box.Max.Z - box2.Min.Z, box2.Max.Z - box.Min.Z);

            return new Vector3(x, y, z);
        }
    }
}
