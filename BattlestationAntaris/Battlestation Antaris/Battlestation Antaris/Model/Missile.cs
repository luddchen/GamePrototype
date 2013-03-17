using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antaris.Model
{

    public class Missile : SpatialObject
    {
        private int timeout;

        public Missile(SpatialObject parent, float offset, ContentManager content, WorldModel world)
            : base(parent.globalPosition, "Models//Weapon//missile", content, world)
        {
            this.rotation = parent.rotation;
            this.attributes.Engine.CurrentVelocity = 10.0f;

            this.timeout = 240;

            this.globalPosition = Vector3.Add(this.globalPosition, Vector3.Multiply(this.rotation.Up, offset));
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.timeout--;

            if (this.timeout == 0)
            {
                this.world.Remove(this);
            }
        }


        public override string ToString()
        {
            return "Missile";
        }

    }

}