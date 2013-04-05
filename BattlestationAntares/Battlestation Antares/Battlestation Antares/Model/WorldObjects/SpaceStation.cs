using System;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// the Antares space station
    /// </summary>
    class SpaceStation : TactileSpatialObject {

        public enum DockState {
            OPEN,
            CLOSED,
            OPENING,
            CLOSING,
            DEFECT
        }

        public static float DockPosition = -150f;

        public Vector3 CurrentDockPosition {
            get {
                return this.globalPosition + (DockPosition + this.airlockMove) * this.rotation.Up;
            }
        }

        /// <summary>
        /// model bone of the rotating part of the station
        /// </summary>
        private ModelBone Barrier1;
        private ModelBone Barrier2;
        private ModelBone Barrier3;
        private ModelBone BarrierTop;
        private ModelBone Airlock;

        public DockState CurrentDockState {
            get {
                if ( dir == 0 ) {
                    if ( airlockMove <= 0 ) {
                        return DockState.OPEN;
                    }
                    if ( airlockMove >= 16 ) {
                        return DockState.CLOSED;
                    }
                } else {
                    if ( dir > 0 ) {
                        return DockState.CLOSING;
                    } else {
                        return DockState.OPENING;
                    }
                }
                return DockState.DEFECT;
            }
        }


        /// <summary>
        /// the transformation matrix of the rotating part of the station
        /// </summary>
        Matrix Barrier1Transform;
        Matrix Barrier2Transform;
        Matrix Barrier3Transform;
        Matrix BarrierTopTransform;
        Matrix AirlockTransform;


        /// <summary>
        /// the current rotation value of the rotating part of the station
        /// </summary>
        float AxisRot = 0.0f;

        float dir = 0f;
        float airlockMove = 16;
        int airLockDelay = 0;

        /// <summary>
        /// create a new space station within the world
        /// </summary>
        /// <param name="position">world position</param>
        public SpaceStation( Vector3 position) : base( "SpaceStation", position ) {
            init();
        }

        protected override void _initMiniMapIcon() {
            this.miniMapIcon.color = MiniMap.SPECIAL_COLOR;
            this.miniMapIcon.AbstractScale = 2.0f;
        }


        /// <summary>
        /// init the space station
        /// </summary>
        private void init() {
            Barrier1 = model.Bones["Barrier1"];
            Barrier2 = model.Bones["Barrier2"];
            Barrier3 = model.Bones["Barrier3"];
            BarrierTop = model.Bones["BarrierTop"];
            Airlock = model.Bones["Airlock"];

            Barrier1Transform = Barrier1.Transform;
            Barrier2Transform = Barrier2.Transform;
            Barrier3Transform = Barrier3.Transform;
            BarrierTopTransform = BarrierTop.Transform;
            AirlockTransform = Airlock.Transform;

        }


        /// <summary>
        /// update the space station
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );

            // update rotation of the rotating part
            AxisRot += (float)( Math.PI / 1440 );

            if ( airLockDelay > 0 ) {
                airLockDelay--;
            } else {
                if ( dir != 0 ) {
                    airlockMove += dir;
                    if ( airlockMove > 16 ) {
                        airlockMove = 16;
                        dir = 0f;
                    }
                    if ( airlockMove < -0.5f ) {
                        airlockMove = -0.5f;
                        dir = 0f;
                    }
                }
            }

            
            //Barrier1.Transform = Matrix.CreateRotationY( AxisRot * 9.0f ) * Barrier1Transform;
            Barrier2.Transform = Matrix.CreateRotationY( -AxisRot * 18.2f ) * Barrier2Transform;
            Barrier3.Transform = Matrix.CreateRotationY( AxisRot * 26.8f ) * Barrier3Transform;
            BarrierTop.Transform = Matrix.CreateScale( new Vector3( 1, 1.0f - 1.0f / 16.5f * (airlockMove + 0.5f), 1 ) );
            Airlock.Transform = Matrix.CreateTranslation(new Vector3( 0, airlockMove, 0 ) ) * AirlockTransform;
        }


        public void OpenDock(int delay) {
            this.airLockDelay = delay;
            dir = -0.2f;
        }

        public void CloseDock( int delay ) {
            this.airLockDelay = delay;
            dir = 0.2f;
        }


        public override void addDebugOutput() {
            Antares.debugViewer.Add( new DebugElement( this, "Airlock", delegate( Object obj ) {
                return String.Format( "{0}", ( obj as SpaceStation ).CurrentDockState );
            } ) );
        }

    }
}
