using ancient.game.entity;
using ancient.game.world;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancientlib.game.entity.ai.pathfinding
{
    public class PathNode : IComparable
    {
        private int x;
        private int y;
        private int z;

        private PathNode parent;

        private float gCost;
        private float hCost;

        public PathNode(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int GetX()
        {
            return this.x;
        }

        public int GetY()
        {
            return this.y;
        }

        public int GetZ()
        {
            return this.z;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(x, y, z);
        }

        public float GetHCost()
        {
            return this.hCost;
        }

        public void SetHCost(float hCost)
        {
            this.hCost = hCost;
        }

        public float GetGCost()
        {
            return this.gCost;
        }

        public void SetGCost(float gCost)
        {
            this.gCost = gCost;
        }

        public float GetFCost()
        {
            return this.gCost + this.hCost;
        }

        public PathNode GetParent()
        {
            return this.parent;
        }

        public void SetParent(PathNode parent)
        {
            this.parent = parent;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PathNode))
                return false;

            PathNode node = (PathNode)obj;
            return x == node.GetX() && y == node.GetY() && z == node.GetZ();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool IsWalkable(Entity entity)
        {
            return !entity.GetWorld().IsSolidAt(x, y, z);
        }

        public List<PathNode> GetNeighbors()
        {
            List<PathNode> neighbors = new List<PathNode>();

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    for (int k = -1; k <= 1; k++)
                    {
                        if (i == 0 && j == 0 && k == 0)
                            continue;

                        neighbors.Add(new PathNode(x + i, y + j, z + k));
                    }
                }
            }

            return neighbors;
        }

        public float GetDistanceTo(PathNode node)
        {
            return Vector3.Distance(GetPosition(), node.GetPosition());
        }

        public override string ToString()
        {
            return "X: " + this.x + ", Y: " + this.y + ", Z: " + this.z;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is PathNode))
                return -999;

            PathNode node = (PathNode)obj;

            if (GetFCost() > node.GetFCost())
                return 1;
            else if (GetFCost() < node.GetFCost())
                return -1;
            else
            {
                if (hCost < node.GetHCost())
                    return -1;
                else if (hCost > node.GetHCost())
                    return 1;
                else
                    return 0;
            }
        }
    }
}
