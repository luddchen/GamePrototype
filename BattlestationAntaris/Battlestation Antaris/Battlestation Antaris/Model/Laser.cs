using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antaris.Model
{

    public class Laser : SpatialObject
    {

        private int timeout;

        public Laser(SpatialObject parent, float offset, ContentManager content, WorldModel world)
            : base(parent.globalPosition, "Models//Weapon//laser", content, world)
        {
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
                this.world.removeObject(this);
            }
        }

    }

}
