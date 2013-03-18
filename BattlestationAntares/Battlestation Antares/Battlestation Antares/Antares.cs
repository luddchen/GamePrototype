using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Battlestation_Antares.Control;
using System.Collections.Generic;
using System;
using Battlestation_Antares.Model;
using Battlestation_Antares.View.HUD;

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
        public GraphicsDeviceManager graphics;


        /// <summary>
        /// game sprite batch
        /// </summary>
        public SpriteBatch spriteBatch;


        /// <summary>
        /// game primitive batch
        /// </summary>
        public PrimitiveBatch primitiveBatch;


        /// <summary>
        /// game input provider
        /// </summary>
        public InputProvider inputProvider;


        /// <summary>
        /// game world model
        /// </summary>
        public Model.WorldModel world;


        /// <summary>
        /// the active situation (control/view)
        /// </summary>
        public SituationController activeSituation;


        /// <summary>
        /// a list of all game situations (control/view)
        /// </summary>
        List<SituationController> allSituations;


        /// <summary>
        /// creates a new Antares game
        /// </summary>
        public Antares() {
            graphics = new GraphicsDeviceManager( this );
            graphics.PreferMultiSampling = true; // antialiasing
            graphics.PreferredBackBufferWidth = (int)( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.9 );
            graphics.PreferredBackBufferHeight = (int)( GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.9 );
            graphics.IsFullScreen = false;
            graphics.SynchronizeWithVerticalRetrace = true;
            Content.RootDirectory = "Content";
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
            if ( this.activeSituation != null ) {
                this.activeSituation.view.Window_ClientSizeChanged();
            }

            if ( this.primitiveBatch != null ) {
                this.primitiveBatch.ClientSizeChanged( this.GraphicsDevice.Viewport );
            }
        }


        /// <summary>
        /// initialize the game
        /// </summary>
        protected override void Initialize() {
            this.inputProvider = new InputProvider();

            Antares.debugViewer = new DebugViewer( this );

            // create situations (control and views)
            this.allSituations = new List<SituationController>();
            this.allSituations.Add( new CockpitController( this, new View.CockpitView( this ) ) );
            this.allSituations.Add( new CommandController( this, new View.CommandView( this ) ) );
            this.allSituations.Add( new MenuController( this, new View.MenuView( this ) ) );
            this.allSituations.Add( new AIController( this, new View.AIView( this ) ) );

            // create and initialize world model
            this.world = new Model.WorldModel( this );

            this.world.Initialize( Content );

            SpatialObjectFactory.initializeFactory( this.Content, this.world );

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
            spriteBatch = new SpriteBatch( GraphicsDevice );
            primitiveBatch = new PrimitiveBatch( GraphicsDevice );

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
            this.inputProvider.Update();

            switch ( this.activeSituation.worldUpdate ) {
                // update world, then update situation 
                case WorldUpdate.PRE:
                    this.world.Update( gameTime );
                    this.activeSituation.Update( gameTime );
                    break;

                // update only situation, no world update
                case WorldUpdate.NO_UPDATE:
                    this.activeSituation.Update( gameTime );
                    break;

                // update situation, then update world
                case WorldUpdate.POST:
                    this.activeSituation.Update( gameTime );
                    this.world.Update( gameTime );
                    break;
            }

            base.Update( gameTime );
        }


        /// <summary>
        /// draw the game in respect to the active situation
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Draw( GameTime gameTime ) {
            this.activeSituation.view.Draw();
            base.Draw( gameTime );
        }
    }
}
