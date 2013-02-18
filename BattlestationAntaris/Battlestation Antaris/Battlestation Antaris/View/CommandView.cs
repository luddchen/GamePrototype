using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;
namespace Battlestation_Antaris.View
{

    class CommandView : View
    {

        int i = 0;

        public CommandView(Game1 game)
            : base(game)
        {
        }

        public override void Draw()
        {
            base.Draw();
            DrawHUD2D();

            i++;
            if (i > 60) i = 0;
            if (i < 30) this.backgroundColor = Color.Green; else this.backgroundColor = Color.Black;
        }


        public override void Initialize()
        {
            HUDString testString = new HUDString("Antaris Command", game.Content);
            testString.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);

            this.allHUD_2D.Add(testString);
        }
    }

}
