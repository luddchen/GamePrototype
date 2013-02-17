using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class Weapon
    {

        public float Damage;

        public float Range;

        public float ProjectileVelocity;

        public Weapon()
        {
            this.Damage = 0;
            this.Range = 0;
            this.ProjectileVelocity = 0;
        }

        public Weapon(float damage, float range, float projectileVelocity)
        {
            this.Damage = damage;
            this.Range = range;
            this.ProjectileVelocity = projectileVelocity;
        }

    }

}
