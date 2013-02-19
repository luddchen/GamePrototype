using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// the players space ship
    /// </summary>
    public class SpaceShip : SpatialObject
    {

        /// <summary>
        /// create a new space ship outside the world
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        public SpaceShip(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content) 
        {
            init();
        }


        /// <summary>
        /// create a new space ship within the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpaceShip(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) 
        {
            init();
        }


        /// <summary>
        /// update the space ship
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }


        /// <summary>
        /// initialize the space ship
        /// </summary>
        private void init()
        {
            // create test attributes
            this.attributes.Engine.Acceleration = 0.01f;
            this.attributes.Engine.MaxVelocity = 1.0f;

            this.attributes.EngineRoll.Acceleration = (float)(Math.PI / 14400);
            this.attributes.EngineRoll.MaxVelocity = (float)(Math.PI / 360);

            this.attributes.EnginePitch.Acceleration = (float)(Math.PI / 14400);
            this.attributes.EnginePitch.MaxVelocity = (float)(Math.PI / 360);

            this.attributes.EngineYaw.Acceleration = (float)(Math.PI / 14400);
            this.attributes.EngineYaw.MaxVelocity = (float)(Math.PI / 360);
        }

    }

}
