using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    public class SpaceShip : SpatialObject
    {

        public SpaceShip(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content) { }

        public SpaceShip(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }

}
