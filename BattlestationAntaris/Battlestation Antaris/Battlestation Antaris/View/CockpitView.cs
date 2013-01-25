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

        Camera camera;

        public CockpitView(Controller controller)
            : base(controller)
        {
            this.camera = new Camera(this.controller.game.GraphicsDevice);
        }

        public override void Draw()
        {
            i++;
            if (i > 60) i = 0;
            if (i < 30) backgroundColor = Color.Yellow; else backgroundColor = Color.Black;
            this.controller.game.GraphicsDevice.Clear(this.backgroundColor);
        }

    }

}
