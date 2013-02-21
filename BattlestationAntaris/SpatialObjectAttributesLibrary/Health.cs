using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class Health : AttributeItem
    {

        public float MaxHealthPoints;

        public float CurrentHealthPoints;

        public float RegenerationRate;

        public float RepairCost;


        public Health()
        {
            this.MaxHealthPoints = 0;

            this.CurrentHealthPoints = 0;

            this.RegenerationRate = 0;

            this.RepairCost = 0;
        }

        public Health(float maxHealthPoints, float regenarationRate, float repairCost)
        {
            this.MaxHealthPoints = maxHealthPoints;

            this.CurrentHealthPoints = maxHealthPoints;

            this.RegenerationRate = regenarationRate;

            this.RepairCost = repairCost;
        }

        public void set(float maxHealthPoints, float regenarationRate, float repairCost)
        {
            this.MaxHealthPoints = maxHealthPoints;

            this.CurrentHealthPoints = maxHealthPoints;

            this.RegenerationRate = regenarationRate;

            this.RepairCost = repairCost;
        }

        public override void setValues(float[] values, ref int index)
        {
            this.MaxHealthPoints = values[index++];

            this.CurrentHealthPoints = this.MaxHealthPoints;

            this.RegenerationRate = values[index++];

            this.RepairCost = values[index++];
        }

        public override float[] getValues()
        {
            float[] values = new float[3];
            values[0] = this.MaxHealthPoints;
            values[1] = this.RegenerationRate;
            values[2] = this.RepairCost;

            return values;
        }

        public override int getNumberOfValues() 
        {
            return 3;
        }

    }

}
