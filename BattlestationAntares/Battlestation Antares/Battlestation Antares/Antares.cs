using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Battlestation_Antaris.Control;
using Battlestation_Antares.Model;
using Battlestation_Antaris.View;
using Battlestation_Antares.View;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework.Content;
using HUD;
using Battlestation_Antares.Control;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antares {

    /// <summary>
    /// the Antares main class
    /// </summary>
    class Antares : Game {
        private const Boolean ACTIVATE_DEBUG = true;

        /// <summary>
        /// DebugViewer draws debugOutput ingame
        /// </summary>
        public static DebugViewer debugViewer;

        /// <summary>
        /// game graphics device manager
        /// </summary>
        public static GraphicsDeviceManager graphics;


        public static ContentManager content;


        /// <summary>
        /// game input provider
        /// </summary>
        public static InputProvider inputProvider;


        /// <summary>
        /// game world model
        /// </summary>
        public static Model.WorldModel world;


        /// <summary>
        /// game sprite batch
        /// </summary>
        private SpriteBatch spriteBatch;


        /// <summary>
        /// the active situation (control/view)
        /// </summary>
        private SituationController activeSituation;


        /// <summary>
        /// a list of all game situations (control/view)
        /// </summary>
        List<SituationController> allSituations;


        private HUDView lastView;
        private int blendValue;

        /// <summary>
        /// creates a new Antares game
        /// </summary>
        public Antares() {
            Antares.graphics = new GraphicsDeviceManager( this );
            Antares.graphics.PreferMultiSampling = true; // antialiasing
            Antares.graphics.PreferredBackBufferWidth = (int)( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.9 );
            Antares.graphics.PreferredBackBufferHeight = (int)( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.9 );
            Antares.graphics.IsFullScreen = false;
            Antares.graphics.SynchronizeWithVerticalRetrace = true;

            this.Content.RootDirectory = "Content";
            Antares.content = Content;
            
            this.IsFixedTimeStep = true;
            Window.AllowUserResizing = true;
        }


        /// <summary>
        /// initialize the game
        /// </summary>
        protected override void Initialize() {
            // create Services
            Antares.inputProvider = new InputProvider();
            HUDService.Initialize( this, Content.Load<Texture2D>( "Sprites//Square" ), Content.Load<SpriteFont>( "Fonts//Font" ), 2, Antares.inputProvider );
            Antares.debugViewer = new DebugViewer();
            SpatialObjectFactory.initializeFactory( this.Content, Antares.world );

            this.spriteBatch = new SpriteBatch( Antares.graphics.GraphicsDevice );

            // create and initialize world model
            Antares.world = new Model.WorldModel( this );
            Antares.world.Initialize( this.Content );

            // create situations (control and views)
            this.allSituations = new List<SituationController>();
            this.allSituations.Add( new CockpitController( this, new CockpitView(null) ) );
            this.allSituations.Add( new CommandController( this, new CommandView(null) ) );
            this.allSituations.Add( new MenuController( this, new MenuView(null) ) );
            this.allSituations.Add( new AIController( this, new AIView(null) ) );
            this.allSituations.Add( new DockController( this, new DockView( null ) ) );

            initializeDebug();

            base.Initialize();
        }

        private void initializeDebug() {
            SituationSwitch situationSwitch = new SituationSwitch( this );
            if ( ACTIVATE_DEBUG ) {
                foreach ( SituationController situationControl in allSituations ) {
                    situationControl.view.Add( Antares.debugViewer );
                    situationControl.Register( situationSwitch );
                    situationControl.view.Add( situationSwitch );
                }
            }
        }


        /// <summary>
        /// load the game content and initialize views
        /// </summary>
        protected override void LoadContent() {
            foreach ( SituationController situation in this.allSituations ) {
                situation.view.Initialize();
            }

            switchTo( Situation.MENU );
        }


        /// <summary>
        /// unload the content
        /// </summary>
        protected override void UnloadContent() { }


        /// <summary>
        /// switch to a specified situation (control/view)
        /// </summary>
        /// <param name="situation">the new active situation</param>
        public void switchTo( Situation situation ) {
            if ( this.activeSituation != null ) {
                this.activeSituation.onExit();
                this.lastView = this.activeSituation.view;
                this.blendValue = 0;
            }
            this.activeSituation = this.allSituations[(int)situation];
            this.activeSituation.onEnter();
            this.IsMouseVisible = true;
        }


        /// <summary>
        /// update the game content
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Update( GameTime gameTime ) {
            // update input
            Antares.inputProvider.Update();

            this.activeSituation.Update( gameTime );

            base.Update( gameTime );
        }


        /// <summary>
        /// draw the game in respect to the active situation
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Draw( GameTime gameTime ) {

            this.activeSituation.view.Draw();

            Antares.graphics.GraphicsDevice.Clear( Color.Black );

            Matrix tMatrix = Matrix.Identity;

            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront, 
                BlendState.AlphaBlend, 
                SamplerState.LinearClamp, 
                DepthStencilState.DepthRead, 
                RasterizerState.CullNone, 
                null,
                tMatrix);

            if ( this.lastView != null ) {
                this.spriteBatch.Draw( this.activeSituation.view.renderTarget, HUDService.RenderTexturePosition, null, 
                            new Color(this.blendValue, this.blendValue, this.blendValue, this.blendValue), 0.0f,
                            HUDService.RenderTextureOrigin, HUDService.RenderTextureScale, SpriteEffects.None, 0.5f );

                this.spriteBatch.Draw( this.lastView.renderTarget, HUDService.RenderTexturePosition, null, 
                            new Color( 255 - this.blendValue, 255 - this.blendValue, 255 - this.blendValue, 255 - this.blendValue ), 0.0f,
                            HUDService.RenderTextureOrigin, HUDService.RenderTextureScale, SpriteEffects.None, 0.4f );

                if ( this.blendValue < 248 ) {
                    this.blendValue += 8;
                } else {
                    this.lastView = null;
                }

            } else {
                this.spriteBatch.Draw( this.activeSituation.view.renderTarget, HUDService.RenderTexturePosition, null, Color.White, 0.0f,
                                            HUDService.RenderTextureOrigin, HUDService.RenderTextureScale, SpriteEffects.None, 0.5f );
            }

            this.spriteBatch.End();
        }


        public static void InitDepthBuffer() {
            Antares.graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() {
                DepthBufferEnable = true,
                DepthBufferWriteEnable = true
            };
            Antares.graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
        }

    }

}
