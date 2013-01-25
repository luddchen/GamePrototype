using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control
{
    public enum Situation
    {
        cockpit = 0, command = 1, menu = 2
    }

    public class Controller
    {

        public Game1 game;

        List<Model.Model> allModels;
        List<View.View> allViews;
        List<SituationController> allSituations;

        View.View activeView;
        SituationController activeSituation;

        public Controller(Game1 game)
        {
            this.game = game;

            this.allModels = new List<Model.Model>();
            this.allViews = new List<View.View>();
            this.allSituations = new List<SituationController>();

            this.allModels.Add(new Model.WorldModel());
            this.allModels.Add(new Model.SpaceShipModel());
            this.allModels.Add(new Model.SpaceStationModel());

            addSituation( new View.CockpitView(this), new CockpitController(this));
            addSituation( new View.CommandView(this), new CommandController(this));
            addSituation( new View.MenuView(this), new MenuController(this));

            switchTo(Situation.menu);
        }

        private void addSituation(View.View view, SituationController situation)
        {
            this.allViews.Add(view);
            this.allSituations.Add(situation);
        }

        public void switchTo(Situation situation)
        {
            this.activeView = this.allViews[(int)situation];
            this.activeSituation = this.allSituations[(int)situation];
        }

        public void Update(GameTime gameTime)
        {
            this.activeSituation.Update(gameTime);
        }

        public void Draw()
        {
            this.activeView.Draw();
        }

    }

}
