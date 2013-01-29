using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Battlestation_Antaris.Model
{

    public class SpatialObject
    {
        public Microsoft.Xna.Framework.Graphics.Model model3d;

        public Matrix[] boneTransforms;

        public float speed;

        public Matrix rotation;

        public Vector3 globalPosition;

        public bool draw;

        public Bounding bounding;

        public SpatialObject(Vector3 position, String modelName, ContentManager content)
        {
            this.draw = true;
            this.globalPosition = position;
            this.rotation = Matrix.Identity;
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>(modelName);
            this.boneTransforms = new Matrix[model3d.Bones.Count];
        }

        public virtual void Update(GameTime gameTime)
        {
            this.globalPosition += Vector3.Multiply(rotation.Forward, speed);
        }

        public void rotateX(float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(rotation.Right, angle);
            rotation = rotation * axisRotation;
        }

        public void rotateY(float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(rotation.Forward, angle);
            rotation = rotation * axisRotation;
        }

        public void rotateZ(float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(rotation.Up, angle);
            rotation = rotation * axisRotation;
        }
    }
}

