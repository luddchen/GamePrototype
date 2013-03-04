using Battlestation_Antaris.Tools;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antaris.Model
{
    public class Dust : SpatialObject
    {
        const int MAX_PARENT_DIST = 1000;

        SpatialObject parent;

        public Dust(SpatialObject parent, ContentManager content, WorldModel world)
            : base(parent.globalPosition, "Models//Dust//dust", content, world)
        {
            this.parent = parent;
            setRandomPos();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float distToParent = Vector3.Distance(parent.globalPosition, this.globalPosition);
            if (distToParent > (MAX_PARENT_DIST))
            {
                setRandomPos();
            }
        }

        private void setRandomPos()
        {
            // create new dust in front of ship (depending on parents moving direction)
            Vector3 localOffset = this.parent.rotation.Forward * MAX_PARENT_DIST * (float)(RandomGen.random.NextDouble() * 2 - 0.9f);

            // move random right/left (depending on parents moving direction)
            localOffset += this.parent.rotation.Right * (float)(RandomGen.random.NextDouble() - 0.5f) * MAX_PARENT_DIST * 0.4f;

            // move random up/down (depending on parents moving direction)
            localOffset += this.parent.rotation.Up * (float)(RandomGen.random.NextDouble() - 0.5f) * MAX_PARENT_DIST * 0.4f;


            this.globalPosition = parent.globalPosition + localOffset;
        }


        public override string ToString()
        {
            return "Dust";
        }
    }
}
