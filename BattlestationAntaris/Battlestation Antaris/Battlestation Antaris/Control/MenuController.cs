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

        private HUD2DTexture testTex;

        private HUD2DButton toCommandButton;

        private HUD2DButton toCockpitButton;

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController(Game1 game, View.View view) : base(game, view) 
        {
            this.worldUpdate = WorldUpdate.NO_UPDATE;

            // test content
            testTex = new HUD2DTexture(this.game);
            testTex.abstractPosition = new Vector2(0.5f, 0.4f);
            testTex.positionType = HUDType.RELATIV;
            testTex.abstractSize = new Vector2(0.5f, 0.5f);
            testTex.sizeType = HUDType.RELATIV;
            testTex.Texture = game.Content.Load<Texture2D>("Sprites//Erde2");

            this.view.allHUD_2D.Add(testTex);

            toCommandButton = new HUD2DButton("Command", new Vector2(0.3f, 0.8f), 0.7f, this.game);
            toCommandButton.positionType = HUDType.RELATIV;

            this.view.allHUD_2D.Add(toCommandButton);

            toCockpitButton = new HUD2DButton("Cockpit", new Vector2(0.7f, 0.8f), 0.7f, this.game);
            toCockpitButton.positionType = HUDType.RELATIV;

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
                    testTex.rotation = testTex.rotation + 0.2f;
                }
            }
        }

    }

}
