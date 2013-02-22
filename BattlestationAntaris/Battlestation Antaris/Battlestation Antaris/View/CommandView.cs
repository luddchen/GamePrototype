using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;
namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the command view
    /// </summary>
    class CommandView : View
    {

        /// <summary>
        /// creates a new command view
        /// </summary>
        /// <param name="game"></param>
        public CommandView(Game1 game)
            : base(game)
        {
        }


        /// <summary>
        /// draw the command view content
        /// </summary>
        protected override void DrawContent()
        {
        }


        /// <summary>
        /// initialize cammand view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // test content
            HUDString testString = new HUDString("Antaris Command", game.Content);
            testString.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height * 0.1f);

            this.allHUD_2D.Add(testString);
        }
    }

}
