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
            if (distToParent > MAX_PARENT_DIST)
            {
                setRandomPos();
            }
        }

        private void setRandomPos()
        {
            this.globalPosition = parent.globalPosition + new Vector3(
                RandomGen.random.Next(MAX_PARENT_DIST) - MAX_PARENT_DIST / 2,
                RandomGen.random.Next(MAX_PARENT_DIST) - MAX_PARENT_DIST / 2,
                RandomGen.random.Next(MAX_PARENT_DIST) - MAX_PARENT_DIST / 2);
        }
    }
}
