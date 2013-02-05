using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;


namespace Battlestation_Antaris.Model
{

    class CollisionObject
    {

        public SpatialObject spatialObject;
        public BoundingSphere movingSphere;

        public CollisionObject(SpatialObject obj)
        {
            this.spatialObject = obj;
            this.movingSphere = 
                BoundingSphere.CreateMerged(
                    obj.bounding.Transform(Matrix.CreateTranslation(obj.globalPosition)),
                    obj.bounding.Transform(Matrix.CreateTranslation(Vector3.Add( obj.globalPosition, Vector3.Multiply(obj.rotation.Forward, obj.speed) )))
                );
        }

    }

}
