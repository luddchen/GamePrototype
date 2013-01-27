using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;
namespace Battlestation_Antaris.View
{

    class CommandView : View
    {

        int i = 0;
        Color backgroundColor = Color.Black;

        public CommandView(Controller controller)
            : base(controller)
        {
        }

        public override void Draw()
        {
            base.Draw();
            i++;
            if (i > 60) i = 0;
            if (i < 30) backgroundColor = Color.Green; else backgroundColor = Color.Black;
            this.controller.game.GraphicsDevice.Clear(this.backgroundColor);
        }

    }

}
