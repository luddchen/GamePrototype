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

    }

}
