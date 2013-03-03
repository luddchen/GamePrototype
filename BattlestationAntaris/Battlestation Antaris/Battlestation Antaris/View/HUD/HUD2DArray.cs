using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD
{
    public enum LayoutDirection
    {
        HORIZONTAL,
        VERTICAL
    }


    public class HUD2DArray : HUD2DContainer
    {
        public static Color BACKGROUND_COLOR = HUD2DButton.backgroundColorNormal;

        public static float ABSOLUT_BORDER = 10;

        public static float RELATIV_BORDER = 0.005f;


        public Vector2 borderSize;

        public LayoutDirection direction;

        private HUD2DTexture background;


        public HUD2DArray(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.abstractSize = abstractSize;
            this.sizeType = sizeType;

            this.direction = LayoutDirection.VERTICAL;

            switch (this.sizeType)
            {

                case HUDType.ABSOLUT:
                    this.borderSize = new Vector2(ABSOLUT_BORDER, ABSOLUT_BORDER);
                    break;

                case HUDType.RELATIV:
                    this.borderSize = new Vector2(RELATIV_BORDER, RELATIV_BORDER);
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.borderSize = new Vector2(ABSOLUT_BORDER, RELATIV_BORDER);
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.borderSize = new Vector2(RELATIV_BORDER, ABSOLUT_BORDER);
                    break;
            }
        }


        public override void Add(HUD2D element)
        {
            base.Add(element);

            element.sizeType = this.sizeType;
            element.positionType = this.sizeType;
            element.layerDepth = this.layerDepth * 0.9f;

            Arrange();

        }


        public override void Remove(HUD2D element)
        {
            base.Remove(element);
            Arrange();
        }


        private void Arrange()
        {
            Vector2 itemSize = this.abstractSize;
            Vector2 itemPosition = new Vector2();

            if (this.direction == LayoutDirection.VERTICAL)
            {
                itemSize.Y = itemSize.Y / this.allChilds.Count;
                itemPosition.Y = -(this.abstractSize.Y / 2) + (itemSize.Y / 2);
            }
            else
            {
                itemSize.X = itemSize.X / this.allChilds.Count;
                itemPosition.X = -(this.abstractSize.X / 2) + (itemSize.X / 2);
            }

            foreach (HUD2D item in this.allChilds)
            {
                item.abstractSize = itemSize - this.borderSize;

                item.abstractPosition = itemPosition;

                if (this.direction == LayoutDirection.VERTICAL)
                {
                    itemPosition.Y += itemSize.Y;
                }
                else
                {
                    itemPosition.X += itemSize.X;
                }
            }
        }


        public override void ClientSizeChanged(Vector2 offset)
        {
            base.ClientSizeChanged(offset);

            if (this.background != null)
            {
                this.background.ClientSizeChanged(this.position);
            }
        }


        public void CreateBackground(bool create)
        {
            if (create)
            {
                this.background = new HUD2DTexture(game);
                this.background.sizeType = this.sizeType;
                this.background.abstractSize = this.abstractSize;
                this.background.color = HUD2DArray.BACKGROUND_COLOR;

                this.background.ClientSizeChanged(this.position);
            }
            else
            {
                background = null;
            }
        }


        public override void Draw(SpriteBatch spritBatch)
        {
            base.Draw(spritBatch);

            if (this.isVisible && this.background != null)
            {
                this.background.Draw(spritBatch);
            }
        }

    }

}
