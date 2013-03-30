using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.View.HUD.CockpitHUD;
using Battlestation_Antares.Tools;
using Battlestation_Antaris.View.HUD.CockpitHUD;
using Battlestation_Antares.Model;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the cockpit view
    /// </summary>
    class CockpitView : HUDView {

        public SpatialObject target;

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


        /// <summary>
        /// a list of background images
        /// </summary>
        List<BackgroundObject> backgroundObjects;

        Skybox skybox;

        Grid grid;

        Microsoft.Xna.Framework.Graphics.Model targetCrossModel;
        Matrix[] targetCrossBoneTransforms;

        Microsoft.Xna.Framework.Graphics.Model beamModel;
        Matrix[] beamTransforms;


        /// <summary>
        /// creates a new cockpit view
        /// </summary>
        /// <param name="game">the game</param>
        public CockpitView(Color? backgroundColor) : base(backgroundColor) {
            this.camera = new Camera();
            this.backgroundObjects = new List<BackgroundObject>();
        }


        /// <summary>
        /// initialize cockpit view HUD and content
        /// </summary>
        public override void Initialize() {

            HUDTexture compassBG = new HUDTexture( "Sprites//Circle", new Vector2( 0.5f, 0.15f ), new Vector2( 0.125f, 0.15f ) );
            compassBG.LayerDepth = 0.11f;
            compassBG.color = new Color( 12, 16, 8, 16 );
            this.Add( compassBG );

            // 2D HUD
            cockpitTexture = new HUDTexture( "Sprites//cockpit3", new Vector2( 0.5f, 0.5f ), new Vector2( 1, 1 ) );
            cockpitTexture.LayerDepth = 1.0f;
            this.Add( cockpitTexture );

            this.Add( Antares.world.miniMapRenderer );

            HUDTexture cross = new HUDTexture( "Sprites//Cross", new Vector2( 0.5f, 0.5f ), new Vector2( 0.02f, 0.03f ) );
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

            this.targetCrossModel = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( "Models//TargetCross" );
            this.targetCrossBoneTransforms = new Matrix[this.targetCrossModel.Bones.Count];

            this.beamModel = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( "Models//Beam" );
            this.beamTransforms = new Matrix[this.beamModel.Bones.Count];

            // background
            for ( int i = 0; i < 4; i++ ) {
                addBackgroundObject( "Models//BGTest//test2" );
            }

            for ( int i = 0; i < 1; i++ ) {
                addBackgroundObject( "Models//BGTest//test" );
            }

            skybox = new Skybox( "Models//Skysphere//skysphere");
            grid = new Grid( "Models//Grid//grid");
        }

        private void addBackgroundObject( String model ) {
            float yaw = (float)( RandomGen.random.NextDouble() * Math.PI * 2 );
            float pitch = (float)( RandomGen.random.NextDouble() * Math.PI );
            float roll = (float)( RandomGen.random.NextDouble() * Math.PI * 2 );
            Matrix bgRot = Tools.Tools.YawPitchRoll( Matrix.Identity, yaw, pitch, roll );
            int red = 128 + RandomGen.random.Next( 127 );
            int green = 128 + RandomGen.random.Next( 127 );
            int blue = 128 + RandomGen.random.Next( 127 );
            Color bgColor = new Color( red, green, blue );
            float scale = 1.25f + (float)RandomGen.random.NextDouble() * 1.5f;

            this.backgroundObjects.Add( new BackgroundObject( model, bgRot, scale, bgColor) );
        }


        /// <summary>
        /// draw cockpit view content
        /// </summary>
        protected override void DrawPreContent() {

            Antares.InitDepthBuffer();

            // init camera
            this.camera.ClampTo( Antares.world.spaceShip );

            this.skybox.Draw( this.camera );

            // draw background
            int nr = 1;
            foreach ( BackgroundObject bg in this.backgroundObjects ) {
                bg.Draw( this.camera, nr++ );
            }

            Tools.Draw3D.Draw( Antares.world.allDrawable, this.camera );

            drawTargetCross();

            if ( this.drawBeam ) {
                float off = 0.00125f;
                for ( float f = 0.0f; f <= 0.7f - off; f += off ) {
                    Vector3 start = Vector3.Hermite( t1, t2, t3, t4, f );
                    Vector3 end = Vector3.Hermite( t1, t2, t3, t4, f + off );
                    Vector3 center = ( start + end ) / 2;
                    Vector3 fRot = Tools.Tools.GetRotation( end - start, Matrix.Identity );

                    Tools.Draw3D.Draw( beamModel, beamTransforms, camera.view, camera.projection, center,
                        Matrix.CreateRotationX( fRot.X ) * Matrix.CreateRotationY( fRot.Z ),
                        new Vector3( 1, 1, Vector3.Distance( start, end ) * 0.95f ) );

                    off *= 1.2f;
                }
            }

            this.grid.Draw( this.camera );
            
        }

        private void drawTargetCross() {
            if ( this.target != null ) {
                Vector3 tRot = Tools.Tools.GetRotation( this.target.globalPosition - Antares.world.spaceShip.globalPosition, Antares.world.spaceShip.rotation );
                Matrix crossRot = Tools.Tools.YawPitchRoll( Antares.world.spaceShip.rotation, tRot.Z, tRot.X, tRot.Y );

                Tools.Draw3D.Draw( this.targetCrossModel, this.targetCrossBoneTransforms,
                                    this.camera.view, this.camera.projection,
                                    this.target.globalPosition, crossRot, new Vector3( this.target.bounding.Radius ) );
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
