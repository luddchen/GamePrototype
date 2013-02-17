using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class Engine
    {
        public float MaxVelocity;

        public float CurrentVelocity;

        public float Acceleration;

        public float ResetForce;

        public Engine()
        {
            this.MaxVelocity = 0;
            this.CurrentVelocity = 0;
            this.Acceleration = 0;
            this.ResetForce = 0;
        }

        public Engine(float maxVelocity, float acceleration, float resetForce)
        {
            this.MaxVelocity = maxVelocity;
            this.CurrentVelocity = 0;
            this.Acceleration = acceleration;
            this.ResetForce = resetForce;
        }

    }

}
