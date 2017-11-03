using ancient.game.entity;
using ancient.game.utils;
using ancientlib.game.utils;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancientlib.game.entity.ai.pathfinding
{
    public class PathFinder
    {
        private Entity entity;

        private List<PathNode> path;
        private int pathCount;

        private PathNode currentPathNode;

        public PathFinder(Entity entity)
        {
            this.path = new List<PathNode>();
            this.entity = entity;
        }

        public void Update(GameTime gameTime)
        {
            if (HasPath())
            {
                entity.AddMovement(Vector3.Forward);

                if (currentPathNode.GetY() - 1 > entity.GetFootPosition().Y)
                {
                    if (entity is EntityLiving)
                        ((EntityLiving)entity).Jump();
                    else
                        entity.AddMovement(Vector3.Up);
                }

                UpdatePath(gameTime);
            }
        }

        private void UpdatePath(GameTime gameTime)
        {
            if (entity is EntityLiving)
                ((EntityLiving)entity).SetLookAt(currentPathNode.GetX() + 0.5f, currentPathNode.GetY(), currentPathNode.GetZ() + 0.5f);

            if (HasReachedCurrentPathNode())
            {
                if (pathCount < path.Count - 1)
                {
                    pathCount++;
                    currentPathNode = path[pathCount];
                }
                else
                    ClearPath();
            }
        }

        private List<PathNode> GetPathTo(int x, int y, int z)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            SortedSet<PathNode> openSet = new SortedSet<PathNode>();
            List<PathNode> closedSet = new List<PathNode>();

            PathNode startNode = new PathNode((int)entity.GetX(), (int)(entity.GetFootPosition().Y + 1), (int)entity.GetZ());
            PathNode endNode = new PathNode(x, y, z);

            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                PathNode currentNode = openSet.Min;

                if (currentNode.Equals(endNode))
                    return ReconstructPath(startNode, currentNode);

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                foreach (PathNode neighbor in currentNode.GetNeighbors())
                {
                    float cost = currentNode.GetGCost() + currentNode.GetDistanceTo(neighbor);

                    if (!neighbor.IsWalkable(entity) || closedSet.Contains(neighbor))
                        continue;

                    if (!openSet.Contains(neighbor) || cost < neighbor.GetGCost())
                    {
                        neighbor.SetGCost(cost);
                        neighbor.SetHCost(neighbor.GetDistanceTo(endNode));
                        neighbor.SetParent(currentNode);

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }

                if (watch.Elapsed.TotalSeconds >= 0.5F)
                {
                    Console.WriteLine("Took too long to find path");
                    break;
                }
            }

            return new List<PathNode>();
        }

        private List<PathNode> ReconstructPath(PathNode startNode, PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            PathNode currentNode = endNode;

            while (!currentNode.Equals(startNode))
            {
                path.Insert(0, currentNode);
                currentNode = currentNode.GetParent();
            }

            return path;
        }

        public void SetPath(int x, int y, int z)
        {
            new Thread(() => SetPathTo(x, y, z)) { IsBackground = true }.Start();
        }

        private void SetPathTo(int x, int y, int z)
        {
            this.path = GetPathTo(x, y, z);
            this.pathCount = 0;

            if (HasPath())
                this.currentPathNode = path[pathCount];
        }

        public void ClearPath()
        {
            this.path.Clear();
            this.pathCount = 0;

            this.currentPathNode = null;
        }

        public List<PathNode> GetPath()
        {
            return this.path;
        }

        public bool HasPath()
        {
            return path.Count > 0;
        }

        private bool HasReachedCurrentPathNode()
        {
            return currentPathNode.GetX() >= Math.Floor(entity.GetBoundingBox().GetCenter().X) && currentPathNode.GetX() <= Math.Ceiling(entity.GetBoundingBox().GetCenter().X) &&
                    currentPathNode.GetZ() >= Math.Floor(entity.GetBoundingBox().GetCenter().Z) && currentPathNode.GetZ() <= Math.Ceiling(entity.GetBoundingBox().GetCenter().Z) &&
                    currentPathNode.GetY() >= Math.Floor(entity.GetBoundingBox().GetCenter().Y) && currentPathNode.GetY() <= Math.Ceiling(entity.GetBoundingBox().GetCenter().Y);
        }
    }
}
