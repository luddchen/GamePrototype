using System;
using Battlestation_Antares.Model;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.View.HUD.CockpitHUD;
using Battlestation_Antaris.Model;
using Battlestation_Antaris.View.HUD.CockpitHUD;
using HUD;
using HUD.HUD;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the cockpit view
    /// </summary>
    class CockpitView : HUDView {

        public TactileSpatialObject target;

        /// <summary>
        /// the game view camera
        /// </summary>
        protected Camera camera;

        HUDTexture cockpitTexture;

        Vector3 t1;
        Vector3 t2;
        Vector3 t3;
        Vector3 t4;
        public bool drawBeam = false;

        SpatialObject targetCross;
        SpatialObject beam;

        /// <summary>
        /// creates a new cockpit view
        /// </summary>
        /// <param name="game">the game</param>
        public CockpitView(Color? backgroundColor) : base(backgroundColor) {
            this.camera = new Camera();
        }


        /// <summary>
        /// initialize cockpit view HUD and content
        /// </summary>
        public override void Initialize() {

            HUDTexture compassBG = new HUDTexture( "Sprites//Circle", new Color( 12, 16, 8, 16 ), new Vector2( 0.5f, 0.15f ), new Vector2( 0.125f, 0.15f ) );
            compassBG.LayerDepth = 0.11f;
            this.Add( compassBG );

            // 2D HUD
            cockpitTexture = new HUDTexture( "Sprites//cockpit3", Color.White, new Vector2( 0.5f, 0.5f ), new Vector2( 1, 1 ) );
            cockpitTexture.LayerDepth = 1.0f;
            this.Add( cockpitTexture );

            this.Add( Antares.world.miniMapRenderer );

            HUDTexture cross = new HUDTexture( "Sprites//Cross", Color.White, new Vector2( 0.5f, 0.5f ), new Vector2( 0.02f, 0.03f ) );
            cross.LayerDepth = 0.8f;
            this.Add( cross );

            Velocity velocity = new Velocity( new Vector2( 0.3f, 0.925f ), new Vector2( 0.01f, 0.15f ) );
            velocity.LayerDepth = 0.3f;
            this.Add( velocity );
            LaserHeat heat = new LaserHeat( new Vector2( 0.38f, 0.895f ), new Vector2( 0.0125f, 0.21f ) );
            heat.LayerDepth = 0.3f;
            heat.AbstractRotation = (float)( -Math.PI / 18.0 );
            this.Add( heat );
            LaserHeat heat2 = new LaserHeat( new Vector2( 0.62f, 0.895f ), new Vector2( 0.0125f, 0.21f ) );
            heat2.LayerDepth = 0.3f;
            heat2.AbstractRotation = (float)( Math.PI / 18.0 );
            this.Add( heat2 );

            ObjectHealth shipHealth = new ObjectHealth( new Vector2( 0.045f, 0.8f ), HUDType.RELATIV );
            shipHealth.setObject( Antares.world.spaceShip, "Sprites//HUD//Ship", 0.75f);
            this.Add( shipHealth );
            shipHealth.LayerDepth = 0.3f;

            ObjectHealth stationHealth = new ObjectHealth( new Vector2( 0.11f, 0.92f ), HUDType.RELATIV );
            stationHealth.setObject( Antares.world.spaceStation, "Sprites//HUD//Station", 0.75f );
            this.Add( stationHealth );
            stationHealth.LayerDepth = 0.3f;


            HUDValueLamp lamp = new HUDValueLamp( new Vector2( 0.5f, 0.03f ), new Vector2( 0.02f, 0.03f ) );
            lamp.GetValue =
                delegate() {
                    if ( Antares.world.spaceShip.target == null ) {
                        return Color.Transparent;
                    }
                    return Color.Green;
                    ;
                };
            this.Add( lamp );

            this.targetCross = new SpatialObject( "TargetCross" );
            this.beam = new SpatialObject( "Beam" );

        }


        /// <summary>
        /// draw cockpit view content
        /// </summary>
        protected override void DrawPreContent() {

            Antares.InitDepthBuffer();

            // init camera
            this.camera.ClampTo( Antares.world.spaceShip );

            Tools.Draw3D.Draw( Antares.world.AllObjects, this.camera );

            drawTargetCross();

            if ( this.drawBeam ) {
                Vector3 start = Vector3.Hermite( t1, t2, t3, t4, 0.0f );
                Vector3 end;
                float off = 0.00125f;
                for ( float f = off; f <= 0.7f - off; f += off , off *= 1.2f ) {
                    end = Vector3.Hermite( t1, t2, t3, t4, f );
                    Vector3 fRot = Tools.Tools.GetRotation( end - start, Matrix.Identity );

                    this.beam.globalPosition = ( start + end ) / 2;
                    this.beam.rotation = Matrix.CreateRotationX( fRot.X ) * Matrix.CreateRotationY( fRot.Z );
                    this.beam.scale = new Vector3( 1, 1, Vector3.Distance( start, end ) * 0.95f );
                    this.beam.Draw( this.camera );
                    start = end;
                }
            }

        }

        private void drawTargetCross() {
            if ( this.target != null ) {
                Vector3 tRot = Tools.Tools.GetRotation( this.target.globalPosition - Antares.world.spaceShip.globalPosition, Antares.world.spaceShip.rotation );
                this.targetCross.rotation = Tools.Tools.YawPitchRoll( Antares.world.spaceShip.rotation, tRot.Z, tRot.X, tRot.Y );
                this.targetCross.globalPosition = this.target.globalPosition;
                this.targetCross.scale = new Vector3( this.target.bounding.Radius );
                this.targetCross.Draw( this.camera );
            }
        }

        public void SetBeamParameter( Vector3 t1, Vector3 t2, Vector3 t3, Vector3 t4 ) {
            this.t1 = t1;
            this.t2 = t2;
            this.t3 = t3;
            this.t4 = t4;
        }

    }
}
