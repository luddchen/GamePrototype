using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// a radar station
    /// </summary>
    public class Radar : SpatialObject
    {

        /// <summary>
        /// create a new radar inside the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public Radar(Vector3 position, ContentManager content, WorldModel world) : base(position, "Models//Radar//radar", content, world) 
        {
            Random random = new Random((int)position.X);

            int pitch = random.Next(2);
            this.attributes.EnginePitch.MaxVelocity = (pitch == 0) ? -0.02f : 0.02f;
            this.attributes.EnginePitch.CurrentVelocity = (pitch == 0) ? -0.02f : 0.02f; ;

            int yaw = random.Next(2);
            this.attributes.EngineYaw.MaxVelocity = (yaw == 0) ? -0.02f : 0.02f;
            this.attributes.EngineYaw.CurrentVelocity = (yaw == 0) ? -0.02f : 0.02f;

            int roll = random.Next(2);
            this.attributes.EngineYaw.MaxVelocity = (roll == 0) ? -0.02f : 0.02f;
            this.attributes.EngineYaw.CurrentVelocity = (roll == 0) ? -0.02f : 0.02f;
        }


        /// <summary>
        /// update the radar
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }

}
