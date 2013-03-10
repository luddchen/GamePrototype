using System;
using Battlestation_Antaris.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the Menu controller
    /// </summary>
    class MenuController : SituationController
    {

        private HUD2DArray buttonGrid;

        private HUD2DArray buttons1;

        private HUD2DArray optionsButtonGroup;

        private HUD2DButton toCommandButton;

        private HUD2DButton toCockpitButton;

        private HUD2DButton optionsButton;

        private HUD2DButton exitButton;

        private HUD2DButton videoButton;

        private HUD2DButton soundButton;

        private HUD2DButton controlButton;


        private List<HUD2DContainer> contentPages;

        private HUD2DArray videoPage;

        private HUD2DArray soundPage;

        private HUD2DArray controlPage;

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController(Game1 game, View.View view) : base(game, view) 
        {
            this.worldUpdate = WorldUpdate.NO_UPDATE;

            // test content

            this.buttonGrid = new HUD2DArray(new Vector2(0.5f, 0.8f), HUDType.RELATIV, new Vector2(600, 150), HUDType.ABSOLUT, game);
            this.buttonGrid.layerDepth = 0.5f;
            this.buttonGrid.direction = LayoutDirection.VERTICAL;

            this.buttons1 =  new HUD2DArray(new Vector2(0.5f, 0.8f), HUDType.RELATIV, new Vector2(600, 150), HUDType.ABSOLUT, game);
            this.buttons1.direction = LayoutDirection.HORIZONTAL;

            this.optionsButtonGroup = new HUD2DArray(new Vector2(0.5f, 0.8f), HUDType.RELATIV, new Vector2(600, 150), HUDType.ABSOLUT, game);
            this.optionsButtonGroup.direction = LayoutDirection.HORIZONTAL;
            this.optionsButtonGroup.isVisible = false;

            this.buttonGrid.Add(this.optionsButtonGroup);
            this.buttonGrid.Add(this.buttons1);


            this.toCommandButton = new HUD2DButton("Command", Vector2.Zero, 1, this.game);
            this.buttons1.Add(toCommandButton);

            this.toCockpitButton = new HUD2DButton("Cockpit", Vector2.Zero, 1, this.game);
            this.buttons1.Add(toCockpitButton);

            this.optionsButton = new HUD2DButton("Options", Vector2.Zero, 1, this.game);
            this.buttons1.Add(optionsButton);

            this.exitButton = new HUD2DButton("Exit", Vector2.Zero, 1, this.game);
            this.buttons1.Add(this.exitButton);

            this.videoButton = new HUD2DButton("Video", Vector2.Zero, 1, this.game);
            this.optionsButtonGroup.Add(videoButton);

            this.soundButton = new HUD2DButton("Sound", Vector2.Zero, 1, this.game);
            this.optionsButtonGroup.Add(soundButton);

            this.controlButton = new HUD2DButton("Control", Vector2.Zero, 1, this.game);
            this.optionsButtonGroup.Add(this.controlButton);


            this.view.allHUD_2D.Add(this.buttonGrid);


            this.contentPages = new List<HUD2DContainer>();

            this.videoPage = new HUD2DArray(new Vector2(0.5f, 0.4f), HUDType.RELATIV, new Vector2(0.7f, 0.5f), HUDType.RELATIV, game);
            this.videoPage.CreateBackground(true);
            this.videoPage.Add(new HUD2DString("Video", game));

            this.soundPage = new HUD2DArray(new Vector2(0.5f, 0.4f), HUDType.RELATIV, new Vector2(0.7f, 0.5f), HUDType.RELATIV, game);
            this.soundPage.CreateBackground(true);
            this.soundPage.Add(new HUD2DString("Sound", game));

            this.controlPage = new HUD2DArray(new Vector2(0.5f, 0.4f), HUDType.RELATIV, new Vector2(0.7f, 0.5f), HUDType.RELATIV, game);
            this.controlPage.CreateBackground(true);
            this.controlPage.Add(new HUD2DString("Control", game));

            this.contentPages.Add(this.videoPage);
            this.contentPages.Add(this.soundPage);
            this.contentPages.Add(this.controlPage);

            foreach (HUD2DContainer container in this.contentPages)
            {
                container.isVisible = false;
                this.view.allHUD_2D.Add(container);
            }

            this.view.Window_ClientSizeChanged();
        }


        /// <summary>
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {

            if (this.videoButton.isUpdatedClicked(this.game.inputProvider)) { }

            if (this.soundButton.isUpdatedClicked(this.game.inputProvider)) { }

            if (this.controlButton.isUpdatedClicked(this.game.inputProvider)) { }


            if (this.toCommandButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.switchTo(Situation.COMMAND);
            }

            if (this.toCockpitButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.switchTo(Situation.COCKPIT);
            }

            if (this.optionsButton.isUpdatedClicked(this.game.inputProvider))
            {
                Color temp = this.optionsButton.foregroundColorHover;
                this.optionsButton.foregroundColorHover = this.optionsButton.foregroundColorNormal;
                this.optionsButton.foregroundColorNormal = temp;

                if (this.optionsButtonGroup.isVisible)
                {
                    this.optionsButtonGroup.isVisible = false;

                    foreach (HUD2DContainer container in this.contentPages)
                    {
                        container.isVisible = false;
                    }
                }
                else
                {
                    this.optionsButtonGroup.isVisible = true;
                }
            }

            if (this.exitButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.Exit();
            }


            if (this.optionsButtonGroup.isVisible)
            {
                if (this.videoButton.isUpdatedClicked(this.game.inputProvider))
                {
                    foreach (HUD2DContainer container in this.contentPages)
                    {
                        container.isVisible = false;
                    }
                    this.videoPage.isVisible = true;
                }

                if (this.soundButton.isUpdatedClicked(this.game.inputProvider))
                {
                    foreach (HUD2DContainer container in this.contentPages)
                    {
                        container.isVisible = false;
                    }
                    this.soundPage.isVisible = true;
                }

                if (this.controlButton.isUpdatedClicked(this.game.inputProvider))
                {
                    foreach (HUD2DContainer container in this.contentPages)
                    {
                        container.isVisible = false;
                    }
                    this.controlPage.isVisible = true;
                }
            }

        }

    }

}
