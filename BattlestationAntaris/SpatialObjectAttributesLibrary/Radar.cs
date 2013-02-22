using System;

namespace SpatialObjectAttributesLibrary
{

    public class Radar : AttributeItem
    {

        float Range;

        public Radar()
        {
            this.Range = 0;
        }

        public Radar(float range)
        {
            this.Range = range;
        }


        public void set(float range)
        {
            this.Range = range;
        }

        public void set(Radar radar)
        {
            this.Range = radar.Range;
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

    }

}
