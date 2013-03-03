using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD
{

    public class HUD2DButtonContainer : HUD2DContainer
    {

        public static float ABSOLUT_BORDER = 10;

        public static float RELATIV_BORDER = 0.005f;

        public Vector2 borderSize;

        public Vector2 abstractSize;

        public HUDType sizeType;


        public HUD2DButtonContainer(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.abstractSize = abstractSize;
            this.sizeType = sizeType;

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

            if (element is HUD2DButton)
            {
                ((HUD2DButton)element).sizeType = this.sizeType;
                ((HUD2DButton)element).positionType = this.sizeType;
                ((HUD2DButton)element).layerDepth = this.layerDepth * 0.9f;
                ArrangeButtons();
            }
            else
            {
                element.layerDepth = this.layerDepth;
            }
        }


        public override void Remove(HUD2D element)
        {
            base.Remove(element);
            ArrangeButtons();
        }


        private void ArrangeButtons()
        {
            int nrOfButtons = 0;

            foreach (HUD2D item in this.allChilds)
            {
                if (item is HUD2DButton)
                {
                    nrOfButtons++;
                }
            }

            float buttonHeight = this.abstractSize.Y / nrOfButtons;
            float buttonY = -(buttonHeight * (nrOfButtons / 2 - 0.5f));

            foreach (HUD2D item in this.allChilds)
            {
                if (item is HUD2DButton)
                {
                    ((HUD2DButton)item).abstractSize.X = this.abstractSize.X - this.borderSize.X;
                    ((HUD2DButton)item).abstractSize.Y = buttonHeight - this.borderSize.Y;

                    ((HUD2DButton)item).abstractPosition.X = 0;
                    ((HUD2DButton)item).abstractPosition.Y = buttonY;

                    buttonY += buttonHeight;
                }
            }
        }

    }

}
