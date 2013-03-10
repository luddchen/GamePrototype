using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Item
    {

        String itemTypeName;

        public List<AI_ItemPort> inputs;

        public List<AI_ItemPort> outputs;


        public AI_Item(String typeName)
        {
            this.itemTypeName = typeName;
            this.inputs = new List<AI_ItemPort>();
            this.outputs = new List<AI_ItemPort>();
        }

    }

}
