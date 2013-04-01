using System;
using Microsoft.Xna.Framework;

namespace SpatialObjectAttributesLibrary {

    /// <summary>
    /// basis class for Spatial Object Attributes : Weapons
    /// </summary>
    public class Weapon : AttributeItem {

        public float Damage;

        public float Range;

        public float ProjectileVelocity;

        public float ReloadTime;

        public float CurrentReloadTime;


        /// <summary>
        /// creates a new SOA Weapon
        /// all Values initialized by zero
        /// </summary>
        public Weapon() {
            this.name = "Weapon";
            this.Damage = 0;
            this.Range = 0;
            this.ProjectileVelocity = 0;
            this.ReloadTime = 0;
            this.CurrentReloadTime = 0;
        }


        /// <summary>
        /// copy constructor
        /// </summary>
        /// <param name="weapon"></param>
        public Weapon( Weapon weapon ) {
            this.name = "Weapon";
            this.Damage = weapon.Damage;
            this.Range = weapon.Range;
            this.ProjectileVelocity = weapon.ProjectileVelocity;
            this.ReloadTime = weapon.ReloadTime;
            this.CurrentReloadTime = weapon.CurrentReloadTime;
        }


        /// <summary>
        /// creates a new SOA Weapon
        /// </summary>
        /// <param name="damage">damage</param>
        /// <param name="range">range</param>
        /// <param name="projectileVelocity">projectile velocity</param>
        public Weapon( float damage, float range, float projectileVelocity, float reloadTime ) {
            this.name = "Weapon";
            this.Damage = damage;
            this.Range = range;
            this.ProjectileVelocity = projectileVelocity;
            this.ReloadTime = reloadTime;
            this.CurrentReloadTime = 0;
        }


        public void set( float damage, float range, float projectileVelocity, float reloadTime ) {
            this.Damage = damage;
            this.Range = range;
            this.ProjectileVelocity = projectileVelocity;
            this.ReloadTime = reloadTime;
            this.CurrentReloadTime = 0;
        }

        public override void setValues( float[] values, ref int index ) {
            this.Damage = values[index++];
            this.Range = values[index++];
            this.ProjectileVelocity = values[index++];
            this.ReloadTime = values[index++];
        }

        public override float[] getValues() {
            float[] values = new float[4];
            values[0] = this.Damage;
            values[1] = this.Range;
            values[2] = this.ProjectileVelocity;
            values[3] = this.ReloadTime;

            return values;
        }

        public override int getNumberOfValues() {
            return 4;
        }

        public void UpdateReloadTime( GameTime gameTime ) {
            if ( this.CurrentReloadTime > 0 ) {
                this.CurrentReloadTime--;
            } else {
                this.CurrentReloadTime = 0;
            }
        }

        public virtual bool Fire() {
            bool success = false;
            if ( this.CurrentReloadTime <= 0 ) {
                this.CurrentReloadTime = this.ReloadTime;
                success = true;
            }
            return success;
        }

        public override string ToString() {
            String output = "";
            output += this.name + ":Damage = " + this.Damage + "\n";
            output += this.name + ":Range = " + this.Range + "\n";
            output += this.name + ":ProjectileVelocity = " + this.ProjectileVelocity + "\n";
            output += this.name + ":ReloadTime = " + this.ReloadTime + "\n";
            output += this.name + ":CurrentReloadTime = " + this.CurrentReloadTime + "\n";

            return output;
        }

    }

}
