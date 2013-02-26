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

        private HUD2DTexture testTex;

        private HUD2DButton toMenuButton;

        private HUD2DButton toCockpitButton;

        /// <summary>
        /// create a new command controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CommandController(Game1 game, View.View view) : base(game, view) 
        {
            // test content
            testTex = new HUD2DTexture(this.game);
            testTex.abstractPosition = new Vector2(0.5f, 0.4f);
            testTex.positionType = HUDType.RELATIV;
            testTex.abstractSize = new Vector2(0.5f, 0.5f);
            testTex.sizeType = HUDType.RELATIV;
            testTex.Texture = game.Content.Load<Texture2D>("Sprites//Galaxy");
            testTex.color = new Color(150, 160, 70, 120);

            this.view.allHUD_2D.Add(testTex);

            toMenuButton = new HUD2DButton("Menu", new Vector2(0.3f, 0.8f), 0.7f, this.game);
            toMenuButton.positionType = HUDType.RELATIV;

            this.view.allHUD_2D.Add(toMenuButton);

            toCockpitButton = new HUD2DButton("Cockpit", new Vector2(0.7f, 0.8f), 0.7f, this.game);
            toCockpitButton.positionType = HUDType.RELATIV;

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
                    testTex.rotation = testTex.rotation + 0.2f;
                }
            }
        }

    }

}
