using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Transformer : AI_Item
    {

        public enum TransformerType
        {
            SCALE = "scale",
            SQR = "sqr",
            SQRT = "sqrt",
            INVERSE = "inverse"
        }

        TransformerType transformerType;

        public AI_Transformer()
            : base("Transformer")
        {
            this.transformerType = TransformerType.SCALE;
            this.inputs.Add(new AI_ItemPort(AI_ItemPort.PortType.INPUT, this));
            this.outputs.Add(new AI_ItemPort(AI_ItemPort.PortType.OUTPUT, this));
        }

    }

}
