using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.View
{

    abstract class View
    {

        public Controller controller;

        public List<HUDElement> allHUDs;

        public View(Controller controller)
        {
            this.controller = controller;
            this.allHUDs = new List<HUDElement>();
        }

        public virtual void Draw()
        {
            foreach (HUDElement element in allHUDs)
            {
                element.Draw();
            }
        }

    }

}
