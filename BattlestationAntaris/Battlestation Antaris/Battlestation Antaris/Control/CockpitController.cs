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
        private FpsDisplay fpsDisplay;
        
        /// <summary>
        /// create a new cockpit controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CockpitController(Game1 game, View.View view) : base(game, view) 
        {
            mouseVisibleCounter = mouseTimeOut;

            HUDRelativeContainer buttons = new HUDRelativeContainer(0.8f, 0.95f, game.GraphicsDevice.Viewport);

            toCommandButton =
                new HUDButton(
                    "Command",
                    new Vector2(0,0),
                    0.5f,
                    game.Content);

            buttons.Add(toCommandButton);

            toMenuButton =
                new HUDButton(
                    "Menu",
                    new Vector2(toCommandButton.Width + 10,0),
                    0.5f,
                    game.Content);

            buttons.Add(toMenuButton);

            this.view.allHUD_2D.Add(buttons);

            fpsDisplay = new FpsDisplay(new Vector2(50, 20), game.Content);
            this.view.allHUD_2D.Add(fpsDisplay);
        }


        /// <summary>
        /// update the cockpit controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            fpsDisplay.Update(gameTime);

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
