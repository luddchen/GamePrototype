using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View
{

    class CockpitView : View
    {
        int i = 0;
        Color backgroundColor = Color.Black;

        public CockpitView(Controller controller)
            : base(controller)
        {
        }

        public override void Draw()
        {
            if (i < 200) i++; else i = 0;
            if (i < 100) backgroundColor = Color.Yellow; else backgroundColor = Color.Black;
            this.controller.game.GraphicsDevice.Clear(this.backgroundColor);
        }

    }

}
