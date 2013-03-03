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

        private HUD2DArray buttonGrid;

        private HUD2DArray buttons1;

        private HUD2DArray buttons2;

        private HUD2DButton toCommandButton;

        private HUD2DButton toCockpitButton;

        private HUD2DButton exitButton;

        private HUD2DButton xButton;

        private HUD2DButton yButton;

        private HUD2DButton zButton;

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
            testTex.layerDepth = 1.0f;

            this.view.allHUD_2D.Add(testTex);

            this.buttonGrid = new HUD2DArray(new Vector2(0.5f, 0.8f), HUDType.RELATIV, new Vector2(600, 150), HUDType.ABSOLUT, game);
            this.buttonGrid.layerDepth = 0.5f;
            this.buttonGrid.direction = LayoutDirection.VERTICAL;
            this.buttonGrid.CreateBackground(true);

            this.buttons1 =  new HUD2DArray(new Vector2(0.5f, 0.8f), HUDType.RELATIV, new Vector2(600, 150), HUDType.ABSOLUT, game);
            this.buttons1.direction = LayoutDirection.HORIZONTAL;

            this.buttons2 = new HUD2DArray(new Vector2(0.5f, 0.8f), HUDType.RELATIV, new Vector2(600, 150), HUDType.ABSOLUT, game);
            this.buttons2.direction = LayoutDirection.HORIZONTAL;

            this.buttonGrid.Add(this.buttons1);
            this.buttonGrid.Add(this.buttons2);


            this.buttons1.CreateBackground(true);

            this.toCommandButton = new HUD2DButton("Command", Vector2.Zero, 1, this.game);
            this.buttons1.Add(toCommandButton);

            this.toCockpitButton = new HUD2DButton("Cockpit", Vector2.Zero, 1, this.game);
            this.buttons1.Add(toCockpitButton);

            this.exitButton = new HUD2DButton("Exit", Vector2.Zero, 1, this.game);
            this.buttons1.Add(this.exitButton);

            this.buttons2.CreateBackground(true);

            this.xButton = new HUD2DButton("X", Vector2.Zero, 1, this.game);
            this.buttons2.Add(xButton);

            this.yButton = new HUD2DButton("Y", Vector2.Zero, 1, this.game);
            this.buttons2.Add(yButton);

            this.zButton = new HUD2DButton("Z", Vector2.Zero, 1, this.game);
            this.buttons2.Add(this.zButton);

            this.view.allHUD_2D.Add(this.buttonGrid);

            this.buttonGrid.ClientSizeChanged(Vector2.Zero);

        }


        /// <summary>
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.buttonGrid.IsUpdatedHovered(this.game.inputProvider)) { }

            if (this.buttons1.IsUpdatedHovered(this.game.inputProvider)) {}

            if (this.buttons2.IsUpdatedHovered(this.game.inputProvider)) { }

            if (this.xButton.isUpdatedClicked(this.game.inputProvider)) { }

            if (this.yButton.isUpdatedClicked(this.game.inputProvider)) { }

            if (this.zButton.isUpdatedClicked(this.game.inputProvider)) { }

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
