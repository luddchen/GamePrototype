using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.View
{

    abstract class View
    {

        protected Controller controller;

        public View(Controller controller)
        {
            this.controller = controller;
        }

        public abstract void Draw();

    }

}
