using System;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD.CockpitHUD;

namespace Battlestation_Antares.Control
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

        private MiniMap.Config mapConfig;
        
        /// <summary>
        /// create a new cockpit controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CockpitController(Antares game, View.View view) : base(game, view) 
        {
            mouseVisibleCounter = mouseTimeOut;

            HUD2DContainer buttons = new HUD2DContainer(new Vector2(0.8f, 0.95f), HUDType.RELATIV, this.game);

            toCommandButton =
                new HUD2DButton(
                    "Command",
                    new Vector2(0,0),
                    0.5f,
                    this.game);

            this.toCommandButton.SetPressedAction(delegate() { this.game.switchTo(Situation.COMMAND); });
            buttons.Add(toCommandButton);

            toMenuButton =
                new HUD2DButton(
                    "Menu",
                    new Vector2(toCommandButton.size.X + 10, 0),
                    0.5f,
                    this.game);

            this.toMenuButton.SetPressedAction(delegate() { this.game.switchTo(Situation.MENU); });
            buttons.Add(toMenuButton);
            this.view.allHUD_2D.Add(buttons);

            fpsDisplay = new FpsDisplay(new Vector2(50, 20), this.game);
            this.view.allHUD_2D.Add(fpsDisplay);

            mapConfig = new MiniMap.Config(new Vector2(0.5f, 0.91f), new Vector2(0.25f, 0.18f), new Vector2(0.25f, 0.18f));
        }

        public override void onEnter()
        {
            this.game.world.miniMap.changeConfig(this.mapConfig);
        }


        /// <summary>
        /// update the cockpit controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

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

            this.game.world.miniMap.ZoomOnMouseWheelOver();

            // redirect input
            this.game.world.spaceShip.InjectControl( this.game.inputProvider.getInput() );

        }

    }

}
