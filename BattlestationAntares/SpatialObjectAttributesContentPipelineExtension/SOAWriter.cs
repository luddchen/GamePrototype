using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

using SpatialObjectAttributesLibrary;
 
using TWrite = SpatialObjectAttributesLibrary.SpatialObjectAttributes;
 
namespace SpatialObjectAttributesContentPipelineExtension
{
    [ContentTypeWriter]
    public class SOAWriter : ContentTypeWriter<TWrite>
    {
        protected override void Write(ContentWriter output, TWrite value)
        {
            List<AttributeItem> allItems = value.getItems();

            foreach (AttributeItem item in allItems)
            {
                float[] allValues = item.getValues();

                foreach (float f in allValues)
                {
                    output.Write((double)f);
                }
            }
        }
 
        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "SpatialObjectAttributesLibrary.SOAReader, SpatialObjectAttributesLibrary";
        }
    }
}