using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class Engine : AttributeItem
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


        /// <summary>
        /// apply the reset force
        /// </summary>
        public void ApplyResetForce()
        {
            if (this.CurrentVelocity >= this.ResetForce)
            {
                this.CurrentVelocity -= this.ResetForce;
            }
            else if (this.CurrentVelocity <= -this.ResetForce)
            {
                this.CurrentVelocity += this.ResetForce;
            }
            else
            {
                this.CurrentVelocity = 0;
            }
        }


        public void set(float maxVelocity, float acceleration, float resetForce)
        {
            this.MaxVelocity = maxVelocity;
            this.CurrentVelocity = 0;
            this.Acceleration = acceleration;
            this.ResetForce = resetForce;
        }

        public override void setValues(float[] values, ref int index)
        {
            this.MaxVelocity = values[index++];
            this.CurrentVelocity = 0;
            this.Acceleration = values[index++];
            this.ResetForce = values[index++];
        }

        public override float[] getValues()
        {
            float[] values = new float[3];
            values[0] = this.MaxVelocity;
            values[1] = this.Acceleration;
            values[2] = this.ResetForce;

            return values;
        }

        public override int getNumberOfValues()
        {
            return 3;
        }

    }

}
