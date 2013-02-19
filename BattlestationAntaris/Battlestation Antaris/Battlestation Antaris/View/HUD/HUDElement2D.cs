using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// abstract basis class for 2D HUD elements
    /// </summary>
    public abstract class HUDElement2D
    {


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spritBatch">the spritebatch</param>
        abstract public void Draw(SpriteBatch spritBatch);

    }

}
