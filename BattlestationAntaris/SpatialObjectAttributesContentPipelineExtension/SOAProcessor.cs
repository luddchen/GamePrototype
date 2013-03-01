using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

using SpatialObjectAttributesLibrary;
 
using TInput = System.String;
using TOutput = SpatialObjectAttributesLibrary.SpatialObjectAttributes;
 
namespace SpatialObjectAttributesContentPipelineExtension
{
    [ContentProcessor(DisplayName = "Spatial Object Attributes Processor")]
    public class SOAProcessor : ContentProcessor<TInput, TOutput>
    {
        public override TOutput Process(TInput input, ContentProcessorContext context)
        {

            SpatialObjectAttributes soa = new SpatialObjectAttributes();

            string[] valueStrings = input.Split(new char[] { '\n', ' ' });

            float[] values = new float[valueStrings.Length];

            double testNum = 0.5;
            char decimalSepparator;
            decimalSepparator = testNum.ToString()[1];

            int index = 0;

            for (int i = 0; i < valueStrings.Length; i++)
            {
                if (valueStrings[i].Contains('#'))    // comments starts with "#" 
                {
                    continue;
                } 
                else 
                {
                    values[index++] = (float)double.Parse(valueStrings[i].Replace('.', decimalSepparator).Replace(',', decimalSepparator));
                }
            }

            index = 0;

            List<AttributeItem> allItems = soa.getItems();

            foreach (AttributeItem item in allItems)
            {
                item.setValues(values, ref index);
            }
 
            return soa;
        }
    }
}