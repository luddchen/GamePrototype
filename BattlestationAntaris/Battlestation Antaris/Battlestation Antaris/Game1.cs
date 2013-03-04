using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Battlestation_Antaris.Control;
using System.Collections.Generic;
using System;

namespace Battlestation_Antaris
{

    /// <summary>
    /// the Antaris main class
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        /// <summary>
        /// game graphics device manager
        /// </summary>
        public GraphicsDeviceManager graphics;


        /// <summary>
        /// game sprite batch
        /// </summary>
        public SpriteBatch spriteBatch;


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
        SituationController activeSituation;


        /// <summary>
        /// a list of all game situations (control/view)
        /// </summary>
        List<SituationController> allSituations;


        /// <summary>
        /// creates a new Antaris game
        /// </summary>
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferMultiSampling = true; // antialiasing
            Content.RootDirectory = "Content";
            this.IsFixedTimeStep = true;
            Window.AllowUserResizing = true;
            Window.ClientSizeChanged += new EventHandler<EventArgs>(Window_ClientSizeChanged);

        }


        /// <summary>
        /// called if client size change
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event</param>
        void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            this.activeSituation.view.Window_ClientSizeChanged();
        }


        /// <summary>
        /// initialize the game
        /// </summary>
        protected override void Initialize()
        {
            this.inputProvider = new InputProvider();

            // create situations (control and views)
            this.allSituations = new List<SituationController>();
            this.allSituations.Add(new CockpitController(this, new View.CockpitView(this)));
            this.allSituations.Add(new CommandController(this, new View.CommandView(this)));
            this.allSituations.Add(new MenuController(this, new View.MenuView(this)));

            // create and initialize world model
            this.world = new Model.WorldModel(this);

            this.world.Initialize(Content);
            
            base.Initialize();
        }


        /// <summary>
        /// load the game content and initialize views
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (SituationController situation in this.allSituations)
            {
                situation.view.Initialize();
            }

            switchTo(Situation.MENU);
        }


        /// <summary>
        /// unload the content
        /// </summary>
        protected override void UnloadContent()
        {
        }


        /// <summary>
        /// switch to a specified situation (control/view)
        /// </summary>
        /// <param name="situation">the new active situation</param>
        public void switchTo(Situation situation)
        {
            if (this.activeSituation != null)
            {
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
        protected override void Update(GameTime gameTime)
        {
            // update input
            this.inputProvider.Update();


            switch (this.activeSituation.worldUpdate)
            {
                // update world, then update situation 
                case WorldUpdate.PRE:
                    this.world.Update(gameTime);
                    this.activeSituation.Update(gameTime);
                    break;

                // update only situation, no world update
                case WorldUpdate.NO_UPDATE:
                    this.activeSituation.Update(gameTime);
                    break;

                // update situation, then update world
                case WorldUpdate.POST:
                    this.activeSituation.Update(gameTime);
                    this.world.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }


        /// <summary>
        /// draw the game in respect to the active situation
        /// </summary>
        /// <param name="gameTime">the game time</param>
        protected override void Draw(GameTime gameTime)
        {
            this.activeSituation.view.Draw();
            base.Draw(gameTime);
        }
    }
}
