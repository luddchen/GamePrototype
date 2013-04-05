using System;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antares.Model {

    class SpatialObjectFactory {

        public static SpatialObject buildSpatialObject( Type spatialObjectType ) {
            SpatialObject newObj;

            if ( spatialObjectType.Equals( typeof( Battlestation_Antares.Model.Radar ) ) ) {
                newObj = new Radar( Vector3.Zero );
            } else if ( spatialObjectType.Equals( typeof( Battlestation_Antares.Model.Turret ) ) ) {
                newObj = new Turret( Vector3.Zero );
            } else {
                throw new ArgumentException( "Factory: Unknown Type" );
            }
            return newObj;
        }
    }

}
