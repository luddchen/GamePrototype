using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class Health
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

    }

}
