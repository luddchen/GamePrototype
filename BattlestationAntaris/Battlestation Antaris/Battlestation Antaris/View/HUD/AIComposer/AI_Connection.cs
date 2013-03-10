using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Connection
    {

        AI_ItemPort source;

        AI_ItemPort target;


        public void setSource(AI_ItemPort source)
        {
            if (this.source != null)
            {
                this.source.connections.Remove(this);
            }
            this.source = source;
            this.source.connections.Add(this);
        }

        public void setTarget(AI_ItemPort target)
        {
            if (this.target != null)
            {
                this.target.connections.Remove(this);
            }
            this.target = target;
            this.target.connections.Add(this);
        }

        public AI_ItemPort getSource()
        {
            return this.source;
        }

        public AI_ItemPort getTarget()
        {
            return this.target;
        }

        public void Delete()
        {
            if (this.source != null)
            {
                this.source.connections.Remove(this);
                this.source = null;
            }

            if (this.target != null)
            {
                this.target.connections.Remove(this);
                this.target = null;
            }
        }

    }

}
