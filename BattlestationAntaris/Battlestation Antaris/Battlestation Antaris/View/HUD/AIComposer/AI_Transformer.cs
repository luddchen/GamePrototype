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
            this.typeString.String = "Transformer";

            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort( AI_ItemPort.PortType.OUTPUT);

            SetSubType(TransformerType.SCALE);
        }


        public override void SetSubType(object subType)
        {
            base.SetSubType(subType);

            if ((TransformerType)subType == TransformerType.SCALE)
            {
                this.parameters = new float[1];
            }
            else
            {
                this.parameters = null;
            }
        }

    }

}
