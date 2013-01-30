using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control
{

    // update of the worldmodel pre/post situation update, or dont update world
    public enum WorldUpdate
    {
        PRE, POST, NO_UPDATE
    }

    abstract class SituationController
    {

        public Controller controller;

        public WorldUpdate worldUpdate;

        public SituationController(Controller controller)
        {
            this.controller = controller;
            this.worldUpdate = WorldUpdate.PRE;
        }

        public abstract void Update(GameTime gameTime);

    }

}
