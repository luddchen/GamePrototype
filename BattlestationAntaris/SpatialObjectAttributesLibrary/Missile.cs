using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class Missile : Weapon
    {

        public float MaxAmount;

        public float CurrentAmount;

        public float ReloadTime;

        public float CurrentReloadTime;


        public Missile()
            : base()
        {
            this.MaxAmount = 0;
            this.CurrentAmount = 0;
            this.ReloadTime = 0;
            this.CurrentReloadTime = 0;
        }

        public Missile(float damage, float range, float projectileVelocity, float maxAmount, float reloadTime)
            : base(damage, range, projectileVelocity)
        {
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

    }

}
