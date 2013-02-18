using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Battlestation_Antaris.Control;
using System.Collections.Generic;
using System;

namespace Battlestation_Antaris
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public GraphicsDeviceManager graphics;
        public SpriteBatch spriteBatch;

        public InputProvider inputProvider;

        SituationController activeSituation;
        List<SituationController> allSituations;

        public Model.WorldModel world;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.inputProvider = new InputProvider();

            this.allSituations = new List<SituationController>();
            this.allSituations.Add(new CockpitController(this, new View.CockpitView(this)));
            this.allSituations.Add(new CommandController(this, new View.CommandView(this)));
            this.allSituations.Add(new MenuController(this, new View.MenuView(this)));

            switchTo(Situation.MENU);

            this.world = new Model.WorldModel(this);

            this.world.Initialize(Content);

            // CollisionTree testing
            Battlestation_Antaris.Model.CollisionOctree tree = new Model.CollisionOctree(2, 1, new BoundingBox(new Vector3(-400, -400, -400), new Vector3(400, 400, 400)));
            tree.insertFromWorld(this.world);

            Console.Out.WriteLine(tree);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (SituationController situation in this.allSituations)
            {
                situation.view.Initialize();
            }
        }

        protected override void UnloadContent()
        {
        }

        public void switchTo(Situation situation)
        {
            this.activeSituation = this.allSituations[(int)situation];
        }

        protected override void Update(GameTime gameTime)
        {
            this.inputProvider.Update();

            switch (this.activeSituation.worldUpdate)
            {
                case WorldUpdate.PRE:
                    this.world.Update(gameTime);
                    this.activeSituation.Update(gameTime);
                    break;

                case WorldUpdate.NO_UPDATE:
                    this.activeSituation.Update(gameTime);
                    break;

                case WorldUpdate.POST:
                    this.activeSituation.Update(gameTime);
                    this.world.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.activeSituation.view.Draw();
            base.Draw(gameTime);
        }
    }
}
