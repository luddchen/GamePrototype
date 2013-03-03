using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;
using System;

namespace Battlestation_Antaris.View.HUD.CommandHUD
{
    class BuildMenu : HUD2DContainer
    {

        private Type buildingObjectType;

        private HUD2DButton buildTurretButton;

        private HUD2DButton buildRadarButton;

        private HUD2DTexture bgTexture;

        public BuildMenu(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {

            buildTurretButton = new HUD2DButton("Turret", new Vector2(0f, 0f), 0.7f, this.game);
            buildTurretButton.abstractPosition = new Vector2(0f, -30f);
            buildTurretButton.layerDepth = 0.4f;
            this.Add(buildTurretButton);

            buildRadarButton = new HUD2DButton("Radar", new Vector2(0f, 0f), 0.7f, this.game);
            buildRadarButton.abstractPosition = new Vector2(0f, 30f);
            buildRadarButton.layerDepth = 0.4f;
            this.Add(buildRadarButton);

            this.bgTexture = new HUD2DTexture(this.game);
            this.bgTexture.abstractSize = new Vector2(150, 150);
            this.bgTexture.color = new Color(30, 30, 30, 100);
            this.Add(this.bgTexture);
        }

        public bool isUpdatedClicked(InputProvider input)
        {
            bool clicked = false;
            if (this.buildTurretButton.isUpdatedClicked(this.game.inputProvider))
            {
                clicked = true;
                buildingObjectType = Type.GetType("Battlestation_Antaris.Model.Turret");
            }

            if (this.buildRadarButton.isUpdatedClicked(this.game.inputProvider))
            {
                clicked = true;
                buildingObjectType = Type.GetType("Battlestation_Antaris.Model.Radar");
            }
            return clicked;
        }

        public Type getBuildingType()
        {
            return buildingObjectType;
        }
    }
}
