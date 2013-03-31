using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// the basis class for spatial objects
    /// </summary>
    class SpatialObjectOld : TactileSpatialObject {


        /// <summary>
        /// apply reset force if no active pitching
        /// </summary>
        private bool resetPitch = true;


        /// <summary>
        /// apply reset force if no active yawing
        /// </summary>
        private bool resetYaw = true;


        /// <summary>
        /// apply reset force if no active rolling
        /// </summary>
        private bool resetRoll = true;


        /// <summary>
        /// apply reset force if no active rolling
        /// </summary>
        private bool resetEngine = true;


        /// <summary>
        /// create a new spatial object within the world model
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpatialObjectOld( String modelName, Vector3 position ) : base(modelName, position: position ) {

            // dirty hack
            if ( !( this is Dust ) ) {
                this.miniMapIcon = new MiniMapIcon( null, this);
                this.miniMapIcon.Texture = Antares.content.Load<Texture2D>( "Models//SpaceShip//spaceship_2d" );
                this.miniMapIcon.color = MiniMap.FRIEND_COLOR;
                this.miniMapIcon.AddToWorld();
            }

            this.addDebugOutput();
        }


        /// <summary>
        /// update the spatial object
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( GameTime gameTime ) {
            ApplyMovement( gameTime );
            ApplyRotation( gameTime );
        }


        /// <summary>
        /// apply object movement
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected void ApplyMovement( GameTime gameTime ) {
            this.globalPosition += Vector3.Multiply( rotation.Forward, this.attributes.Engine.CurrentVelocity );

            if ( resetEngine ) {
                this.attributes.Engine.ApplyResetForce();
            }

            resetEngine = true;
        }


        /// <summary>
        /// apply engines rotation
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected void ApplyRotation( GameTime gameTime ) {

            // rotate
            if ( this.attributes.EngineYaw.CurrentVelocity != 0 ) {
                this.rotation = Tools.Tools.Yaw( this.rotation, this.attributes.EngineYaw.CurrentVelocity );
                this.rotationRepairCountdown--;
            }

            if ( this.attributes.EnginePitch.CurrentVelocity != 0 ) {
                this.rotation = Tools.Tools.Pitch( this.rotation, this.attributes.EnginePitch.CurrentVelocity );
                this.rotationRepairCountdown--;
            }

            if ( this.attributes.EngineRoll.CurrentVelocity != 0 ) {
                this.rotation = Tools.Tools.Roll( this.rotation, this.attributes.EngineRoll.CurrentVelocity );
                this.rotationRepairCountdown--;
            }


            // repair rotation matrix if necessary
            if ( this.rotationRepairCountdown < 0 ) {
                Tools.Tools.Repair( ref this.rotation );
                this.rotationRepairCountdown = SpatialObjectOld.MAX_ROTATION_UNTIL_REPAIR;
            }


            // apply reset forces if no active control
            if ( resetPitch ) {
                this.attributes.EnginePitch.ApplyResetForce();
            }

            if ( resetYaw ) {
                this.attributes.EngineYaw.ApplyResetForce();
            }

            if ( resetRoll ) {
                this.attributes.EngineRoll.ApplyResetForce();
            }

            resetPitch = true;
            resetYaw = true;
            resetRoll = true;
        }


        /// <summary>
        /// inject multiple control requests to this object
        /// </summary>
        /// <param name="controlSequence"></param>
        public override void InjectControl( List<Control.Control> controlSequence ) {
            foreach ( Control.Control control in controlSequence ) {
                InjectControl( control );
            }
        }


        /// <summary>
        /// inject a single control request to this object
        /// </summary>
        /// <param name="control"></param>
        public override void InjectControl( Control.Control control ) {
            // experimental control stuff

            switch ( control ) {
                case Control.Control.PITCH_UP:
                    this.attributes.EnginePitch.Accelerate();
                    resetPitch = false;
                    break;

                case Control.Control.PITCH_DOWN:
                    this.attributes.EnginePitch.Decelerate();
                    resetPitch = false;
                    break;

                case Control.Control.YAW_LEFT:
                    this.attributes.EngineYaw.Accelerate();
                    resetYaw = false;
                    break;

                case Control.Control.YAW_RIGHT:
                    this.attributes.EngineYaw.Decelerate();
                    resetYaw = false;
                    break;

                case Control.Control.ROLL_CLOCKWISE:
                    this.attributes.EngineRoll.Accelerate();
                    resetRoll = false;
                    break;

                case Control.Control.ROLL_ANTICLOCKWISE:
                    this.attributes.EngineRoll.Decelerate();
                    resetRoll = false;
                    break;

                case Control.Control.INCREASE_THROTTLE:
                    this.attributes.Engine.Accelerate();
                    resetEngine = false;
                    break;

                case Control.Control.DECREASE_THROTTLE:
                    this.attributes.Engine.Decelerate();
                    resetEngine = false;
                    break;

                case Control.Control.ZERO_THROTTLE:
                    this.attributes.Engine.CurrentVelocity = 0f;
                    resetEngine = false;
                    break;
            }

        }


        public virtual void addDebugOutput() {
        }

        public override string ToString() {
            return "SpatialObject";
        }

    }
}

