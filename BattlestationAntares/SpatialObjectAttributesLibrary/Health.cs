using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary {

    /// <summary>
    /// Spatial Object Attributes : Health
    /// </summary>
    public class Health : AttributeItem {

        public float MaxHealthPoints;

        public float CurrentHealthPoints;

        public float RegenerationRate;

        public float RepairCost;


        public Health() {
            this.name = "Health";
            this.MaxHealthPoints = 0;
            this.CurrentHealthPoints = 0;
            this.RegenerationRate = 0;
            this.RepairCost = 0;
        }

        public Health( Health health ) {
            this.name = "Health";
            this.MaxHealthPoints = health.MaxHealthPoints;
            this.CurrentHealthPoints = health.CurrentHealthPoints;
            this.RegenerationRate = health.RegenerationRate;
            this.RepairCost = health.RepairCost;
        }

        public Health( float maxHealthPoints, float regenarationRate, float repairCost ) {
            this.name = "Health";
            this.MaxHealthPoints = maxHealthPoints;
            this.CurrentHealthPoints = maxHealthPoints;
            this.RegenerationRate = regenarationRate;
            this.RepairCost = repairCost;
        }

        public void set( float maxHealthPoints, float regenarationRate, float repairCost ) {
            this.MaxHealthPoints = maxHealthPoints;
            this.CurrentHealthPoints = maxHealthPoints;
            this.RegenerationRate = regenarationRate;
            this.RepairCost = repairCost;
        }

        public override void setValues( float[] values, ref int index ) {
            this.MaxHealthPoints = values[index++];
            this.CurrentHealthPoints = this.MaxHealthPoints;
            this.RegenerationRate = values[index++];
            this.RepairCost = values[index++];
        }

        public override float[] getValues() {
            float[] values = new float[3];
            values[0] = this.MaxHealthPoints;
            values[1] = this.RegenerationRate;
            values[2] = this.RepairCost;

            return values;
        }

        public override int getNumberOfValues() {
            return 3;
        }


        // usefull methods

        /// <summary>
        /// trigger regeneration
        /// </summary>
        public void Regenerate() {
            if ( this.CurrentHealthPoints < this.MaxHealthPoints ) {
                this.CurrentHealthPoints += this.RegenerationRate;
            }
        }

        /// <summary>
        /// repair to max health
        /// </summary>
        public void Repair() {
            this.CurrentHealthPoints = this.MaxHealthPoints;
        }

        /// <summary>
        /// apply damage
        /// </summary>
        /// <param name="damage">the recieved damage</param>
        /// <returns>true : if out of health</returns>
        public bool ApplyDamage( float damage ) {
            this.CurrentHealthPoints -= damage;
            if ( this.CurrentHealthPoints < 0 ) {
                this.CurrentHealthPoints = 0;
            }
            return ( this.CurrentHealthPoints == 0 );
        }

        public override string ToString() {
            String output = "";
            output += this.name + ":MaxHealthPoints = " + this.MaxHealthPoints + "\n";
            output += this.name + ":CurrentHealthPoints = " + this.CurrentHealthPoints + "\n";
            output += this.name + ":RegenerationRate = " + this.RegenerationRate + "\n";
            output += this.name + ":RepairCost = " + this.RepairCost + "\n";

            return output;
        }

    }

}
