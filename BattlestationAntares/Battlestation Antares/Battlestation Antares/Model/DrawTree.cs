using System;
using System.Collections.Generic;
using Battlestation_Antares.View;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;

namespace Battlestation_Antaris.Model {

    class DrawTree {

        protected List<Tuple<SpatialObject, float>> objects;

        public DrawTree() {
            this.objects = new List<Tuple<SpatialObject, float>>();
        }

        public void init( List<SpatialObject> collection, Camera camera ) {
            this.objects.Clear();
            float dist;
            foreach ( SpatialObject obj in collection ) {
                dist = Vector3.Distance( obj.globalPosition, camera.position );
                float dot = Vector3.Dot( obj.globalPosition - camera.position, camera.forward );
                if ( dot < 0 ) {
                    dist *= -1;
                }
                if ( obj is SpaceShip ) { // dirty hack for Dock / Undock -> SpaceStation in front of SpaceShip
                    dist += 100;
                }
                this.objects.Add( new Tuple<SpatialObject, float>( obj, dist ) );
            }

            this.objects.Sort( CompareByDist );
        }

        public void Draw( Camera camera ) {
            foreach ( Tuple<SpatialObject, float> t in this.objects ) {
                if ( t.Item2 > -100 ) {
                    t.Item1.Draw( camera );
                } else {
                    break;
                }
            }
        }

        private static int CompareByDist( Tuple<SpatialObject, float> x, Tuple<SpatialObject, float> y ) {
            float result = y.Item2 - x.Item2;
            if ( result < 0 ) {
                return -1;
            } else if ( result > 0 ) {
                return 1;
            } else {
                return 0;
            }
        }

    }

}
