using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.View.HUD.AIComposer
{
    public class AI_Output : AI_Item
    {

        public enum OutputType
        {
            RESULT = "result",
            REMEMBER = "remember"
        }

        OutputType outputType;

        public AI_Output()
            : base("Output")
        {
            this.outputType = OutputType.RESULT;
            this.inputs.Add(new AI_ItemPort(AI_ItemPort.PortType.INPUT, this));
        }

    }

}
