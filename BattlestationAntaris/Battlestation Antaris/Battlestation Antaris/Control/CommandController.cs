using System;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;
using Battlestation_Antaris.View.HUD.CommandHUD;
using Battlestation_Antaris.View.HUD.CockpitHUD;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the command controller
    /// </summary>
    class CommandController : SituationController
    {
        private const int MAX_CAMERA_ZOOM = 1000;

        private const int MIN_CAMERA_ZOOM = 4000;

        private enum CommandMode { SCROLL, BUILD }

        private CommandMode currentMode;

        private HUD2DButton toMenuButton;

        private HUD2DButton toCockpitButton;

        private MiniMap.Config mapConfig;

        private BuildMenu buildMenu;

        /// <summary>
        /// create a new command controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CommandController(Game1 game, View.View view) : base(game, view) 
        {
            this.currentMode = CommandMode.SCROLL;

            toMenuButton = new HUD2DButton("Menu", new Vector2(0.1f, 0.9f), 0.7f, this.game);
            toMenuButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add(toMenuButton);

            toCockpitButton = new HUD2DButton("Cockpit", new Vector2(0.9f, 0.9f), 0.7f, this.game);
            toCockpitButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add(toCockpitButton);

            buildMenu = new BuildMenu(new Vector2(0.9f, 0.5f), HUDType.RELATIV, this.game);
            this.view.allHUD_2D.Add(buildMenu);

            mapConfig = new MiniMap.Config(new Vector2(0.5f, 0.5f), new Vector2(0.625f, 1f), new Vector2(0.625f, 1f));
            mapConfig.iconPositionScale = 0.25f;

        }

        public override void onEnter()
        {
            this.game.world.miniMap.changeConfig(this.mapConfig);
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

            if (this.buildMenu.isUpdatedClicked(this.game.inputProvider))
            {
                this.currentMode = CommandMode.BUILD;
            }

            if (this.game.inputProvider.isRightMouseButtonPressed())
            {
                this.currentMode = CommandMode.SCROLL;
            }

            if (currentMode == CommandMode.SCROLL && this.game.inputProvider.isLeftMouseButtonDown())
            {
                Vector2 mousePosChange = this.game.inputProvider.getMousePosChange();
                this.game.world.overviewCamPos.X += mousePosChange.X;
                this.game.world.overviewCamPos.Z += mousePosChange.Y;
            }


            if (currentMode == CommandMode.BUILD)
            {
                Type buildingType = buildMenu.getBuildingType();
                // TODO: draw building at mouse position
            }

            if (currentMode == CommandMode.BUILD && this.game.inputProvider.isLeftMouseButtonDown())
            {
                Type buildingType = buildMenu.getBuildingType();
                // TODO: create and add building to the model
            }

            if (this.game.inputProvider.getMouseWheelChange() != 0)
            {
                this.game.world.miniMap.ZoomOnMouseWheelOver();
            }
        }

    }

}
