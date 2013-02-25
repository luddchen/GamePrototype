using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using SpatialObjectAttributesLibrary;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// the players space ship
    /// </summary>
    public class SpaceShip : SpatialObject
    {

        /// <summary>
        /// create a new space ship within the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpaceShip(Vector3 position, String modelName, ContentManager content, WorldModel world)
            : base(position, modelName, content, world)
        {
            init(content);
        }


        /// <summary>
        /// update the space ship
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            if (this.attributes.Missile.CurrentReloadTime > 0)
            {
                this.attributes.Missile.CurrentReloadTime--;
            }
        }


        public override void InjectControl(Control.Control control)
        {
            base.InjectControl(control);

            if (control == Control.Control.FIRE_LASER)
            {
                Laser laser = new Laser(this, -2.0f, this.world.game.Content, this.world);
            }

            if (control == Control.Control.FIRE_MISSILE)
            {
                if (this.attributes.Missile.CurrentReloadTime <= 0)
                {
                    Missile missile = new Missile(this, -2.0f, this.world.game.Content, this.world);
                    this.attributes.Missile.CurrentReloadTime = this.attributes.Missile.ReloadTime;
                }
            }
        }


        /// <summary>
        /// initialize the space ship
        /// </summary>
        private void init(ContentManager content)
        {
            this.attributes = new SpatialObjectAttributes( content.Load<SpatialObjectAttributes>("Attributes//SpaceShip") );
        }

    }

}
