using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Tools
{

    /// <summary>
    /// a class with static methods for Matrix and Vector manipulation
    /// </summary>
    public class Tools
    {

        /// <summary>
        /// rotates an input matrix on the local matrix right-axis
        /// </summary>
        /// <param name="input">the input matrix</param>
        /// <param name="angle">the rotation angle</param>
        /// <returns>the rotated matrix</returns>
        public static Matrix Pitch(Matrix input, float angle)
        {
            // get local right-axis
            Matrix axisRotation = Matrix.CreateFromAxisAngle(input.Right, angle);

            // compute rotation
            return input * axisRotation;
        }


        /// <summary>
        /// rotates an input matrix on the local matrix forward-axis
        /// </summary>
        /// <param name="input">the input matrix</param>
        /// <param name="angle">the rotation angle</param>
        /// <returns>the rotated matrix</returns>
        public static Matrix Roll(Matrix input, float angle)
        {
            // get local forward-axis
            Matrix axisRotation = Matrix.CreateFromAxisAngle(input.Forward, angle);

            // compute rotation
            return input * axisRotation;
        }


        /// <summary>
        /// rotates an input matrix on the local matrix up-axis
        /// </summary>
        /// <param name="input">the input matrix</param>
        /// <param name="angle">the rotation angle</param>
        /// <returns>the rotated matrix</returns>
        public static Matrix Yaw(Matrix input, float angle)
        {
            // get local up-axis
            Matrix axisRotation = Matrix.CreateFromAxisAngle(input.Up, angle);

            // compute rotation
            return input * axisRotation;
        }


        /// <summary>
        /// rotates an input matrix on their loacal up-/right-/forward-axis
        /// in this order
        /// </summary>
        /// <param name="input">the input matrix</param>
        /// <param name="yaw">the rotation angle on up-axis</param>
        /// <param name="pitch">the rotation angle on right-axis</param>
        /// <param name="roll">the rotation angle on forward-axis</param>
        /// <returns>the rotated matrix</returns>
        public static Matrix YawPitchRoll(Matrix input, float yaw, float pitch, float roll)
        {
            return Yaw(Pitch(Roll(input, roll), pitch), yaw);
        }


        /// <summary>
        /// computes the rotation of a global target vector in relation to a local rotation matrix
        /// </summary>
        /// <param name="targetVector">the global target vector</param>
        /// <param name="localRotation">the local rotation matrix</param>
        /// <returns>the rotation on up-axis (Vector3.Z) and on right-axis (Vector3.X)</returns>
        public static Vector3 GetRotation(Vector3 targetVector, Matrix localRotation)
        {
            Vector3 rotation = new Vector3();

            // project global target vector to local target vector
            double forward = Vector3.Dot(targetVector, localRotation.Forward);
            double right = Vector3.Dot(targetVector, localRotation.Right);
            double up = Vector3.Dot(targetVector, localRotation.Up);

            // compute rotation on up-axis
            rotation.Z = (float)Math.Atan2(forward, right);

            // compute length of local target vector after projection on forward-right-plane 
            double planeDist = Math.Sqrt(forward * forward + right * right);

            // compute rotation on right-axis
            rotation.X  = (float)Math.Atan2(planeDist, up);

            return rotation;
        }

    }

}
