using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Control;

namespace Battlestation_Antares.View.HUD.AIComposer {
    public class AI_Output : AI_Item {

        public enum OutputType {
            RESULT,
            REMEMBER
        }

        public AI_Output( Vector2 abstractPosition, HUDType positionType, SituationController controller ) : base( abstractPosition, positionType, controller ) {
            this.typeString.String = "Output";

            AddPort( AI_ItemPort.PortType.INPUT );

            SetSubType( OutputType.RESULT );
        }

    }

}
