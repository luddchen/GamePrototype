using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// a dangerous turret
    /// </summary>
    public class Turret : SpatialObject
    {

        /// <summary>
        /// create a new turret outside of the world
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        public Turret(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content) {}


        /// <summary>
        /// create a new turret and insert into the world
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public Turret(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) { }


        /// <summary>
        /// update the turret
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }

    }

}
