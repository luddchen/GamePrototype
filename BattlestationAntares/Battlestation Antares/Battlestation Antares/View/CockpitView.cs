using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.View.HUD.CockpitHUD;
using Battlestation_Antares.Tools;
using Battlestation_Antaris.View.HUD.CockpitHUD;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the cockpit view
    /// </summary>
    class CockpitView : View {

        public SpatialObject target;

        /// <summary>
        /// the game view camera
        /// </summary>
        protected Camera camera;

        HUDTexture cockpitTexture;


        /// <summary>
        /// a list of background images
        /// </summary>
        List<BackgroundObject> backgroundObjects;

        Skybox skybox;

        Grid grid;

        Microsoft.Xna.Framework.Graphics.Model targetCrossModel;
        Matrix[] targetCrossBoneTransforms;


        /// <summary>
        /// creates a new cockpit view
        /// </summary>
        /// <param name="game">the game</param>
        public CockpitView(Color? backgroundColor) : base(backgroundColor) {
            this.camera = new Camera();
;
            this.backgroundObjects = new List<BackgroundObject>();
        }


        /// <summary>
        /// initialize cockpit view HUD and content
        /// </summary>
        public override void Initialize() {

            HUDTexture compassBG = new HUDTexture();
            compassBG.AbstractPosition = new Vector2( 0.5f, 0.15f );
            compassBG.PositionType = HUDType.RELATIV;
            compassBG.AbstractSize = new Vector2( 0.125f, 0.15f );
            compassBG.SizeType = HUDType.RELATIV;
            compassBG.LayerDepth = 0.11f;
            compassBG.color = new Color( 12, 16, 8, 16 );
            compassBG.Texture = Antares.content.Load<Texture2D>( "Sprites//Circle" );
            this.Add( compassBG );

            // 2D HUD
            cockpitTexture = new HUDTexture();
            cockpitTexture.AbstractPosition = new Vector2( 0.5f, 0.5f );
            cockpitTexture.PositionType = HUDType.RELATIV;
            cockpitTexture.AbstractSize = new Vector2( 1, 1 );
            cockpitTexture.SizeType = HUDType.RELATIV;
            cockpitTexture.Texture = Antares.content.Load<Texture2D>( "Sprites//cockpit3" );
            cockpitTexture.LayerDepth = 1.0f;
            this.Add( cockpitTexture );

            this.Add( Antares.world.miniMap );

            HUDTexture cross = new HUDTexture( Antares.content.Load<Texture2D>( "Sprites//Cross" ), new Vector2( 0.5f, 0.5f ), new Vector2( 48, 48 ), null, 1.0f, 0 );
            cross.PositionType = HUDType.RELATIV;
            cross.LayerDepth = 0.8f;
            this.Add( cross );

            this.Add( new Velocity( new Vector2( 0.3f, 0.925f ), HUDType.RELATIV, new Vector2( 15, 0.15f ), HUDType.ABSOLUT_RELATIV ) );
            LaserHeat heat = new LaserHeat( new Vector2( 0.35f, 0.925f ), HUDType.RELATIV, new Vector2( 15, 0.15f ), HUDType.ABSOLUT_RELATIV );
            heat.LayerDepth = 0.3f;
            this.Add( heat );

            ObjectHealth shipHealth = new ObjectHealth( new Vector2( 0.045f, 0.8f ), HUDType.RELATIV );
            shipHealth.setObject( Antares.world.spaceShip, Antares.content.Load<Texture2D>( "Sprites//HUD//Ship" ), 0.75f);
            this.Add( shipHealth );
            shipHealth.LayerDepth = 0.3f;

            ObjectHealth stationHealth = new ObjectHealth( new Vector2( 0.11f, 0.92f ), HUDType.RELATIV );
            stationHealth.setObject( Antares.world.spaceStation, Antares.content.Load<Texture2D>( "Sprites//HUD//Station" ), 0.75f );
            this.Add( stationHealth );
            stationHealth.LayerDepth = 0.3f;


            HUDValueLamp lamp = new HUDValueLamp( new Vector2( 0.5f, 0.03f ), HUDType.RELATIV, new Vector2( 0.02f, 0.03f ), HUDType.RELATIV );
            lamp.GetValue =
                delegate() {
                    if ( Antares.world.spaceShip.target == null ) {
                        return new Color( 64, 64, 64 );
                    }
                    return Color.Green;
                    ;
                };
            this.Add( lamp );

            this.targetCrossModel = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( "Models//TargetCross" );
            this.targetCrossBoneTransforms = new Matrix[this.targetCrossModel.Bones.Count];

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

    }
}
