using System;
using System.Collections.Generic;

namespace SpatialObjectAttributesLibrary
{

    public abstract class AttributeItem
    {
        public String name;

        public abstract void setValues(float[] values, ref int index);

        public abstract float[] getValues();

        public abstract int getNumberOfValues();

    }

}
