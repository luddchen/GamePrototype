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

        TransformerType transformerType;

        public AI_Transformer(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, "Transformer", game)
        {
            this.transformerType = TransformerType.SCALE;

            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort( AI_ItemPort.PortType.OUTPUT);
        }

    }

}
