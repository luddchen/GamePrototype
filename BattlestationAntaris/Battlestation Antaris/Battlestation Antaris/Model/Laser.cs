using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.Model
{

    public class Laser : SpatialObject
    {

        private int timeout;

        private SpatialObject parent;

        private float length = 0.0f;

        private float maxLength = 250.0f;

        private float upOffset;

        private float rightOffset;

        private float forwardOffset = 0;

        public Laser(SpatialObject parent, float upOffset, float rightOffset, ContentManager content, WorldModel world)
            : base(parent.globalPosition, "Models//Weapon//laser", content, world)
        {
            this.parent = parent;
            this.rotation = parent.rotation;
            this.attributes.Engine.CurrentVelocity = 30.0f;
            this.upOffset = upOffset;
            this.rightOffset = rightOffset;

            this.timeout = 60;

            this.miniMapIcon.Texture = content.Load<Texture2D>("Models//Weapon//laser_2d");
            this.miniMapIcon.color = MiniMap.WEAPON_COLOR;
            this.miniMapIcon.scale = 0.4f;
        }


        public override void Update(GameTime gameTime)
        {
            this.rotation = this.parent.rotation;
            this.scale.Z = -this.length;
            this.globalPosition = this.parent.globalPosition
                                + this.rotation.Forward * (this.length / 2 + this.forwardOffset)
                                + this.rotation.Up * this.upOffset
                                + this.rotation.Right * this.rightOffset;
                                

            this.timeout--;
            if (this.length < this.maxLength)
            {
                this.length += 50.0f;
            }
            else
            {
                this.forwardOffset += this.attributes.Engine.CurrentVelocity;
            }

            if (this.timeout == 0)
            {
                this.world.Remove(this);
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
