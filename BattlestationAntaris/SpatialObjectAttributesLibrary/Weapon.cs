using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    /// <summary>
    /// basis class for Spatial Object Attributes : Weapons
    /// </summary>
    public class Weapon : AttributeItem
    {

        public float Damage;

        public float Range;

        public float ProjectileVelocity;


        /// <summary>
        /// creates a new SOA Weapon
        /// all Values initialized by zero
        /// </summary>
        public Weapon()
        {
            this.Damage = 0;
            this.Range = 0;
            this.ProjectileVelocity = 0;
        }


        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="weapon"></param>
        public Weapon(Weapon weapon)
        {
            this.Damage = weapon.Damage;
            this.Range = weapon.Range;
            this.ProjectileVelocity = weapon.ProjectileVelocity;
        }


        /// <summary>
        /// creates a new SOA Weapon
        /// </summary>
        /// <param name="damage">damage</param>
        /// <param name="range">range</param>
        /// <param name="projectileVelocity">projectile velocity</param>
        public Weapon(float damage, float range, float projectileVelocity)
        {
            this.Damage = damage;
            this.Range = range;
            this.ProjectileVelocity = projectileVelocity;
        }


        public void set(float damage, float range, float projectileVelocity)
        {
            this.Damage = damage;
            this.Range = range;
            this.ProjectileVelocity = projectileVelocity;
        }

        public override void setValues(float[] values, ref int index)
        {
            this.Damage = values[index++];
            this.Range = values[index++];
            this.ProjectileVelocity = values[index++];
        }

        public override float[] getValues()
        {
            float[] values = new float[3];
            values[0] = this.Damage;
            values[1] = this.Range;
            values[2] = this.ProjectileVelocity;

            return values;
        }

        public override int getNumberOfValues()
        {
            return 3;
        }

    }

}
