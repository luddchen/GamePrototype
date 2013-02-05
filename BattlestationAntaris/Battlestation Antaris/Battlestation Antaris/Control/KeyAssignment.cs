using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Battlestation_Antaris.Control
{

    public class KeyAssignment
    {

        public Control control;
        public Keys key;

        public KeyAssignment(Control control, Keys key)
        {
            this.control = control;
            this.key = key;
        }

    }

}
