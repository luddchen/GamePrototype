using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Mixer : AI_Item
    {

        public enum MixerType
        {
            AVG = "avg",
            MULTIPLY = "multiply",
            MIN = "min",
            MAX = "max"
        }

        MixerType mixerType;

        public AI_Mixer()
            : base("Mixer")
        {
            this.mixerType = MixerType.AVG;
            this.inputs.Add(new AI_ItemPort(AI_ItemPort.PortType.INPUT, this));
            this.inputs.Add(new AI_ItemPort(AI_ItemPort.PortType.INPUT, this));
            this.outputs.Add(new AI_ItemPort(AI_ItemPort.PortType.OUTPUT, this));
        }

    }

}
