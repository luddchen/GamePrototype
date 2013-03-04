using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Tools
{

    public class RayCaster
    {

        public SpatialObject source;

        public SpatialObject target;

        public float distance;

        private DynamicOctree<SpatialObject> octree;

        private RayCastThreadPool pool;


        public RayCaster(SpatialObject source, DynamicOctree<SpatialObject> octree, RayCastThreadPool pool)
        {
            this.source = source;
            this.octree = octree;
            this.pool = pool;
            this.distance = float.MaxValue;
        }

        public void ThreadPoolCallback(Object threadContext)
        {
            this.target = this.octree.CastRay(new Ray(this.source.globalPosition, this.source.rotation.Forward), this.source.bounding.Radius + 0.1f, ref this.distance);

            if (this.target != null || this.distance < 5000)
            {
                Interlocked.Increment(ref this.pool.hits);
            }

            if (Interlocked.Decrement(ref this.pool.numberOfTasks) == 0)
            {
                this.pool.doneEvent.Set();
            }
        }

    }


    public class RayCastThreadPool
    {

        private WorldModel world;

        private RayCaster[] caster;

        public ManualResetEvent doneEvent;

        public int numberOfTasks;

        public int hits;

        public RayCastThreadPool(WorldModel world)
        {
            this.world = world;
        }

        public void StartRayCasting()
        {
            //Console.Out.Write("start Raycasting .. ");
            this.caster = new RayCaster[this.world.allTurrets.Count];
            this.numberOfTasks = this.world.allTurrets.Count;
            this.doneEvent = new ManualResetEvent(false);
            this.hits = 0;

            int counter = 0;

            foreach (SpatialObject item in this.world.allTurrets)
            {
                RayCaster rayCaster = new RayCaster(item, this.world.treeTest, this);
                caster[counter] = rayCaster;
                ThreadPool.QueueUserWorkItem(rayCaster.ThreadPoolCallback, null);
                counter++;
            }

            doneEvent.WaitOne();

            //Console.Out.WriteLine(" finish -> " + this.hits + " hits");
        }

    }


}
