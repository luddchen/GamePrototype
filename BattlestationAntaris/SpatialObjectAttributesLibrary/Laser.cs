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

    }
}

