using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antares.Model {
    class SpatialObjectFactory {
        private static ContentManager content;
        private static WorldModel world;

        public static void initializeFactory( ContentManager content, WorldModel world ) {
            SpatialObjectFactory.content = content;
            SpatialObjectFactory.world = world;
        }

        public static SpatialObjectOld buildSpatialObject( Type spatialObjectType ) {
            SpatialObjectOld newObj;

            if ( spatialObjectType.Equals( typeof( Battlestation_Antares.Model.Radar ) ) ) {
                newObj = new Radar( Vector3.Zero, content, world );
            } else if ( spatialObjectType.Equals( typeof( Battlestation_Antares.Model.Turret ) ) ) {
                newObj = new Turret( Vector3.Zero, content, world );
            } else {
                throw new ArgumentException( "Factory: Unknown Type" );
            }
            return newObj;
        }
    }
}
