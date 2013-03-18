﻿using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// a radar station
    /// </summary>
    public class Radar : SpatialObject {

        public List<SpatialObject> objectsInRange;


        /// <summary>
        /// create a new radar inside the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public Radar( Vector3 position, ContentManager content, WorldModel world )
            : base( position, "Models//Radar//radar_1", content, world ) {
            Random random = new Random( (int)position.X );

            int pitch = random.Next( 2 );
            this.attributes.EnginePitch.MaxVelocity = ( pitch == 0 ) ? -0.02f : 0.02f;
            this.attributes.EnginePitch.CurrentVelocity = ( pitch == 0 ) ? -0.02f : 0.02f;
            ;

            int yaw = random.Next( 2 );
            this.attributes.EngineYaw.MaxVelocity = ( yaw == 0 ) ? -0.02f : 0.02f;
            this.attributes.EngineYaw.CurrentVelocity = ( yaw == 0 ) ? -0.02f : 0.02f;

            int roll = random.Next( 2 );
            this.attributes.EngineYaw.MaxVelocity = ( roll == 0 ) ? -0.02f : 0.02f;
            this.attributes.EngineYaw.CurrentVelocity = ( roll == 0 ) ? -0.02f : 0.02f;

            this.attributes.Radar.Range = 500.0f;

            this.objectsInRange = new List<SpatialObject>();

            this.miniMapIcon.Texture = content.Load<Texture2D>( "Models//Radar//radar_2d" );
            this.miniMapIcon.color = MiniMap.FRIEND_COLOR;
            this.miniMapIcon.scale = 0.7f;
            this.miniMapIcon.updateRotation = false;
        }


        /// <summary>
        /// update the radar
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );

            this.objectsInRange.Clear();

            float distance;

            // objects
            foreach ( SpatialObject obj in this.world.allObjects ) {
                distance = Vector3.Distance( this.globalPosition, obj.globalPosition );

                if ( distance <= this.attributes.Radar.Range ) {
                    this.objectsInRange.Add( obj );
                }
            }

            //// weapons
            //foreach (SpatialObject obj in this.world.allWeapons)
            //{
            //    distance = Vector3.Distance(this.globalPosition, obj.globalPosition);

            //    if (distance <= this.attributes.Radar.Range)
            //    {
            //        this.objectsInRange.Add(obj);
            //    }
            //}
        }


        public override string ToString() {
            return "Radar";
        }

    }

}