using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control
{

    abstract class SituationController
    {

        public Game1 game;

        public View.View view;

        public WorldUpdate worldUpdate;

        public SituationController(Game1 game, View.View view)
        {
            this.game = game;
            this.view = view;
            this.worldUpdate = WorldUpdate.PRE;
        }

        public abstract void Update(GameTime gameTime);

    }

}
