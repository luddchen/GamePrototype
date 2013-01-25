using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    abstract class SpatialObject
    {

        public Vector3 globalPosition;

        public Bounding bounding;

        public abstract void Update(GameTime gameTime);

        public abstract void Draw();
    }
}

