using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antaris.Model
{
    public class SpaceStation : SpatialObject
    {

        public SpaceStation(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content) {}

        public SpaceStation(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            rotateZ((float)(Math.PI / 1440));
        }

    }
}
