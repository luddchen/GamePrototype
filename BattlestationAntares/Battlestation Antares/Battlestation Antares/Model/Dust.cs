﻿using Battlestation_Antares.Tools;
using Microsoft.Xna.Framework;
using System;
using Battlestation_Antaris.Model;

namespace Battlestation_Antares.Model {

    class Dust : SpatialObject {
        const int MAX_PARENT_DIST = 1000;

        TactileSpatialObject parent;

        public Dust( TactileSpatialObject parent )
            : base( "Dust", position: parent.globalPosition ) {
            this.parent = parent;
            this.scale.X = 0.5f;
            this.scale.Y = 0.5f;
            setRandomPos();
        }

        public override void Update( GameTime gameTime ) { 
            float distToParent = Vector3.Distance( parent.globalPosition, this.globalPosition );
            if ( distToParent > ( MAX_PARENT_DIST ) ) {
                setRandomPos();
            }
            this.rotation = this.parent.rotation;
            this.scale.Z = Math.Abs( this.parent.attributes.Engine.CurrentVelocity ) + 1;
        }

        private void setRandomPos() {
            // create new dust in front of ship (depending on parents moving direction)
            Vector3 localOffset = this.parent.rotation.Forward * MAX_PARENT_DIST * (float)( RandomGen.random.NextDouble() * 2 - 0.9f );

            // move random right/left (depending on parents moving direction)
            localOffset += this.parent.rotation.Right * (float)( RandomGen.random.NextDouble() - 0.5f ) * MAX_PARENT_DIST * 0.4f;

            // move random up/down (depending on parents moving direction)
            localOffset += this.parent.rotation.Up * (float)( RandomGen.random.NextDouble() - 0.5f ) * MAX_PARENT_DIST * 0.4f;


            this.globalPosition = parent.globalPosition + localOffset;
        }


        public override string ToString() {
            return "Dust";
        }
    }
}
