using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Input : AI_Item {

        public enum InputType {
            DISTANCE,
            ORTHOGONAL_VELOCITY,
            ROTATION,
            SHIELD_HEALTH,
            HULL_HEALTH
        }

        public AI_Input( Vector2 abstractPosition, HUDType positionType ) : base( abstractPosition, positionType ) {
            this.typeString.String = "Input";

            AddPort( AI_ItemPort.PortType.OUTPUT );

            SetSubType( InputType.DISTANCE );
        }

    }

}
