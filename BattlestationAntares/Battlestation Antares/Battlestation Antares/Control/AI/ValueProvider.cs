using System;
using System.Collections.Generic;
using Battlestation_Antares.Model;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;

namespace Battlestation_Antares.Control.AI {

    class ValueProvider {

        public static float Distance( TactileSpatialObject source, TactileSpatialObject target, float maxDistance ) {
            float dist = ( target.globalPosition - source.globalPosition ).Length();
            dist = 1.0f - dist / maxDistance;

            return ( dist >= 0 ) ? dist : 0.0f;
        }


        public static float OrthogonalVelocity( TactileSpatialObject source, TactileSpatialObject target, float maxSpeed ) {
            Vector3 direction = source.globalPosition - target.globalPosition;
            float ortho = Vector3.Dot( direction, target.rotation.Forward );
            ortho = ortho * target.attributes.Engine.CurrentVelocity;
            ortho = ortho / maxSpeed;

            if ( ortho < -1 ) {
                ortho = -1;
            }
            if ( ortho > 1 ) {
                ortho = 1;
            }
            return ( ortho + 1.0f ) / 2;
        }


        public static float Rotation( TactileSpatialObject source, TactileSpatialObject target ) {
            Vector3 targetVector = target.globalPosition - source.globalPosition;
            Vector3 rot = Tools.Tools.GetRotation( targetVector, source.rotation );

            rot.X = (float)( Math.Abs( rot.X ) / Math.PI );
            rot.Z = (float)( Math.Abs( rot.Z ) / Math.PI );

            return 1 - ( rot.X * rot.Z );
        }


        public static float ShieldStatus( TactileSpatialObject target ) {
            return target.attributes.Shield.CurrentHealthPoints / target.attributes.Shield.MaxHealthPoints;
        }


        public static float HullStatus( TactileSpatialObject target ) {
            return target.attributes.Hull.CurrentHealthPoints / target.attributes.Hull.MaxHealthPoints;
        }
    }

}
