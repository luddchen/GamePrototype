using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control
{

    class Controller
    {

        public Game1 game;

        List<View.View> allViews;
        List<Model.Model> allModels;

        View.View activeView;

        public Controller(Game1 game)
        {
            this.game = game;

            this.allViews = new List<View.View>();
            this.allViews.Add( new View.CockpitView(this) );
            this.allViews.Add( new View.CommandView(this) );
            this.activeView = this.allViews[1];

            this.allModels = new List<Model.Model>();
            this.allModels.Add(new Model.WorldModel());
            this.allModels.Add(new Model.SpaceShipModel());
            this.allModels.Add(new Model.SpaceStationModel());

        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw()
        {
            this.activeView.Draw();
        }

    }

}
