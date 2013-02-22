using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class Laser : Weapon
    {

        public float HeatProduction;

        public float HeatUntilCooldown;

        public float CurrentHeat;

        public float HeatRegeneration;


        public Laser() : base() 
        {
            this.HeatProduction = 0;
            this.HeatUntilCooldown = 0;
            this.CurrentHeat = 0;
            this.HeatRegeneration = 0;
        }

        public Laser(float damage, float range, float projectileVelocity, float heatProduction, float heatUntilCooldown, float heatRegeneration)
            : base(damage, range, projectileVelocity)
        {
            this.HeatProduction = heatProduction;
            this.HeatUntilCooldown = heatUntilCooldown;
            this.CurrentHeat = 0;
            this.HeatRegeneration = heatRegeneration;
        }


        public void set(float damage, float range, float projectileVelocity, float heatProduction, float heatUntilCooldown, float heatRegeneration)
        {
            base.set(damage, range, projectileVelocity);

            this.HeatProduction = heatProduction;
            this.HeatUntilCooldown = heatUntilCooldown;
            this.CurrentHeat = 0;
            this.HeatRegeneration = heatRegeneration;
        }

        public void set(Laser laser)
        {
            base.set(laser);

            this.HeatProduction = laser.HeatProduction;
            this.HeatUntilCooldown = laser.HeatUntilCooldown;
            this.CurrentHeat = laser.CurrentHeat;
            this.HeatRegeneration = laser.HeatRegeneration;
        }

        public override void setValues(float[] values, ref int index)
        {
            base.setValues(values, ref index);

            this.HeatProduction = values[index++];
            this.HeatUntilCooldown = values[index++];
            this.CurrentHeat = 0;
            this.HeatRegeneration = values[index++];
        }

        public override float[] getValues()
        {
            float[] values1 = base.getValues();
            float[] values2 = new float[6];
            Array.Copy(values1, 0, values2, 0, 3);

            values2[3] = this.HeatProduction;
            values2[4] = this.HeatUntilCooldown;
            values2[5] = this.HeatRegeneration;

            return values2;
        }

        public override int getNumberOfValues()
        {
            return base.getNumberOfValues() + 3;
        }

    }
}

