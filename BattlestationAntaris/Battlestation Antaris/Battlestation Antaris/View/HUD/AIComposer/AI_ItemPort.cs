using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_ItemPort
    {
        public enum PortType
        {
            INPUT,
            OUTPUT
        }

        public PortType portType;

        public AI_Item item;

        public List<AI_Connection> connections;


        public AI_ItemPort(PortType portType, AI_Item item)
        {
            this.portType = portType;
            this.item = item;
            this.connections = new List<AI_Connection>();
        }

        public void Add(AI_Connection c) 
        {
            if (!(this.portType == PortType.INPUT && this.connections.Count > 0))
            {
                switch (this.portType)
                {
                    case PortType.INPUT :
                        c.setTarget(this);
                        break;
                    case PortType.OUTPUT :
                        c.setSource(this);
                        break;
                }
            }
        }


        public void Remove(AI_Connection c)
        {
            if (this.connections.Contains(c))
            {
                switch (this.portType)
                {
                    case PortType.INPUT:
                        c.setTarget(null);
                        break;
                    case PortType.OUTPUT:
                        c.setSource(null);
                        break;
                }
            }
        }

    }

}
