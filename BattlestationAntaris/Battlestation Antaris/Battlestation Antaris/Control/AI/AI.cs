using System;
using System.Collections.Generic;
using Battlestation_Antaris.View.HUD.AIComposer;

namespace Battlestation_Antaris.Control.AI
{

    public class AI
    {

        List<AI_Input.InputType> inputs;

        public AI()
        {
            this.inputs = new List<AI_Input.InputType>();
        }

        public void Create(AI_Container aiContainer)
        {
            // create lists for input ,connection , items
            List<AI_Connection> connections = new List<AI_Connection>();
            List<AI_Item> items = new List<AI_Item>();
            List<Tuple<int[], int[]>> portIndices = new List<Tuple<int[], int[]>>();

            foreach(AI_Item item in aiContainer.aiItems) 
            {
                if (item is AI_Input)
                {
                    if (! this.inputs.Contains(((AI_Input)item).inputType))
                    {
                        this.inputs.Add(((AI_Input)item).inputType);
                    }
                    connections.AddRange(((AI_Input)item).outputs[0].connections);
                    items.Add(item);
                    int[] read = new int[0];
                    int[] write = new int[1];
                    write[0] = this.inputs.IndexOf( ((AI_Input)item).inputType );
                    Tuple<int[], int[]> indices = new Tuple<int[], int[]>(read, write);
                    portIndices.Add(indices);
                }
            }


            // iterate connections
            while (connections.Count > 0)
            {
                AI_Connection con = connections[0];

                if (con.getTarget() != null)
                {
                    // check all sources fulfilled
                    bool ready = true;
                    AI_Item targetItem = con.getTarget().item;
                    foreach (AI_ItemPort targetInputPort in targetItem.inputs)
                    {

                    }

                    foreach (AI_ItemPort port in con.getTarget().item.outputs)
                    {
                        connections.AddRange(port.connections);
                    }
                }

                connections.RemoveAt(0);
            }
        }

    }

}
