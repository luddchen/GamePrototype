using System;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// the players space ship
    /// </summary>
    class SpaceShip : TactileSpatialObject {

        public TactileSpatialObject target;

        private float[] laserOffsets;

        private int laserIndex = 0;

        /// <summary>
        /// create a new space ship within the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpaceShip( Vector3 position ) : base( "SpaceShip//spaceship1_2", position ) {
            this.attributes = new SpatialObjectAttributes( Antares.content.Load<SpatialObjectAttributes>( "Attributes//SpaceShip" ) );
            this.laserOffsets = new float[2] { -4.0f, 4.0f };
            this.attributes.Engine.ZeroBarrier = true;
        }

        protected override void _initMiniMapIcon() {
            this.miniMapIcon.Texture = Antares.content.Load<Texture2D>( "Models//SpaceShip//spaceship_2d" );
            this.miniMapIcon.color = MiniMap.SPECIAL_COLOR;
        }


        /// <summary>
        /// update the space ship
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );

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
                Laser laser = new Laser( this, -2.0f, this.laserOffsets[this.laserIndex]);
                this.laserIndex++;
                if ( this.laserIndex >= this.laserOffsets.Length ) {
                    this.laserIndex = 0;
                }
            }

            if ( control == Control.Control.FIRE_MISSILE ) {
                if ( this.attributes.Missile.CurrentReloadTime <= 0 ) {
                    Missile missile = new Missile( this, -2.0f );
                    this.attributes.Missile.CurrentReloadTime = this.attributes.Missile.ReloadTime;
                }
            }

            if ( control == Control.Control.TARGET_NEXT_ENEMY ) {
                //Console.WriteLine("Blubb!");
                float testDist = float.MaxValue;
                this.target = Antares.world.octree.CastRay( new Ray( this.globalPosition, this.rotation.Forward ), 1, ref testDist );
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

        public override string ToString() {
            return "SpaceShip";
        }

    }

}
