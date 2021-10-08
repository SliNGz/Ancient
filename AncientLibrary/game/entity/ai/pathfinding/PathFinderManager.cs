using ancientlib.game.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ancientlib.game.entity.ai.pathfinding
{
    public class PathFinderManager
    {
        public static int nextPathID;
        private Queue<PathStruct> pathQueue;

        public PathFinderManager()
        {
            this.pathQueue = new Queue<PathStruct>();
            ThreadUtils.CreateThread("Path Finder " + nextPathID, new ThreadStart(Update), true).Start();
            nextPathID++;
        }

        public void Update()
        {
            while (true)
            {
                if (pathQueue.Count > 0)
                {
                    PathStruct path = pathQueue.Dequeue();

                    path.PathFinder.SetPathUnsafe(path.X, path.Y, path.Z);
                }

                Thread.Sleep(TimeSpan.FromSeconds(1 / 16F));
            }
        }

        public void EnqueuePath(PathFinder pathFinder, int x, int y, int z)
        {
            pathQueue.Enqueue(new PathStruct(pathFinder, x, y, z));
        }
    }

    struct PathStruct
    {
        public PathFinder PathFinder;
        public int X;
        public int Y;
        public int Z;

        public PathStruct(PathFinder pathFinder, int x, int y, int z)
        {
            this.PathFinder = pathFinder;
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
    }
}
