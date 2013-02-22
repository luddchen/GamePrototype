using System;
using Battlestation_Antaris.View;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the command controller
    /// </summary>
    class CommandController : SituationController
    {

        private HUDTexture testTex;

        private HUDButton toMenuButton;

        private HUDButton toCockpitButton;

        /// <summary>
        /// create a new command controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CommandController(Game1 game, View.View view) : base(game, view) 
        {
            // test content
            testTex = new HUDTexture(game.Content);
            testTex.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height * 0.4f);
            testTex.Width = game.GraphicsDevice.Viewport.Width / 2;
            testTex.Height = testTex.Width * 0.7f;
            testTex.Texture = game.Content.Load<Texture2D>("Sprites//Galaxy");
            testTex.Color = new Color(150, 160, 70, 120);

            this.view.allHUD_2D.Add(testTex);

            toMenuButton =
                new HUDButton(
                    "Menu",
                    new Vector2(
                        game.GraphicsDevice.Viewport.Width * 0.3f,
                        game.GraphicsDevice.Viewport.Height * 0.8f),
                    0.7f,
                    game.Content);

            this.view.allHUD_2D.Add(toMenuButton);

            toCockpitButton =
                new HUDButton(
                    "Cockpit",
                    new Vector2(
                        game.GraphicsDevice.Viewport.Width * 0.7f,
                        game.GraphicsDevice.Viewport.Height * 0.8f),
                    0.7f,
                    game.Content);

            this.view.allHUD_2D.Add(toCockpitButton);

        }


        /// <summary>
        /// update the command controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.toMenuButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.switchTo(Situation.MENU);
            }

            if (this.toCockpitButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.switchTo(Situation.COCKPIT);
            }

            if (this.game.inputProvider.isMouseButtonPressed())
            {
                Vector2 mousePos = this.game.inputProvider.getMousePos();

                if (this.testTex.Intersects(mousePos))
                {
                    testTex.Rotation = testTex.Rotation + 0.2f;
                }
            }
        }

    }

}
