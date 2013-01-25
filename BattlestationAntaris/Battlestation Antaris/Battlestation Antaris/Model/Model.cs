using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Model
{

    abstract class Model
    {

        public abstract void Init();

        public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);

    }

}
