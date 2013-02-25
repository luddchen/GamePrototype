using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// abstract basis class for 2D HUD elements
    /// </summary>
    public abstract class HUDElement2D
    {

        public Vector2 Position;

        public float layerDepth = 0.5f;

        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spritBatch">the spritebatch</param>
        public abstract void Draw(SpriteBatch spritBatch);


        public abstract void Window_ClientSizeChanged(Viewport viewport);

    }

}
