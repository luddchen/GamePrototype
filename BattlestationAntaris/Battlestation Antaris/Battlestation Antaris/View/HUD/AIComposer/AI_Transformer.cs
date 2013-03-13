using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Transformer : AI_Item
    {

        public enum TransformerType
        {
            SCALE,
            SQR,
            SQRT,
            INVERSE
        }

        public AI_Transformer(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.subType = TransformerType.SCALE;
            this.itemTypeName = "Transformer";
            this.typeString.String = this.itemTypeName;

            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort( AI_ItemPort.PortType.OUTPUT);

            this.subTypeString.String = this.subType.ToString();
        }

    }

}
