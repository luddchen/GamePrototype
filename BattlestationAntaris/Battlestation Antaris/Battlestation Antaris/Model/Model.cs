using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antaris.Model
{

    public abstract class Model
    {

        public abstract void Initialize(ContentManager content);

        public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);

    }

}
