using System;
using Battlestation_Antaris.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the Menu controller
    /// </summary>
    class MenuController : SituationController
    {

        private HUDTexture testTex;

        private HUDButton toCommandButton;

        private HUDButton toCockpitButton;

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController(Game1 game, View.View view) : base(game, view) 
        {
            this.worldUpdate = WorldUpdate.NO_UPDATE;

            // test content
            testTex = new HUDTexture(game.Content);
            testTex.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height * 0.4f);
            testTex.Width = game.GraphicsDevice.Viewport.Width / 3;
            testTex.Height = testTex.Width;
            testTex.Texture = game.Content.Load<Texture2D>("Sprites//Erde2");

            this.view.allHUD_2D.Add(testTex);

            toCommandButton = 
                new HUDButton(
                    "Command", 
                    new Vector2(
                        game.GraphicsDevice.Viewport.Width * 0.3f, 
                        game.GraphicsDevice.Viewport.Height * 0.8f),
                    0.7f,
                    game.Content);

            this.view.allHUD_2D.Add(toCommandButton);

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
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.toCommandButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.switchTo(Situation.COMMAND);
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
