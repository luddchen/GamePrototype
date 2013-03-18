using Battlestation_Antares.Control;
using Microsoft.Xna.Framework;
using System;

namespace Battlestation_Antares.View.HUD.CommandHUD {
    class BuildMenu : HUD2DContainer {

        private Type buildingObjectType;

        private HUD2DButton buildTurretButton;

        private HUD2DButton buildRadarButton;

        private HUD2DTexture bgTexture;

        public BuildMenu( Vector2 abstractPosition, HUDType positionType )
            : base( abstractPosition, positionType ) {

            buildTurretButton = new HUD2DButton( "Turret", new Vector2( 0f, 0f ), 0.7f );
            buildTurretButton.abstractPosition = new Vector2( 0f, -30f );
            buildTurretButton.layerDepth = 0.4f;
            this.Add( buildTurretButton );

            buildRadarButton = new HUD2DButton( "Radar", new Vector2( 0f, 0f ), 0.7f );
            buildRadarButton.abstractPosition = new Vector2( 0f, 30f );
            buildRadarButton.layerDepth = 0.4f;
            this.Add( buildRadarButton );

            this.bgTexture = new HUD2DTexture();
            this.bgTexture.abstractSize = new Vector2( 150, 150 );
            this.bgTexture.color = new Color( 30, 30, 30, 100 );
            this.Add( this.bgTexture );
        }

        public bool isUpdatedClicked( InputProvider input ) {
            bool clicked = false;
            if ( this.buildTurretButton.isUpdatedClicked( Antares.inputProvider ) ) {
                clicked = true;
                buildingObjectType = typeof( Battlestation_Antares.Model.Turret );
            }

            if ( this.buildRadarButton.isUpdatedClicked( Antares.inputProvider ) ) {
                clicked = true;
                buildingObjectType = typeof( Battlestation_Antares.Model.Radar );
            }
            return clicked;
        }

        public Type getStructureType() {
            return buildingObjectType;
        }
    }
}
