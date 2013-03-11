using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.AIComposer
{
    public class AI_Output : AI_Item
    {

        public enum OutputType
        {
            RESULT,
            REMEMBER
        }

        public AI_Output(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, "Output", game)
        {
            this.subType = OutputType.RESULT;
            AddPort(AI_ItemPort.PortType.INPUT);

            this.subTypeString.String = this.subType.ToString();
        }

    }

}
