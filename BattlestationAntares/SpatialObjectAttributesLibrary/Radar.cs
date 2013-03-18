using System;

namespace SpatialObjectAttributesLibrary
{

    /// <summary>
    /// Spatial Object Attributes : Radar
    /// </summary>
    public class Radar : AttributeItem
    {

        public float Range;

        public Radar()
        {
            this.name = "Radar";
            this.Range = 0;
        }

        public Radar(float range)
        {
            this.name = "Radar";
            this.Range = range;
        }

        public Radar(Radar radar)
        {
            this.name = "Radar";
            this.Range = radar.Range;
        }

        public void set(float range)
        {
            this.Range = range;
        }

        public override void setValues(float[] values, ref int index)
        {
            this.Range = values[index++];
        }

        public override float[] getValues()
        {
            float[] values = new float[1];
            values[0] = this.Range;

            return values;
        }

        public override int getNumberOfValues()
        {
            return 1;
        }


        public override string ToString()
        {
            String output = "";
            output += this.name + ":Range = " + this.Range + "\n";

            return output;
        }

    }

}
