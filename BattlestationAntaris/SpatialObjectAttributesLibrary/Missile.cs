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

        public float ReloadTime;

        public float CurrentReloadTime;


        public Missile()
            : base()
        {
            this.name = "Missile";
            this.MaxAmount = 0;
            this.CurrentAmount = 0;
            this.ReloadTime = 0;
            this.CurrentReloadTime = 0;
        }

        public Missile(Missile missile)
            : base(missile)
        {
            this.name = "Missile";
            this.MaxAmount = missile.MaxAmount;
            this.CurrentAmount = missile.CurrentAmount;
            this.ReloadTime = missile.ReloadTime;
            this.CurrentReloadTime = missile.CurrentReloadTime;
        }

        public Missile(float damage, float range, float projectileVelocity, float maxAmount, float reloadTime)
            : base(damage, range, projectileVelocity)
        {
            this.name = "Missile";
            this.MaxAmount = maxAmount;
            this.CurrentAmount = 0;
            this.ReloadTime = reloadTime;
            this.CurrentReloadTime = 0;
        }


        public void set(float damage, float range, float projectileVelocity, float maxAmount, float reloadTime)
        {
            base.set(damage, range, projectileVelocity);

            this.MaxAmount = maxAmount;
            this.CurrentAmount = 0;
            this.ReloadTime = reloadTime;
            this.CurrentReloadTime = 0;
        }


        public override void setValues(float[] values, ref int index)
        {
            base.setValues(values, ref index);

            this.MaxAmount = values[index++];
            this.CurrentAmount = 0;
            this.ReloadTime = values[index++];
            this.CurrentReloadTime = 0;
        }

        public override float[] getValues()
        {
            float[] values1 = base.getValues();
            float[] values2 = new float[5];
            Array.Copy(values1, 0, values2, 0, 3);

            values2[3] = this.MaxAmount;
            values2[4] = this.ReloadTime;

            return values2;
        }

        public override int getNumberOfValues()
        {
            return base.getNumberOfValues() + 2;
        }


        public override string ToString()
        {
            String output = base.ToString();
            output += this.name + ":MaxAmount = " + this.MaxAmount + "\n";
            output += this.name + ":CurrentAmount = " + this.CurrentAmount + "\n";
            output += this.name + ":ReloadTime = " + this.ReloadTime + "\n";
            output += this.name + ":RCurrentReloadTime = " + this.CurrentReloadTime + "\n";

            return output;
        }

    }

}
