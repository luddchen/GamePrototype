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

        private HUD2DButton toMenuButton;

        private HUD2DButton toCockpitButton;

        private const int MAX_CAMERA_ZOOM = 1000;

        private const int MIN_CAMERA_ZOOM = 4000;

        /// <summary>
        /// create a new command controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CommandController(Game1 game, View.View view) : base(game, view) 
        {
            toMenuButton = new HUD2DButton("Menu", new Vector2(0.1f, 0.9f), 0.7f, this.game);
            toMenuButton.positionType = HUDType.RELATIV;

            this.view.allHUD_2D.Add(toMenuButton);

            toCockpitButton = new HUD2DButton("Cockpit", new Vector2(0.9f, 0.9f), 0.7f, this.game);
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

            if (this.game.inputProvider.isLeftMouseButtonDown())
            {
                Vector2 mousePosChange = this.game.inputProvider.getMousePosChange();
                this.game.world.overviewCamPos.X += mousePosChange.X;
                this.game.world.overviewCamPos.Z += mousePosChange.Y;
            }

            if (this.game.inputProvider.getMouseWheelChange() != 0)
            {
                float newCameraY = this.game.world.overviewCamPos.Y - 3 * this.game.inputProvider.getMouseWheelChange();
                newCameraY = Math.Max(MAX_CAMERA_ZOOM, newCameraY);
                newCameraY = Math.Min(MIN_CAMERA_ZOOM, newCameraY);
                this.game.world.overviewCamPos.Y = newCameraY;
            }
        }

    }

}
