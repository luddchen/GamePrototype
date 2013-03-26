using Battlestation_Antares.Control;
using Microsoft.Xna.Framework;
using System;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD.CommandHUD {
    class BuildMenu : HUDArray {

        private Type buildingObjectType;

        private HUDButton buildTurretButton;

        private HUDButton buildRadarButton;

        private Action action;

        public BuildMenu( Vector2 abstractPosition, HUDType positionType, Action action, SituationController controller )
            : base( abstractPosition, new Vector2(0.1f, 0.1f) ) {

            this.action = action;

            buildTurretButton = new HUDButton( "Turret", new Vector2( 0f, 0f ), 0.7f, controller );
            buildTurretButton.AbstractPosition = new Vector2( 0f, -30f );
            buildTurretButton.LayerDepth = 0.4f;
            buildTurretButton.SetPressedAction( 
                delegate() {
                    this.buildingObjectType = typeof( Battlestation_Antares.Model.Turret );
                    this.action();
                } );
            this.Add( buildTurretButton );

            buildRadarButton = new HUDButton( "Radar", new Vector2( 0f, 0f ), 0.7f, controller );
            buildRadarButton.AbstractPosition = new Vector2( 0f, 30f );
            buildRadarButton.LayerDepth = 0.4f;
            buildRadarButton.SetPressedAction(
                delegate() {
                    this.buildingObjectType = typeof( Battlestation_Antares.Model.Radar );
                    this.action();
                } );
            this.Add( buildRadarButton );

            //this.CreateBackground( true ); // use SetBackground() instead
        }

        public Type getStructureType() {
            return buildingObjectType;
        }
    }
}
