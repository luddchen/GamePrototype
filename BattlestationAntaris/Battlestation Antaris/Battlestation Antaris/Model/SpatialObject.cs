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

        public Vector3 globalPosition;

        public Bounding bounding;

        public SpatialObject(Vector3 position, String modelName, ContentManager content)
        {
            this.globalPosition = position;
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>(modelName);
            boneTransforms = new Matrix[model3d.Bones.Count];
        }

        public void Update(GameTime gameTime)
        {
        }
    }
}

