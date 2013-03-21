using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Battlestation_Antares.Control;
using System.Collections.Generic;
using System;
using Battlestation_Antares.Model;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antares {

    /// <summary>
    /// the Antares main class
    /// </summary>
    public class Antares : Microsoft.Xna.Framework.Game {
        private const Boolean ACTIVATE_DEBUG = true;

        /// <summary>
        /// DebugViewer draws debugOutput ingame
        /// </summary>
        public static DebugViewer debugViewer;

        /// <summary>
        /// game graphics device manager
        /// </summary>
        public static GraphicsDeviceManager graphics;


        public static Vector2 renderSize;


        public static ContentManager content;

        /// <summary>
        /// game sprite batch
        /// </summary>
        public static SpriteBatch spriteBatch;


        /// <summary>
        /// game primitive batch
        /// </summary>
        public static PrimitiveBatch primitiveBatch;


        /// <summary>
        /// game input provider
        /// </summary>
        public static InputProvider inputProvider;


        /// <summary>
        /// game world model
        /// </summary>
        public static Model.WorldModel world;


        /// <summary>
        /// the active situation (control/view)
        /// </summary>
        public SituationController activeSituation;


        /// <summary>
        /// a list of all game situations (control/view)
        /// </summary>
        List<SituationController> allSituations;


        //private RenderTarget2D renderTarget;

        //private Texture2D renderTexture;
        private Vector2 renderTextureOrigin;
        private Vector2 renderTexturePos;
        private float renderTextureScale;

        private int renderWidth = 1920;
        private int renderHeight = 1080;

        private View.View lastView;
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
            Window.ClientSizeChanged += new EventHandler<EventArgs>( Window_ClientSizeChanged );
        }


        /// <summary>
        /// called if client size change
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        void Window_ClientSizeChanged( object sender, EventArgs e ) {

            _calculateRenderTextureParameter();

            if ( this.activeSituation != null ) {
                this.activeSituation.view.Window_ClientSizeChanged();
            }

            if ( Antares.primitiveBatch != null ) {
                Antares.primitiveBatch.ClientSizeChanged();
            }
        }


        /// <summary>
        /// initialize the game
        /// </summary>
        protected override void Initialize() {
            Antares.inputProvider = new InputProvider();

            Antares.debugViewer = new DebugViewer();

            // create and initialize world model
            Antares.world = new Model.WorldModel( this );

            Antares.world.Initialize( this.Content );

            // create situations (control and views)
            this.allSituations = new List<SituationController>();
            this.allSituations.Add( new CockpitController( this, new View.CockpitView() ) );
            this.allSituations.Add( new CommandController( this, new View.CommandView() ) );
            this.allSituations.Add( new MenuController( this, new View.MenuView() ) );
            this.allSituations.Add( new AIController( this, new View.AIView() ) );

            SpatialObjectFactory.initializeFactory( this.Content, Antares.world );

            initializeDebug();

            base.Initialize();
        }

        private void initializeDebug() {
            if ( ACTIVATE_DEBUG ) {
                foreach ( SituationController situationControl in allSituations ) {
                    situationControl.view.allHUD_2D.Add( Antares.debugViewer );
                }
            }
        }


        /// <summary>
        /// load the game content and initialize views
        /// </summary>
        protected override void LoadContent() {
            Antares.spriteBatch = new SpriteBatch( GraphicsDevice );
            Antares.primitiveBatch = new PrimitiveBatch( GraphicsDevice );

            _calculateRenderTextureParameter();

            foreach ( SituationController situation in this.allSituations ) {
                situation.view.Initialize();
            }

            switchTo( Situation.MENU );
        }


        /// <summary>
        /// unload the content
        /// </summary>
        protected override void UnloadContent() {
        }


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
            this.activeSituation.view.Window_ClientSizeChanged();
        }


        /// <summary>
        /// update the game content
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Update( GameTime gameTime ) {
            // update input
            Antares.inputProvider.Update();

            switch ( this.activeSituation.worldUpdate ) {
                // update world, then update situation 
                case WorldUpdate.PRE:
                    Antares.world.Update( gameTime );
                    this.activeSituation.Update( gameTime );
                    break;

                // update only situation, no world update
                case WorldUpdate.NO_UPDATE:
                    this.activeSituation.Update( gameTime );
                    break;

                // update situation, then update world
                case WorldUpdate.POST:
                    this.activeSituation.Update( gameTime );
                    Antares.world.Update( gameTime );
                    break;
            }

            base.Update( gameTime );
        }


        /// <summary>
        /// draw the game in respect to the active situation
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Draw( GameTime gameTime ) {
            this._initRenderTarget( this.activeSituation.view );

            Antares.graphics.GraphicsDevice.SetRenderTarget( this.activeSituation.view.renderTarget );

            this.activeSituation.view.Draw();

            Antares.graphics.GraphicsDevice.SetRenderTarget( null );

            Antares.graphics.GraphicsDevice.Clear( new Color(16,0,0 ) );

            Antares.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if ( this.lastView != null ) {
                Antares.spriteBatch.Draw( this.activeSituation.view.renderTarget, this.renderTexturePos, null, 
                            new Color(this.blendValue, this.blendValue, this.blendValue, this.blendValue), 0.0f,
                            this.renderTextureOrigin, this.renderTextureScale, SpriteEffects.None, 0.5f );

                Antares.spriteBatch.Draw( this.lastView.renderTarget, this.renderTexturePos, null, 
                            new Color( 255 - this.blendValue, 255 - this.blendValue, 255 - this.blendValue, 255 - this.blendValue ), 0.0f,
                            this.renderTextureOrigin, this.renderTextureScale, SpriteEffects.None, 0.4f );

                if ( this.blendValue < 248 ) {
                    this.blendValue += 8;
                } else {
                    this.lastView = null;
                }
            } else {
                Antares.spriteBatch.Draw( this.activeSituation.view.renderTarget, this.renderTexturePos, null, Color.White, 0.0f,
                                            this.renderTextureOrigin, this.renderTextureScale, SpriteEffects.None, 0.5f );
            }

            Antares.spriteBatch.End();
        }


        public void setRenderSize( int width, int height ) {
            this.renderWidth = width;
            this.renderHeight = height;
            _calculateRenderTextureParameter();
        }


        private void _calculateRenderTextureParameter() {
            //if ( this.renderTarget == null ) {
            //    this.renderTarget = new RenderTarget2D( Antares.graphics.GraphicsDevice, this.renderWidth, this.renderHeight, true, 
            //                                            Antares.graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24 );
            //    this.renderTextureOrigin = new Vector2( this.renderTarget.Width / 2.0f, this.renderTarget.Height / 2.0f );
            //    Antares.renderSize = new Vector2( this.renderTarget.Width, this.renderTarget.Height);
            //}

            this.renderTexturePos = new Vector2( Antares.graphics.GraphicsDevice.Viewport.Width / 2.0f, Antares.graphics.GraphicsDevice.Viewport.Height / 2.0f );

            this.renderTextureOrigin = new Vector2( this.renderWidth / 2.0f, this.renderHeight / 2.0f );
            Antares.renderSize = new Vector2( this.renderWidth, this.renderHeight );

            float xScale = (float)Antares.graphics.GraphicsDevice.Viewport.Width / (float)this.renderWidth;
            float yScale = (float)Antares.graphics.GraphicsDevice.Viewport.Height / (float)this.renderHeight;

            this.renderTextureScale = Math.Min( xScale, yScale );
            Antares.inputProvider.setMouseTransform( this.renderTexturePos, this.renderTextureOrigin, this.renderTextureScale );
        }


        private void _initRenderTarget( View.View view ) {
            if ( view.renderTarget == null || view.renderTarget.Width != this.renderWidth || view.renderTarget.Height != this.renderHeight ) {
                view.renderTarget = new RenderTarget2D( Antares.graphics.GraphicsDevice, this.renderWidth, this.renderHeight, true,
                                                        Antares.graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24 );
            }
        }
    }

}
