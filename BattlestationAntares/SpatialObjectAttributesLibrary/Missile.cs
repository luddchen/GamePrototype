using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    /// <summary>
    /// Spatial Object Attributes : Missile
    /// </summary>
    public class Missile : Weapon
    {

        public float MaxAmount;

        public float CurrentAmount;


        public Missile()
            : base()
        {
            this.name = "Missile";
            this.MaxAmount = 0;
            this.CurrentAmount = 0;
        }

        public Missile(Missile missile)
            : base(missile)
        {
            this.name = "Missile";
            this.MaxAmount = missile.MaxAmount;
            this.CurrentAmount = missile.CurrentAmount;
        }

        public Missile(float damage, float range, float projectileVelocity, float maxAmount, float reloadTime)
            : base(damage, range, projectileVelocity, reloadTime)
        {
            this.name = "Missile";
            this.MaxAmount = maxAmount;
            this.CurrentAmount = 0;
        }


        public void set(float damage, float range, float projectileVelocity, float maxAmount, float reloadTime)
        {
            base.set(damage, range, projectileVelocity, reloadTime);

            this.MaxAmount = maxAmount;
            this.CurrentAmount = 0;
        }


        public override void setValues(float[] values, ref int index)
        {
            base.setValues(values, ref index);

            this.MaxAmount = values[index++];
            this.CurrentAmount = 0;
        }

        public override float[] getValues()
        {
            float[] values1 = base.getValues();
            float[] values2 = new float[getNumberOfValues()];
            Array.Copy(values1, 0, values2, 0, base.getNumberOfValues());

            values2[base.getNumberOfValues()] = this.MaxAmount;

            return values2;
        }

        public override int getNumberOfValues()
        {
            return base.getNumberOfValues() + 1;
        }


        public override string ToString()
        {
            String output = base.ToString();
            output += this.name + ":MaxAmount = " + this.MaxAmount + "\n";
            output += this.name + ":CurrentAmount = " + this.CurrentAmount + "\n";

            return output;
        }

    }

}
