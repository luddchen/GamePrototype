using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antaris.Model
{

    public class Laser : SpatialObject
    {

        private WorldModel wold;

        private int timeout;

        public Laser(SpatialObject parent, float offset, ContentManager content, WorldModel world)
            : base(parent.globalPosition, "Models//Weapon//laser", content)
        {
            this.wold = world;
            this.wold.allLaserBeams.Add(this);

            this.rotation = parent.rotation;
            this.attributes.Engine.CurrentVelocity = 15.0f;

            this.timeout = 360;

            this.globalPosition = Vector3.Add(this.globalPosition, Vector3.Multiply(this.rotation.Up, offset));
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.timeout--;

            if (this.timeout == 0)
            {
                this.wold.removeObject(this);
            }
        }

    }

}
