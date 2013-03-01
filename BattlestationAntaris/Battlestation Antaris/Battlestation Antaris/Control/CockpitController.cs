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

        private HUD2DButton toCommandButton;
        private HUD2DButton toMenuButton;
        private FpsDisplay fpsDisplay;

        private HUD2DButton debugButton;
        
        /// <summary>
        /// create a new cockpit controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CockpitController(Game1 game, View.View view) : base(game, view) 
        {
            mouseVisibleCounter = mouseTimeOut;

            HUD2DContainer buttons = new HUD2DContainer(new Vector2(0.8f, 0.95f), HUDType.RELATIV, this.game);

            toCommandButton =
                new HUD2DButton(
                    "Command",
                    new Vector2(0,0),
                    0.5f,
                    this.game);

            buttons.Add(toCommandButton);

            toMenuButton =
                new HUD2DButton(
                    "Menu",
                    new Vector2(toCommandButton.size.X + 10, 0),
                    0.5f,
                    this.game);

            buttons.Add(toMenuButton);

            this.view.allHUD_2D.Add(buttons);

            fpsDisplay = new FpsDisplay(new Vector2(50, 20), this.game);
            this.view.allHUD_2D.Add(fpsDisplay);

            debugButton = new HUD2DButton("Debug", new Vector2(50, 100), 0.5f, this.game);
            this.view.allHUD_2D.Add(debugButton);
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

            if (this.debugButton.isUpdatedClicked(this.game.inputProvider))
            {
                Console.Out.WriteLine(this.game.world.spaceShip.attributes);
            }

            // redirect input
            this.game.world.spaceShip.InjectControl( this.game.inputProvider.getInput() );

        }

    }

}
