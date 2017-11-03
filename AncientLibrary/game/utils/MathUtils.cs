using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public class MathUtils
    {
        public static Vector3 GetRotationFromPosition(double xPos, double yPos, double zPos, double xLookAt, double yLookAt, double zLookAt)
        {
            xLookAt -= xPos;
            yLookAt -= yPos;
            zLookAt -= zPos;

            float yaw = (float)(Math.Atan2(xLookAt, zLookAt) - Math.PI);
            float pitch = (float)Math.Atan2(yLookAt, (float)Math.Sqrt(xLookAt * xLookAt + zLookAt * zLookAt));

            return new Vector3(yaw, pitch, 0);
        }

        public static float CurveAngle(float from, float to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            Vector2 fromVector = new Vector2((float)Math.Cos(from), (float)Math.Sin(from));
            Vector2 toVector = new Vector2((float)Math.Cos(to), (float)Math.Sin(to));

            Vector2 currentVector = Slerp(fromVector, toVector, step);

            return (float)Math.Atan2(currentVector.Y, currentVector.X);
        }

        public static Vector2 Slerp(Vector2 from, Vector2 to, float step)
        {
            if (step == 0) return from;
            if (from == to || step == 1) return to;

            double theta = Math.Acos(Vector2.Dot(from, to));
            if (theta == 0) return to;

            double sinTheta = Math.Sin(theta);

            return (float)(Math.Sin((1 - step) * theta) / sinTheta) * from + (float)(Math.Sin(step * theta) / sinTheta) * to;
        }

        public static float BilinearInterpolation(float bottomLeft, float topLeft, float bottomRight, float topRight,
                            float xMin, float xMax,
                            float zMin, float zMax,
                            float x, float z)
        {
            float width = xMax - xMin,
                    height = zMax - zMin,

                    xDistanceToMaxValue = xMax - x,
                    zDistanceToMaxValue = zMax - z,

                    xDistanceToMinValue = x - xMin,
                    zDistanceToMinValue = z - zMin;

            return 1F / (width * height) *
            (
                bottomLeft * xDistanceToMaxValue * zDistanceToMaxValue +
                bottomRight * xDistanceToMinValue * zDistanceToMaxValue +
                topLeft * xDistanceToMaxValue * zDistanceToMinValue +
                topRight * xDistanceToMinValue * zDistanceToMinValue
            );
        }
    }
}
