using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.Model
{
    public class SpaceStation : SpatialObject
    {

        ModelBone StationAxis;
        Matrix StationAxisTransform;
        float AxisRot = 0.0f;

        public SpaceStation(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content) 
        {
            init();
        }

        public SpaceStation(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) 
        {
            init();
        }

        private void init()
        {
            Console.Out.WriteLine("Station Bounding Sphere : " + this.bounding + " (" + this.model3d.Meshes.Count + " meshes)");

            StationAxis = model3d.Bones["StationAxis"];
            StationAxisTransform = StationAxis.Transform;

            rotateX((float)(-Math.PI/2));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            AxisRot += (float)(Math.PI / 1440);

            StationAxis.Transform = Matrix.CreateRotationZ(AxisRot) * StationAxisTransform;
        }

    }
}
