using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the menu view
    /// </summary>
    class MenuView : View
    {
        int i = 0;


        /// <summary>
        /// creates a new menu view
        /// </summary>
        /// <param name="game">the game</param>
        public MenuView(Game1 game)
            : base(game)
        {
        }
        

        /// <summary>
        /// draw the view content
        /// </summary>
        protected override void DrawContent()
        {
            // test code
            i++;
            if (i > 60) i = 0;
            if (i < 30) this.backgroundColor = Color.Blue; else this.backgroundColor = Color.Black;
        }


        /// <summary>
        /// initialize menu view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // test content
            HUDString testString = new HUDString("Antaris Menu", game.Content);
            testString.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);

            this.allHUD_2D.Add(testString);
        }
    }

}
