using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.utils
{
    public class DirectionHelper
    {
        public enum Direction
        {
            NORTH,
            SOUTH,
            WEST,
            EAST
        }

        public static Direction GetDirection(float yaw, float pitch)
        {
            if (MathHelper.ToDegrees(yaw) <= 45 && MathHelper.ToDegrees(yaw) >= -45)
                return Direction.NORTH;
            else if (MathHelper.ToDegrees(yaw) >= 135 && MathHelper.ToDegrees(yaw) <= 180 ||
                MathHelper.ToDegrees(yaw) >= -180 && MathHelper.ToDegrees(yaw) <= -135)
                return Direction.SOUTH;
            else if (MathHelper.ToDegrees(yaw) > 45 && MathHelper.ToDegrees(yaw) < 135)
                return Direction.WEST;
            else
                return Direction.EAST;
        }
    }
}