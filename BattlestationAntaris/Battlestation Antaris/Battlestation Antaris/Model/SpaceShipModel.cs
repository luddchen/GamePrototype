using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    public class SpaceShipModel : Model
    {
        public SpatialObject ship;

        public SpaceShipModel(Controller controller) : base(controller)
        {
        }

        public override void Initialize(ContentManager content)
        {
            this.ship = new SpatialObject(new Vector3(0, 0, 0), "Models/compass", content);
            this.ship.draw = false;
            this.controller.world.allObjects.Add(ship);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

    }

}
