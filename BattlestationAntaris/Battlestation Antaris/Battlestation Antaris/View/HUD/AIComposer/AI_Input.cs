using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Input : AI_Item
    {

        public enum InputType
        {
            DISTANCE = "distance",
            ORTHOGONAL_VELOCITY = "orthogonal velocity",
            ROTATION = "rotation",
            SHIELD_HEALTH = "shield health",
            HULL_HEALTH = "hull health"
        }

        public InputType inputType;

        public AI_Input() 
            : base("Input")
        {
            this.inputType = InputType.DISTANCE;
            this.outputs.Add(new AI_ItemPort(AI_ItemPort.PortType.OUTPUT, this));
        }

    }

}
