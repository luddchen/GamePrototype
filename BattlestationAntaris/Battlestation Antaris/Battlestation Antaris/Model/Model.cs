using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.Model
{

    public abstract class Model
    {

        public Controller controller;

        public Model(Controller controller)
        {
            this.controller = controller;
        }

        public abstract void Initialize(ContentManager content);

        public abstract void Update(Microsoft.Xna.Framework.GameTime gameTime);

    }

}
