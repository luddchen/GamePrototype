using Microsoft.Xna.Framework;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    class AI_Output : AI_Item {

        public enum OutputType {
            RESULT,
            REMEMBER
        }

        public AI_Output() : base() {
            this.typeString.Text = "Output";

            AddPort( AI_ItemPort.PortType.INPUT );

            SetSubType( OutputType.RESULT );
        }

    }

}
