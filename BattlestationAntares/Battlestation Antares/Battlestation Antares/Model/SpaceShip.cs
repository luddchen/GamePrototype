using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using SpatialObjectAttributesLibrary;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.Tools;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// the players space ship
    /// </summary>
    public class SpaceShip : SpatialObjectOld {

        public SpatialObjectOld target;

        private float[] laserOffsets;

        private int laserIndex = 0;

        /// <summary>
        /// create a new space ship within the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpaceShip( Vector3 position, String modelName, ContentManager content, WorldModel world )
            : base( position, modelName, content, world ) {
            this.attributes = new SpatialObjectAttributes( content.Load<SpatialObjectAttributes>( "Attributes//SpaceShip" ) );
            this.miniMapIcon.Texture = content.Load<Texture2D>( "Models//SpaceShip//spaceship_2d" );
            this.miniMapIcon.color = MiniMap.SPECIAL_COLOR;
            //this.miniMapIcon.scale = 2.0f;
            this.laserOffsets = new float[2] { -4.0f, 4.0f };

            this.attributes.Engine.ZeroBarrier = true;
        }


        /// <summary>
        /// update the space ship
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );
            this.attributes.Shield.Regenerate();

            this.attributes.Laser.CurrentHeat -= this.attributes.Laser.HeatRegeneration;
            if ( this.attributes.Laser.CurrentHeat < 0 ) {
                this.attributes.Laser.CurrentHeat = 0;
            }

            if ( this.attributes.Laser.CurrentReloadTime > 0 ) {
                this.attributes.Laser.CurrentReloadTime--;
            }

            if ( this.attributes.Missile.CurrentReloadTime > 0 ) {
                this.attributes.Missile.CurrentReloadTime--;
            }
        }


        public override void InjectControl( Control.Control control ) {
            base.InjectControl( control );

            if ( control == Control.Control.FIRE_LASER && this.attributes.Laser.CurrentReloadTime <= 0
                && this.attributes.Laser.CurrentHeat < this.attributes.Laser.HeatUntilCooldown ) {
                this.attributes.Laser.CurrentHeat += this.attributes.Laser.HeatProduction;
                this.attributes.Laser.CurrentReloadTime = this.attributes.Laser.ReloadTime;
                Laser laser = new Laser( this, -2.0f, this.laserOffsets[this.laserIndex], this.world.game.Content, this.world );
                this.laserIndex++;
                if ( this.laserIndex >= this.laserOffsets.Length ) {
                    this.laserIndex = 0;
                }
            }

            if ( control == Control.Control.FIRE_MISSILE ) {
                if ( this.attributes.Missile.CurrentReloadTime <= 0 ) {
                    Missile missile = new Missile( this, -2.0f, this.world.game.Content, this.world );
                    this.attributes.Missile.CurrentReloadTime = this.attributes.Missile.ReloadTime;
                }
            }

            if ( control == Control.Control.TARGET_NEXT_ENEMY ) {
                //Console.WriteLine("Blubb!");
                float testDist = float.MaxValue;
                this.target = this.world.octree.CastRay( new Ray( this.globalPosition, this.rotation.Forward ), 1, ref testDist );
            }

        }

        public override void addDebugOutput() {
            Antares.debugViewer.Add( new DebugElement( this, "Speed", delegate( Object obj ) {
                return String.Format( "{0:F2}", ( obj as SpaceShip ).attributes.Engine.CurrentVelocity );
            } ) );

            //Game1.debugViewer.Add(new DebugElement(this, "Yaw", delegate(Object obj)
            //{
            //    return String.Format("{0:F2}", (obj as SpaceShip).attributes.EngineYaw.CurrentVelocity * 100);
            //}));

            //Game1.debugViewer.Add(new DebugElement(this, "Pitch", delegate(Object obj)
            //{
            //    return String.Format("{0:F2}", (obj as SpaceShip).attributes.EnginePitch.CurrentVelocity * 100);
            //}));

            //Game1.debugViewer.Add(new DebugElement(this, "Roll", delegate(Object obj)
            //{
            //    return String.Format("{0:F2}", (obj as SpaceShip).attributes.EngineRoll.CurrentVelocity * 100);
            //}));

            Antares.debugViewer.Add( new DebugElement( this, "Distance", delegate( Object obj ) {
                return String.Format( "{0:F0}", ( obj as SpaceShip ).globalPosition.Length() );
            } ) );

        }


        public override void onHit( float damage ) {
            if ( this.attributes.Shield.ApplyDamage( damage ) ) {
                this.attributes.Hull.ApplyDamage( damage );
            }
        }

        public override string ToString() {
            return "SpaceShip";
        }

    }

}
