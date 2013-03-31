using System;
using Battlestation_Antares.Control.AI;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// a dangerous turret
    /// </summary>
    class Turret : TactileSpatialObject {
        public AI ai;

        private Random random;

        private int timeout;

        private int beamCooldown;

        /// <summary>
        /// create a new turret and insert into the world
        /// </summary>
        /// <param name="position">position</param>
        public Turret( Vector3 position ) : base( "Turret//turret", position ) {
            this.random = new Random( (int)position.X );
            this.timeout = this.random.Next( 120 ) + 60;
            this.beamCooldown = 30;

            this.attributes = new SpatialObjectAttributes( Antares.content.Load<SpatialObjectAttributes>( "Attributes//Turret" ) );
        }

        protected override void _initMiniMapIcon() {
            this.miniMapIcon.Texture = Antares.content.Load<Texture2D>( "Models//Turret//turret_2d" );
            this.miniMapIcon.color = MiniMap.FRIEND_COLOR;
        }


        /// <summary>
        /// update the turret
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );

            if ( this.ai != null ) {
                this.ai.targetObjects = Antares.world.AllTactileObjects;

                this.ai.ThreadPoolCallback( null );

                float maxValue = 0;
                TactileSpatialObject target = null;

                for ( int i = 0; i < this.ai.targetResults.Count; i++ ) {
                    if ( maxValue < this.ai.targetResults[i] ) {
                        maxValue = this.ai.targetResults[i];
                        target = this.ai.targetObjects[i];
                    }
                }

                if ( target != null ) {
                    Vector3 rot = Tools.Tools.GetRotation( target.globalPosition - this.globalPosition, this.rotation );

                    if ( rot.Z < this.attributes.EngineYaw.CurrentVelocity ) {
                        InjectControl( Control.Control.YAW_RIGHT );
                    }

                    if ( rot.Z > this.attributes.EngineYaw.CurrentVelocity ) {
                        InjectControl( Control.Control.YAW_LEFT );
                    }

                    if ( rot.X < this.attributes.EnginePitch.CurrentVelocity ) {
                        InjectControl( Control.Control.PITCH_DOWN );
                    }

                    if ( rot.X > this.attributes.EnginePitch.CurrentVelocity ) {
                        InjectControl( Control.Control.PITCH_UP );
                    }

                    if ( ( Math.Abs( rot.X ) < Math.PI / 90 && Math.Abs( rot.Z ) < Math.PI / 90 ) ) {
                        this.beamCooldown--;
                        if ( this.beamCooldown < 0 ) {
                            this.beamCooldown = 30;
                            Laser laser = new Laser( this, 0.0f, 0.0f );
                        }
                    }
                }

            }

        }


        public override string ToString() {
            return "Turret";
        }

    }

}
