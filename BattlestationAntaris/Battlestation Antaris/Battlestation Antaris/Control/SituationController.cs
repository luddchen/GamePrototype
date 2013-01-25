using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control
{

    abstract class SituationController
    {

        public Controller controller;

        public SituationController(Controller controller)
        {
            this.controller = controller;
        }

        public abstract void Update(GameTime gameTime);

    }

}
