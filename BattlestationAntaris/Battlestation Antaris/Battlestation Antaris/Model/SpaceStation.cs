using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// the Antaris space station
    /// </summary>
    public class SpaceStation : SpatialObject
    {

        /// <summary>
        /// model bone of the rotating part of the station
        /// </summary>
        private ModelBone StationAxis;


        /// <summary>
        /// the transformation matrix of the rotating part of the station
        /// </summary>
        Matrix StationAxisTransform;


        /// <summary>
        /// the current rotation value of the rotating part of the station
        /// </summary>
        float AxisRot = 0.0f;


        /// <summary>
        /// create a new space station outside the world
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        public SpaceStation(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content) 
        {
            init();
        }


        /// <summary>
        /// create a new space station within the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpaceStation(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) 
        {
            init();
        }


        /// <summary>
        /// init the space station
        /// </summary>
        private void init()
        {
            // test output of bounding sphere
            // Console.Out.WriteLine("Station Bounding Sphere : " + this.bounding + " (" + this.model3d.Meshes.Count + " meshes)");

            StationAxis = model3d.Bones["StationAxis"];
            StationAxisTransform = StationAxis.Transform;

            // initial rotation of full station, dont know why this is necessary
            this.rotation = Tools.Tools.Pitch(this.rotation, (float)(-Math.PI/2));
        }


        /// <summary>
        /// update the space station
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);

            // update rotation of the rotating part
            AxisRot += (float)(Math.PI / 1440);

            StationAxis.Transform = Matrix.CreateRotationZ(AxisRot) * StationAxisTransform;
        }

    }
}
