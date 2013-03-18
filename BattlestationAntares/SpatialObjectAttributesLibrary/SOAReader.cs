using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
 
using TRead = SpatialObjectAttributesLibrary.SpatialObjectAttributes;
 
namespace SpatialObjectAttributesLibrary
{
    public class SOAReader : ContentTypeReader<TRead>
    {
        protected override TRead Read(ContentReader input, TRead existingInstance)
        {
            SpatialObjectAttributes soa = new SpatialObjectAttributes();

            List<AttributeItem> allItems = soa.getItems();

            foreach (AttributeItem item in allItems)
            {
                int index = 0;
                item.setValues(readMultiple(input, item.getNumberOfValues()), ref index);
            }

            return soa;
        }

        private float[] readMultiple(ContentReader input, int amount)
        {
            float[] values = new float[amount];

            for (int index = 0; index < amount; index++)
            {
                values[index] = (float)input.ReadDouble();
            }

            return values;
        }

    }
}