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
        public SpaceShip( Vector3 position ) : base( "SpaceShip", position ) {
            this.laserOffsets = new float[2] { -4.0f, 4.0f };
            this.attributes.Engine.ZeroBarrier = true;
            this.attributes.SetUpdatePreferences( engineUpdate: true, weaponUpdate: true );
        }

        protected override void _initControlDictionary() {
            base._initControlDictionary();
            this.controlDictionary[Control.Command.TARGET_NEXT_ENEMY] = _targetNextEnemy;
            this.controlDictionary[Control.Command.FIRE_LASER] = _fireLaser;
            this.controlDictionary[Control.Command.FIRE_MISSILE] = _fireMissile;
        }

        protected override void _initMiniMapIcon() {
            this.miniMapIcon.color = MiniMap.SPECIAL_COLOR;
        }

        public override void addDebugOutput() {
            Antares.debugViewer.Add( new DebugElement( this, "Speed", delegate( Object obj ) {
                return String.Format( "{0:F2}", ( obj as SpaceShip ).attributes.Engine.CurrentVelocity );
            } ) );

            Antares.debugViewer.Add( new DebugElement( this, "Distance", delegate( Object obj ) {
                return String.Format( "{0:F0}", ( obj as SpaceShip ).globalPosition.Length() );
            } ) );

        }

        private void _fireLaser() {
            if ( this.attributes.Laser.Fire() ) {
                Laser laser = new Laser( this, -2.0f, this.laserOffsets[this.laserIndex] );
                this.laserIndex++;
                if ( this.laserIndex >= this.laserOffsets.Length ) {
                    this.laserIndex = 0;
                }
            }
        }

        private void _fireMissile() {
            if ( this.attributes.Missile.CurrentReloadTime <= 0 ) {
                Missile missile = new Missile( this, -2.0f );
                this.attributes.Missile.CurrentReloadTime = this.attributes.Missile.ReloadTime;
            }
        }

        private void _targetNextEnemy() {
            float testDist = float.MaxValue;
            this.target = Antares.world.octree.CastRay( new Ray( this.globalPosition, this.rotation.Forward ), 1, ref testDist );
        }

    }

}
