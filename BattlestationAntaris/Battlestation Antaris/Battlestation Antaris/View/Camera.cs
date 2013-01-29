using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.View
{

    public class Camera
    {
        GraphicsDevice device;

        public Matrix projection;
        public Matrix view;
        public int nearClipping = 1;
        public int farClipping = 5000;

        public Camera(GraphicsDevice device)
        {
            this.device = device;
            Update(Vector3.Zero, Vector3.UnitY, Vector3.UnitZ);
        }

        public void Update(Vector3 position, Vector3 direction, Vector3 up)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 2, this.device.Viewport.AspectRatio, this.nearClipping, this.farClipping);
            view = Matrix.CreateLookAt(position, Vector3.Add(position, direction), up);
        }

        public void ClampTo(SpatialObject obj)
        {
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 2, this.device.Viewport.AspectRatio, this.nearClipping, this.farClipping);
            view = Matrix.CreateLookAt(obj.globalPosition, Vector3.Add(obj.globalPosition, obj.rotation.Forward), obj.rotation.Up);
        }

    }

}
