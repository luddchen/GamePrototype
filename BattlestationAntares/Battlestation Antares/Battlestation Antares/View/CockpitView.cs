using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.View.HUD.CockpitHUD;
using Battlestation_Antares.Tools;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the cockpit view
    /// </summary>
    class CockpitView : View {
        /// <summary>
        /// the game view camera
        /// </summary>
        protected Camera camera;

        /// <summary>
        /// the cockpit compass
        /// </summary>
        Compass3d compass;


        TargetInfo targetInfo;

        HUD2DTexture cockpitTexture;


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
        public CockpitView() {
            this.camera = new Camera();

            this.compass = new Compass3d( Antares.content );
            this.allHUD_3D.Add( this.compass );
            this.is3D = true;

            this.backgroundColor = Color.White;
            this.backgroundObjects = new List<BackgroundObject>();
        }


        /// <summary>
        /// initialize cockpit view HUD and content
        /// </summary>
        public override void Initialize() {
            // 3D HUD
            this.compass.Initialize( Antares.world.spaceShip );


            // 2D HUD
            cockpitTexture = new HUD2DTexture();
            cockpitTexture.abstractPosition = new Vector2( 0.5f, 0.5f );
            cockpitTexture.positionType = HUDType.RELATIV;
            cockpitTexture.abstractSize = new Vector2( 1, 1 );
            cockpitTexture.sizeType = HUDType.RELATIV;
            cockpitTexture.Texture = Antares.content.Load<Texture2D>( "Sprites//cockpit3" );
            cockpitTexture.layerDepth = 1.0f;
            this.allHUD_2D.Add( cockpitTexture );

            this.allHUD_2D.Add( Antares.world.miniMap );

            this.targetInfo = new TargetInfo( new Vector2( 60, 200 ), HUDType.ABSOLUT, new Vector2( 150, 60 ), HUDType.ABSOLUT);
            this.allHUD_2D.Add( this.targetInfo );

            HUD2DTexture cross = new HUD2DTexture( Antares.content.Load<Texture2D>( "Sprites//Cross" ), new Vector2( 0.5f, 0.5f ), new Vector2( 48, 48 ), null, 1.0f, 0 );
            cross.positionType = HUDType.RELATIV;
            cross.layerDepth = 0.8f;
            this.allHUD_2D.Add( cross );

            this.allHUD_2D.Add( new Velocity( new Vector2( 0.3f, 0.925f ), HUDType.RELATIV, new Vector2( 15, 0.15f ), HUDType.ABSOLUT_RELATIV ) );
            this.allHUD_2D.Add( new LaserHeat( new Vector2( 0.35f, 0.925f ), HUDType.RELATIV, new Vector2( 15, 0.15f ), HUDType.ABSOLUT_RELATIV ) );

            HUD2DValueCircle circle = new HUD2DValueCircle( new Vector2( 0.25f, 0.95f ), HUDType.RELATIV, new Vector2( 0.06f, 0.1f ), HUDType.RELATIV );
            circle.GetValue =
                delegate() {
                    return 1.0f - Math.Abs( Antares.world.spaceShip.attributes.EnginePitch.CurrentVelocity / Antares.world.spaceShip.attributes.EnginePitch.MaxVelocity );
                };
            circle.SetMaxColor( Color.Blue );
            circle.SetMinColor( Color.DarkRed );
            this.allHUD_2D.Add( circle );

            HUD2DValueCircle circle2 = new HUD2DValueCircle( new Vector2( 0.19f, 0.95f ), HUDType.RELATIV, new Vector2( 0.06f, 0.1f ), HUDType.RELATIV );
            circle2.GetValue =
                delegate() {
                    return 1.0f - Math.Abs( Antares.world.spaceShip.attributes.EngineYaw.CurrentVelocity / Antares.world.spaceShip.attributes.EngineYaw.MaxVelocity );
                };
            circle2.SetMaxColor( Color.Green );
            circle2.SetMinColor( Color.Red );
            this.allHUD_2D.Add( circle2 );

            HUD2DValueLamp lamp = new HUD2DValueLamp( new Vector2( 0.5f, 0.03f ), HUDType.RELATIV, new Vector2( 0.02f, 0.03f ), HUDType.RELATIV );
            lamp.GetValue =
                delegate() {
                    if ( Antares.world.spaceShip.target == null ) {
                        return new Color( 64, 64, 64 );
                    }
                    return Color.Green;
                    ;
                };
            this.allHUD_2D.Add( lamp );

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
            this.cockpitTexture.color = this.backgroundColor;

            // init depth buffer
            Antares.graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() {
                DepthBufferEnable = true,
                DepthBufferWriteEnable = true
            };
            Antares.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            // init camera
            this.camera.ClampTo( Antares.world.spaceShip );

            // init compass
            if ( this.targetInfo.target != null ) {
                this.compass.target = this.targetInfo.target.globalPosition;
            } else {
                this.compass.target = Antares.world.spaceStation.globalPosition;
            }


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
            if ( this.targetInfo.target != null ) {
                Vector3 tRot = Tools.Tools.GetRotation( this.targetInfo.target.globalPosition - Antares.world.spaceShip.globalPosition, Antares.world.spaceShip.rotation );
                Matrix crossRot = Tools.Tools.YawPitchRoll( Antares.world.spaceShip.rotation, tRot.Z, tRot.X, tRot.Y );

                Tools.Draw3D.Draw( this.targetCrossModel, this.targetCrossBoneTransforms,
                                    this.camera.view, this.camera.projection,
                                    this.targetInfo.target.globalPosition, crossRot, new Vector3( this.targetInfo.target.bounding.Radius ) );
            }
        }

    }
}
