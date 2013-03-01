using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD
{

    public class HUD2DContainer : HUD2D
    {

        // list of all childs
        private List<HUD2D> allChilds;



        public HUD2DContainer(Vector2 abstractPosition, HUDType positionType, Game1 game) 
            : base(game)
        {
            this.abstractPosition = abstractPosition;

            this.positionType = positionType;

            this.allChilds = new List<HUD2D>();
        }


        public void Add(HUD2D element)
        {
            this.allChilds.Add(element);
            element.ClientSizeChanged(this.position);
        }


        public override void Draw(SpriteBatch spritBatch)
        {
            if (this.isVisible)
            {
                foreach (HUD2D item in this.allChilds)
                {
                    item.Draw(spritBatch);
                }
            }
        }


        public override void ClientSizeChanged(Vector2 offset)
        {
            base.ClientSizeChanged(offset);

            foreach (HUD2D item in this.allChilds)
            {
                item.ClientSizeChanged(this.position);
            }
        }


    }

}
