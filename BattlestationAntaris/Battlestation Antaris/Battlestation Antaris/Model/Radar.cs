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
        /// create a radar outside the world
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        public Radar(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content){}


        /// <summary>
        /// create a new radar inside the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public Radar(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) { }


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
