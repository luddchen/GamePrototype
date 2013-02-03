using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View
{

    class MenuView : View
    {
        int i = 0;

        public MenuView(Game1 game)
            : base(game)
        {
        }

        public override void Draw()
        {
            base.Draw();

            i++;
            if (i > 60) i = 0;
            if (i < 30) this.backgroundColor = Color.Blue; else this.backgroundColor = Color.Black;
        }

        public override void Initialize()
        {
        }
    }

}
