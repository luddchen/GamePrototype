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

        public void init( List<SpatialObject> collection, SpatialObject center ) {
            this.objects.Clear();
            float dist;
            foreach ( SpatialObject obj in collection ) {
                if ( obj is Skybox ) {
                    dist = float.MaxValue;
                } else if ( obj is Grid ) {
                    dist = 0.1f;
                } else {
                    dist = Vector3.Distance( obj.globalPosition, center.globalPosition );
                    float dot = Vector3.Dot( obj.globalPosition - center.globalPosition, center.rotation.Forward );
                    if ( dot < 0 ) {
                        dist *= -1;
                    }
                }
                this.objects.Add( new Tuple<SpatialObject, float>( obj, dist ) );
            }

            this.objects.Sort( CompareByDist );
        }

        public void Draw( Camera camera ) {
            foreach ( Tuple<SpatialObject, float> t in this.objects ) {
                if ( t.Item2 > 0 ) {
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
