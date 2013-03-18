using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.View {

    /// <summary>
    /// a class that holds the necessary matrices for 3D projection
    /// </summary>
    public class Camera {

        /// <summary>
        /// the projection matrix
        /// </summary>
        public Matrix projection;


        /// <summary>
        /// the view matrix
        /// </summary>
        public Matrix view;


        /// <summary>
        /// near clipping value
        /// </summary>
        public float nearClipping = 1;


        /// <summary>
        /// far clipping value
        /// </summary>
        public float farClipping = 10000;


        /// <summary>
        /// create a new camera
        /// </summary>
        /// <param name="device">game graphics device</param>
        public Camera() {
            Update( Vector3.Zero, Vector3.Forward, Vector3.Up );  // set matrices to standard
        }


        /// <summary>
        /// update the camera matrices to a new position and direction
        /// </summary>
        /// <param name="position">new position</param>
        /// <param name="direction">new direction</param>
        /// <param name="up">new up vector</param>
        public void Update( Vector3 position, Vector3 direction, Vector3 up ) {
            projection = Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4, Antares.graphics.GraphicsDevice.Viewport.AspectRatio, this.nearClipping, this.farClipping );
            view = Matrix.CreateLookAt( position, Vector3.Add( position, direction ), up );
        }


        /// <summary>
        /// creates the camera view from the perspective of a spatial object
        /// </summary>
        /// <param name="obj">a spatial object</param>
        public void ClampTo( SpatialObject obj ) {
            Update( obj.globalPosition, obj.rotation.Forward, obj.rotation.Up );
        }

    }

}
