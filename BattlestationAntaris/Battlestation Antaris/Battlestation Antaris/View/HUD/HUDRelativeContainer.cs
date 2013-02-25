using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD
{

    class HUDRelativeContainer : HUDElement2D
    {

        public float relativeX;

        public float relativeY;

        // list of mappings <element, relative position>
        private List<Tuple<HUDElement2D, Vector2>> allChilds;



        public HUDRelativeContainer(float relativeX, float relativeY, Viewport viewport)
        {
            this.relativeX = relativeX;
            this.relativeY = relativeY;
            this.Position = new Vector2(viewport.Width * this.relativeX, viewport.Height * this.relativeY);

            this.allChilds = new List<Tuple<HUDElement2D, Vector2>>();
        }


        public void Add(HUDElement2D element)
        {
            this.allChilds.Add(Tuple.Create(element, element.Position));
            element.Position += this.Position;
        }


        public override void Draw(SpriteBatch spritBatch)
        {
            foreach (Tuple<HUDElement2D, Vector2> tupel in this.allChilds)
            {
                tupel.Item1.Draw(spritBatch);
            }
        }


        public override void Window_ClientSizeChanged(Viewport viewport)
        {
            this.Position = new Vector2(viewport.Width * this.relativeX, viewport.Height * this.relativeY);

            foreach (Tuple<HUDElement2D, Vector2> tupel in this.allChilds)
            {
                tupel.Item1.Position = tupel.Item2 + this.Position;
            }
        }


    }

}
