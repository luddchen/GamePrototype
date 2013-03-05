using System;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;
using Battlestation_Antaris.View.HUD.CommandHUD;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Battlestation_Antaris.Model;
using Battlestation_Antaris.Tools;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the command controller
    /// </summary>
    class CommandController : SituationController
    {
        private const int MAX_CAMERA_ZOOM = 1000;

        private const int MIN_CAMERA_ZOOM = 4000;

        private enum CommandMode { NORMAL, BUILD }

        private CommandMode currentMode;

        private HUD2DButton toMenuButton;

        private HUD2DButton toCockpitButton;

        private MiniMap.Config mapConfig;

        private BuildMenu buildMenu;

        private Dictionary<Type, MouseTexture> mouseTextures;

        private MouseTexture currentMouseTexture;

        /// <summary>
        /// create a new command controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CommandController(Game1 game, View.View view) : base(game, view) 
        {
            this.currentMode = CommandMode.NORMAL;

            toMenuButton = new HUD2DButton("Menu", new Vector2(0.1f, 0.9f), 0.7f, this.game);
            toMenuButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add(toMenuButton);

            toCockpitButton = new HUD2DButton("Cockpit", new Vector2(0.9f, 0.9f), 0.7f, this.game);
            toCockpitButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add(toCockpitButton);

            buildMenu = new BuildMenu(new Vector2(0.9f, 0.5f), HUDType.RELATIV, this.game);
            this.view.allHUD_2D.Add(buildMenu);

            mouseTextures = new Dictionary<Type, MouseTexture>();
            mouseTextures.Add(typeof(Battlestation_Antaris.Model.Turret), new MouseTexture(game.Content.Load<Texture2D>("Models//Turret//turret_2d"), game));
            mouseTextures.Add(typeof(Battlestation_Antaris.Model.Radar), new MouseTexture(game.Content.Load<Texture2D>("Models//Radar//radar_2d"), game));
            this.view.allHUD_2D.AddRange(mouseTextures.Values);

            mapConfig = new MiniMap.Config(new Vector2(0.5f, 0.5f), new Vector2(0.625f, 1f), new Vector2(0.625f, 1f));
            mapConfig.iconPositionScale = 0.25f;

        }

        public override void onEnter()
        {
            this.game.world.miniMap.changeConfig(this.mapConfig);
        }

        public override void onExit()
        {
            this.currentMode = CommandMode.NORMAL;
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

            if (currentMode == CommandMode.BUILD)
            {
                // activate mouse texture
                Type buildingType = buildMenu.getStructureType();
                this.mouseTextures[buildingType].isVisible = true;
                this.mouseTextures[buildingType].update();
            }

            if (currentMode == CommandMode.BUILD && this.game.inputProvider.isLeftMouseButtonPressed())
            {
                createNewStructure();
            }

            if (currentMode == CommandMode.BUILD && this.game.inputProvider.isRightMouseButtonPressed())
            {
                Type structureType = buildMenu.getStructureType();
                this.mouseTextures[structureType].isVisible = false;
                this.currentMode = CommandMode.NORMAL;
            }

            if (this.buildMenu.isUpdatedClicked(this.game.inputProvider))
            {
                if (this.currentMouseTexture != null)
                {
                    this.currentMouseTexture.isVisible = false;
                }
                this.currentMode = CommandMode.BUILD;
            }

            if (this.game.inputProvider.getMouseWheelChange() != 0)
            {
                this.game.world.miniMap.ZoomOnMouseWheelOver();
            }
        }

        private void createNewStructure()
        {
            Type structureType = buildMenu.getStructureType();
            Vector2 mousePos = this.game.inputProvider.getMousePos();
            if (this.game.world.miniMap.Intersects(mousePos))
            {
                SpatialObject newStructure = SpatialObjectFactory.buildSpatialObject(structureType);
                Vector2 miniMapCoord = this.game.world.miniMap.screenToMiniMapCoord(this.game.inputProvider.getMousePos());
                newStructure.globalPosition = this.game.world.miniMap.miniMapToWorldCoord(miniMapCoord);
            }
        }

    }

}
