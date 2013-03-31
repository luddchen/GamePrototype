﻿using System;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antares.Model {
    class SpatialObjectFactory {
        private static ContentManager content;
        private static WorldModel world;

        public static void initializeFactory( ContentManager content, WorldModel world ) {
            SpatialObjectFactory.content = content;
            SpatialObjectFactory.world = world;
        }

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
