using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD
{
    class HUDRelativeTexture : HUDTexture
    {

        public float relativeWidth;

        public float relativeHeight;

        public float relativeX;

        public float relativeY;

        public HUDRelativeTexture(float relativeWidth, float relativeHeight, float relativeX, float relativeY, Viewport viewport,ContentManager content)
            : base(content)
        {
            this.relativeWidth = relativeWidth;
            this.relativeHeight = relativeHeight;
            this.relativeX = relativeX;
            this.relativeY = relativeY;

            Window_ClientSizeChanged(viewport);
        }


        public override void Window_ClientSizeChanged(Viewport viewport)
        {
            this.Width = viewport.Width * this.relativeWidth;
            this.Height = viewport.Height * this.relativeHeight;
            this.Position = new Vector2(viewport.Width * this.relativeX, viewport.Height * this.relativeY);
        }

    }
}
