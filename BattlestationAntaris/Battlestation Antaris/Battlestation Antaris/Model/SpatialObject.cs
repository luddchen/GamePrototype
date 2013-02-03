using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

        public BoundingSphere bounding;

        public SpatialObject(Vector3 position, String modelName, ContentManager content)
        {
            this.draw = true;
            this.globalPosition = position;
            this.rotation = Matrix.Identity;
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>(modelName);
            this.boneTransforms = new Matrix[model3d.Bones.Count];
            this.speed = 0.0f;

            // experimental bounding stuff --------------------------
            this.bounding = new BoundingSphere();
            this.model3d.CopyAbsoluteBoneTransformsTo(this.boneTransforms);
            foreach (ModelMesh mesh in model3d.Meshes)
            {
                this.bounding = BoundingSphere.CreateMerged( mesh.BoundingSphere.Transform(this.boneTransforms[mesh.ParentBone.Index]), this.bounding);
            }
            // -------------------------------------------
        }

        public SpatialObject(Vector3 position, String modelName, ContentManager content, WorldModel world) : this(position, modelName, content)
        {
            world.allObjects.Add(this);
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

