using System;
using Microsoft.Xna.Framework;

namespace SpatialObjectAttributesLibrary {

    /// <summary>
    /// Spatial Object Attributes : Laser
    /// </summary>
    public class Laser : Weapon {

        public float HeatProduction;

        public float HeatUntilCooldown;

        public float CurrentHeat;

        public float HeatRegeneration;


        public Laser()
            : base() {
            this.name = "Laser";
            this.HeatProduction = 0;
            this.HeatUntilCooldown = 0;
            this.CurrentHeat = 0;
            this.HeatRegeneration = 0;
        }

        public Laser( Laser laser )
            : base( laser ) {
            this.name = "Laser";
            this.HeatProduction = laser.HeatProduction;
            this.HeatUntilCooldown = laser.HeatUntilCooldown;
            this.CurrentHeat = laser.CurrentHeat;
            this.HeatRegeneration = laser.HeatRegeneration;
        }

        public Laser( float damage, float range, float projectileVelocity, float heatProduction, float heatUntilCooldown, float heatRegeneration, float reloadTime )
            : base( damage, range, projectileVelocity, reloadTime ) {
            this.name = "Laser";
            this.HeatProduction = heatProduction;
            this.HeatUntilCooldown = heatUntilCooldown;
            this.CurrentHeat = 0;
            this.HeatRegeneration = heatRegeneration;
        }


        public void set( float damage, float range, float projectileVelocity, float heatProduction, float heatUntilCooldown, float heatRegeneration, float reloadTime ) {
            base.set( damage, range, projectileVelocity, reloadTime );

            this.HeatProduction = heatProduction;
            this.HeatUntilCooldown = heatUntilCooldown;
            this.CurrentHeat = 0;
            this.HeatRegeneration = heatRegeneration;
        }

        public override void setValues( float[] values, ref int index ) {
            base.setValues( values, ref index );

            this.HeatProduction = values[index++];
            this.HeatUntilCooldown = values[index++];
            this.CurrentHeat = 0;
            this.HeatRegeneration = values[index++];
        }

        public override float[] getValues() {
            float[] values1 = base.getValues();
            float[] values2 = new float[getNumberOfValues()];
            Array.Copy( values1, 0, values2, 0, base.getNumberOfValues() );

            values2[base.getNumberOfValues()] = this.HeatProduction;
            values2[base.getNumberOfValues() + 1] = this.HeatUntilCooldown;
            values2[base.getNumberOfValues() + 2] = this.HeatRegeneration;

            return values2;
        }

        public override int getNumberOfValues() {
            return base.getNumberOfValues() + 3;
        }

        public void CoolDown( GameTime gameTime ) {
            this.CurrentHeat -= this.HeatRegeneration;
            if ( this.CurrentHeat < 0 ) {
                this.CurrentHeat = 0;
            }
        }

        public override bool Fire() {
            bool success = false;
            if ( this.CurrentHeat < this.HeatUntilCooldown && base.Fire() ) {
                this.CurrentHeat += this.HeatProduction;
                success = true;
            }
            return success;
        }

        public override string ToString() {
            String output = base.ToString();
            output += this.name + ":HeatProduction = " + this.HeatProduction + "\n";
            output += this.name + ":HeatUntilCooldown = " + this.HeatUntilCooldown + "\n";
            output += this.name + ":CurrentHeat = " + this.CurrentHeat + "\n";
            output += this.name + ":HeatRegeneration = " + this.HeatRegeneration + "\n";

            return output;
        }

    }
}

