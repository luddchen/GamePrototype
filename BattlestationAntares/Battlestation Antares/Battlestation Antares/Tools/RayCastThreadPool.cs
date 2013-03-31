using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antares.Model;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;

namespace Battlestation_Antares.Tools {

    class RayCaster {

        public TactileSpatialObject source;

        public TactileSpatialObject target;

        public float distance;

        private DynamicOctree<TactileSpatialObject> octree;

        private RayCastThreadPool pool;


        public RayCaster( TactileSpatialObject source, DynamicOctree<TactileSpatialObject> octree, RayCastThreadPool pool ) {
            this.source = source;
            this.octree = octree;
            this.pool = pool;
            this.distance = float.MaxValue;
        }

        public void ThreadPoolCallback( Object threadContext ) {
            //if ( this.source is SpatialObjectOld ) {
                this.target = 
                    this.octree.CastRay( 
                        new Ray( this.source.globalPosition, this.source.rotation.Forward ), 
                        this.source.bounding.Radius + 0.1f, ref this.distance );
            //}
            if ( this.target != null || this.distance < 5000 ) {
                Interlocked.Increment( ref this.pool.hits );
            }

            if ( Interlocked.Decrement( ref this.pool.numberOfTasks ) == 0 ) {
                this.pool.doneEvent.Set();
            }
        }

    }


    class RayCastThreadPool {

        private WorldModel world;

        private RayCaster[] caster;

        public ManualResetEvent doneEvent;

        public int numberOfTasks;

        public int hits;

        public RayCastThreadPool( WorldModel world ) {
            this.world = world;
        }

        public void StartRayCasting() {
            //Console.Out.Write("start Raycasting .. ");
            this.numberOfTasks = this.world.AllTurrets.Count;
            this.caster = new RayCaster[this.numberOfTasks];
            this.doneEvent = new ManualResetEvent( false );
            this.hits = 0;

            int counter = 0;

            foreach ( Turret item in this.world.AllTurrets ) {
                RayCaster rayCaster = new RayCaster( item, this.world.octree, this );
                caster[counter] = rayCaster;
                ThreadPool.QueueUserWorkItem( rayCaster.ThreadPoolCallback, null );
                counter++;
            }

            doneEvent.WaitOne();

            //Console.Out.WriteLine(" finish -> " + this.hits + " hits");
        }

    }


}
