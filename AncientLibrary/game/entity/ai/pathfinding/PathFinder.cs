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

        private Stopwatch watch;

        public PathFinder(Entity entity)
        {
            this.path = new List<PathNode>();
            this.entity = entity;
            watch = new Stopwatch();
        }

        public void Update(GameTime gameTime)
        {
            if (HasPath())
            {
                Vector3 movement = Vector3.Forward;

                if (currentPathNode.GetY() > entity.GetFootPosition().Y)
                {
                    if (entity is EntityLiving)
                    {
                        if (entity is EntityFlying)
                            movement += Vector3.Up;
                        else
                            ((EntityLiving)entity).Jump();
                    }
                    else
                        movement += Vector3.Up;
                }
                else if (currentPathNode.GetY() < entity.GetFootPosition().Y)
                {
                    if (entity is EntityFlying)
                        movement += Vector3.Down;
                }

                entity.SetMovement(movement);

                UpdatePath(gameTime);
            }
        }

        private void UpdatePath(GameTime gameTime)
        {
            if (entity is EntityLiving)
                ((EntityLiving)entity).SetLookAt(currentPathNode.GetX() + 0.5F, currentPathNode.GetY(), currentPathNode.GetZ() + 0.5F);

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

        public List<PathNode> GetPathTo(int x, int y, int z)
        {
            watch.Reset();
            watch.Start();

            SortedSet<PathNode> openSet = new SortedSet<PathNode>();
            List<PathNode> closedSet = new List<PathNode>();

            Vector3 center = entity.GetBoundingBox().GetCenter();
            PathNode startNode = new PathNode((int)Math.Round(center.X), (int)(entity.GetFootPosition().Y), (int)Math.Round(center.Z));
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

                if (watch.Elapsed.TotalSeconds >= 3)
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
            entity.GetWorld().GetPathFinderManager().EnqueuePath(this, x, y, z);
        }

        public void SetPathUnsafe(int x, int y, int z)
        {
            this.path = GetPathTo(x, y, z);
            this.pathCount = 0;

            if (HasPath())
                this.currentPathNode = path[pathCount];
        }

        public void SetPath(List<PathNode> path)
        {
            this.path = path;
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
