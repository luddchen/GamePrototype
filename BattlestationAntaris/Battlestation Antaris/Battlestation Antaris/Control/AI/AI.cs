using System;
using System.Collections.Generic;
using Battlestation_Antaris.View.HUD.AIComposer;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.Control.AI
{

    public class AI
    {

        List<AI_Input.InputType> inputs;

        List<AI_Item> items;

        List<Tuple<int[], int[]>> portIndices;

        int maxIndice;

        public SpatialObject source;

        public List<SpatialObject> targetObjects;

        public List<float> targetResults;


        public AI()
        {
            this.inputs = new List<AI_Input.InputType>();
            this.items = new List<AI_Item>();
            this.portIndices = new List<Tuple<int[], int[]>>();
            this.targetObjects = new List<SpatialObject>();
            this.targetResults = new List<float>();
        }

        public AI(AI ai)
        {
            this.inputs = new List<AI_Input.InputType>(ai.inputs);
            this.items = new List<AI_Item>(ai.items);
            this.portIndices = new List<Tuple<int[], int[]>>(ai.portIndices);
            this.maxIndice = ai.maxIndice;
            this.targetObjects = new List<SpatialObject>();
            this.targetResults = new List<float>();
        }

        public void Create(AI_Container aiContainer)
        {
            this.inputs.Clear();
            this.items.Clear();
            this.portIndices.Clear();
            // create lists for input ,connection , items
            List<AI_Connection> connections = new List<AI_Connection>();

            foreach(AI_Item item in aiContainer.aiItems) 
            {
                if (item is AI_Input)
                {
                    if (! this.inputs.Contains((AI_Input.InputType)item.subType))
                    {
                        this.inputs.Add((AI_Input.InputType)item.subType);
                    }
                    connections.AddRange(((AI_Input)item).outputs[0].connections);
                    items.Add(item);
                    int[] read = new int[0];
                    int[] write = new int[1];
                    write[0] = this.inputs.IndexOf((AI_Input.InputType)item.subType);
                    Tuple<int[], int[]> indices = new Tuple<int[], int[]>(read, write);
                    portIndices.Add(indices);
                }
            }
            this.maxIndice = this.inputs.Count;


            // iterate connections
            while (connections.Count > 0)
            {
                AI_Connection con = connections[0];

                if (con.getTarget() != null && !(this.items.Contains(con.getTarget().item)))
                {
                    // check all sources fulfilled
                    bool ready = true;

                    AI_Item targetItem = con.getTarget().item;
                    foreach (AI_ItemPort targetInputPort in targetItem.inputs)
                    {
                        foreach (AI_Connection targetInputConnection in targetInputPort.connections)
                        {
                            if (! this.items.Contains(targetInputConnection.getSource().item)) 
                            {
                                ready = false;
                                break;
                            }
                        }
                        if (!ready) 
                        { 
                            break; 
                        }
                    }

                    // if fulfilled
                    if (ready)
                    {
                        this.items.Add(targetItem);

                        int[] read = new int[targetItem.inputs.Count];
                        int[] write = new int[targetItem.outputs.Count];
                        for (int i = 0; i < targetItem.inputs.Count; i++)
                        {
                            int indiceValue = -1;
                            if (targetItem.inputs[i].connections.Count > 0)
                            {
                                AI_Connection inputConnection = targetItem.inputs[i].connections[0];
                                int portNumber = inputConnection.getSource().item.outputs.IndexOf(inputConnection.getSource());
                                int sourceItemNumber = this.items.IndexOf(inputConnection.getSource().item);
                                indiceValue = this.portIndices[sourceItemNumber].Item2[portNumber];
                            }
                            read[i] = indiceValue;
                        }

                        for (int i = 0; i < targetItem.outputs.Count; i++)
                        {
                            write[i] = this.maxIndice;
                            this.maxIndice++;
                        }

                        Tuple<int[], int[]> indices = new Tuple<int[], int[]>(read, write);
                        portIndices.Add(indices);

                        foreach (AI_ItemPort port in con.getTarget().item.outputs)
                        {
                            connections.AddRange(port.connections);
                        }
                    }
                }

                connections.RemoveAt(0);
            }
            Console.WriteLine(this.maxIndice + " : " + this.inputs.Count);
        }


        public void ThreadPoolCallback(Object threadContext)
        {
            this.targetResults.Clear();

            Console.WriteLine(this.maxIndice);

            foreach (SpatialObject target in this.targetObjects)
            {
                if (target == this.source || target is Dust)
                {
                    this.targetResults.Add(-1.0f);
                    continue;
                }

                float[] values = new float[this.maxIndice];

                // init inputs
                for (int i = 0; i < this.inputs.Count; i++ )
                {
                    switch (this.inputs[i])
                    {
                        case AI_Input.InputType.DISTANCE :
                            values[i] = ValueProvider.Distance(this.source, target, this.source.attributes.Laser.Range);
                            break;
                        case AI_Input.InputType.ORTHOGONAL_VELOCITY :
                            values[i] = ValueProvider.OrthogonalVelocity(this.source, target, this.source.attributes.Laser.ProjectileVelocity);
                            break;
                        case AI_Input.InputType.ROTATION :
                            values[i] = ValueProvider.Rotation(this.source, target);
                            break;
                        case AI_Input.InputType.SHIELD_HEALTH :
                            values[i] = ValueProvider.ShieldStatus(target);
                            break;
                        case AI_Input.InputType.HULL_HEALTH :
                            values[i] = ValueProvider.HullStatus(target);
                            break;
                    }
                }


                foreach (AI_Item item in this.items)
                {
                    Tuple<int[], int[]> tuple = this.portIndices[this.items.IndexOf(item)];

                    if (item is AI_Input)
                    {
                        continue;
                    }
                    if (item is AI_Transformer)
                    {
                        // simple copy for testing
                        values[tuple.Item2[0]] = values[tuple.Item1[0]];
                        continue;
                    }
                    if (item is AI_Mixer)
                    {
                        // simple avg for testing
                        values[tuple.Item2[0]] = (values[tuple.Item1[0]] + values[tuple.Item1[1]]) / 2;
                        continue;
                    }
                    if (item is AI_Output)
                    {
                        // assuming only one output, so last value in values is the result
                        continue;
                    }
                }

                // copy result
                this.targetResults.Add(values[values.Length - 1]);
            }
        }


        public override string ToString()
        {
            String outString = "Inputs = { ";

            foreach (AI_Input.InputType input in this.inputs)
            {
                outString += "( " + input + " ) ";
            }

            outString += "}\n";

            outString += "Items = \n"; 

            foreach (AI_Item item in this.items)
            {
                outString += "    " + item.GetType() + " : [ ";

                Tuple<int[], int[]> tuple = this.portIndices[this.items.IndexOf(item)];

                foreach (int value in tuple.Item1)
                {
                    outString += "(" + value + ") ";
                }
                outString += "] -> [ ";
                foreach (int value in tuple.Item2)
                {
                    outString += "(" + value + ") ";
                }

                outString += "]\n";
            }

            return outString;
        }

    }

}
