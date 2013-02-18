using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Tools
{

    public class Tools
    {

        public static Matrix Pitch(Matrix input, float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(input.Right, angle);
            return input * axisRotation;
        }

        public static Matrix Roll(Matrix input, float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(input.Forward, angle);
            return input * axisRotation;
        }

        public static Matrix Yaw(Matrix input, float angle)
        {
            Matrix axisRotation = Matrix.CreateFromAxisAngle(input.Up, angle);
            return input * axisRotation;
        }

        public static Matrix YawPitchRoll(Matrix input, float yaw, float pitch, float roll)
        {
            return Yaw(Pitch(Roll(input, roll), pitch), yaw);
        }

    }

}
