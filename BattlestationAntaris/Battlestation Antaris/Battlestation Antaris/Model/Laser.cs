using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;

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
            this.miniMapIcon.Texture = content.Load<Texture2D>("Models//Weapon//laser_2d");
            this.miniMapIcon.color = MiniMap.WEAPON_COLOR;
            this.miniMapIcon.scale = 0.4f;
        }


        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            this.timeout--;

            if (this.timeout == 0)
            {
                this.world.removeObject(this);
                this.miniMapIcon.RemoveFromWorld();
                this.miniMapIcon = null;
            }
        }


        public override string ToString()
        {
            return "Laser";
        }

    }

}
