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

        private HUD2DButton exitButton;

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

            HUD2DArray buttons = 
                new HUD2DArray(new Vector2(0.5f, 0.8f), HUDType.RELATIV, new Vector2(600, 150), HUDType.ABSOLUT, game);

            buttons.layerDepth = 0.4f;
            buttons.CreateBackground(true);
            buttons.direction = LayoutDirection.HORIZONTAL;

            //HUD2DTexture buttonsBackground = new HUD2DTexture(game);
            //buttonsBackground.sizeType = buttons.sizeType;
            //buttonsBackground.abstractSize = buttons.abstractSize;
            //buttonsBackground.color = HUD2DButton.backgroundColorNormal;
            //buttons.Add(buttonsBackground);


            toCommandButton = new HUD2DButton("Command", Vector2.Zero, 1, this.game);

            buttons.Add(toCommandButton);

            toCockpitButton = new HUD2DButton("Cockpit", Vector2.Zero, 1, this.game);

            buttons.Add(toCockpitButton);

            exitButton = new HUD2DButton("Exit", Vector2.Zero, 1, this.game);

            buttons.Add(exitButton);

            this.view.allHUD_2D.Add(buttons);
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

            if (this.exitButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.Exit();
            }

            if (this.game.inputProvider.isLeftMouseButtonPressed())
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
