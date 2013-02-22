using System;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.View;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the cockpit controller
    /// </summary>
    class CockpitController : SituationController
    {
        private int mouseTimeOut = 120;
        private int mouseVisibleCounter;

        private HUDButton toCommandButton;
        private HUDButton toMenuButton;

        private HUDTexture cokpitTexture;
        
        /// <summary>
        /// create a new cockpit controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CockpitController(Game1 game, View.View view) : base(game, view) 
        {
            mouseVisibleCounter = mouseTimeOut;

            cokpitTexture = new HUDTexture(game.Content);
            cokpitTexture.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);
            cokpitTexture.Width = game.GraphicsDevice.Viewport.Width;
            cokpitTexture.Height = game.GraphicsDevice.Viewport.Height;
            cokpitTexture.Texture = game.Content.Load<Texture2D>("Sprites//cockpit3");

            this.view.allHUD_2D.Add(cokpitTexture);

            toCommandButton =
                new HUDButton(
                    "Command",
                    new Vector2(
                        game.GraphicsDevice.Viewport.Width * 0.94f,
                        game.GraphicsDevice.Viewport.Height * 0.95f),
                    0.5f,
                    game.Content);

            this.view.allHUD_2D.Add(toCommandButton);

            toMenuButton =
                new HUDButton(
                    "Menu",
                    new Vector2(
                        game.GraphicsDevice.Viewport.Width * 0.83f,
                        game.GraphicsDevice.Viewport.Height * 0.95f),
                    0.5f,
                    game.Content);

            this.view.allHUD_2D.Add(toMenuButton);
        }


        /// <summary>
        /// update the cockpit controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.game.inputProvider.isMouseMoved())
            {
                mouseVisibleCounter = mouseTimeOut;
                this.game.IsMouseVisible = true;
            }
            else
            {
                if (mouseVisibleCounter > 0)
                {
                    mouseVisibleCounter--;
                }
                else
                {
                    this.game.IsMouseVisible = false;
                }
            }

            if (this.toCommandButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.switchTo(Situation.COMMAND);
            }

            if (this.toMenuButton.isUpdatedClicked(this.game.inputProvider))
            {
                this.game.switchTo(Situation.MENU);
            }

            // redirect input
            this.game.world.spaceShip.InjectControl( this.game.inputProvider.getInput() );

        }

    }

}
