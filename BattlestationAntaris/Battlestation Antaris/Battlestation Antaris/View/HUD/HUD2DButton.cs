using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.View.HUD
{

    public class HUD2DButton : HUD2DString
    {

        //public Color backgroundColorNormal = new Color(32, 48, 48, 160);

        //public Color backgroundColorHover = new Color(40, 64, 64, 192);

        //public Color backgroundColorPressed = new Color(32, 48, 48, 255);

        //public Color foregroundColorNormal = Color.White;

        //public Color foregroundColorHover = new Color(255, 255, 128);

        //public Color foregroundColorPressed = new Color(128, 255, 128);

        //private static float scaleNormal = 1.0f;

        //private static float scaleHover = 0.99f;

        //private static float scalePressed = 0.96f;

        public ButtonStyle style;

        private float overallScale;

        private Action action;


        public HUD2DButton(String text, Vector2 position, float scale, Game1 game) : base(text, game)
        {
            this.abstractPosition = position;
            this.overallScale = scale;
            this.scale = scale;
            this.style = ButtonStyle.DefaultButtonStyle();

            this.color = this.style.foregroundColorNormal;
            this.BackgroundColor = this.style.backgroundColorNormal;
            this.BackgroundTexture = this.game.Content.Load<Texture2D>("Sprites\\Button2");
            this.BackgroundTextureOrigin = new Vector2(BackgroundTexture.Width / 2, BackgroundTexture.Height / 2);
        }

        public bool isUpdatedClicked(InputProvider input)
        {
            bool clicked = false;

            if (this.Intersects(input.getMousePos()))
            {
                if (input.isLeftMouseButtonPressed())
                {
                    this.color = this.style.foregroundColorPressed;
                    this.BackgroundColor = this.style.backgroundColorPressed;
                    this.scale = this.style.scalePressed * this.overallScale;
                    clicked = true;
                    if (this.action != null)
                    {
                        this.action();
                    }
                }
                else
                {
                    this.color = this.style.foregroundColorHover;
                    this.BackgroundColor = this.style.backgroundColorHover;
                    this.scale = this.style.scaleHover * this.overallScale;
                }
            }
            else
            {
                this.color = this.style.foregroundColorNormal;
                this.BackgroundColor = this.style.backgroundColorNormal;
                this.scale = this.style.scaleNormal * this.overallScale;
            }

            return clicked;
        }


        public void SetAction(Action action)
        {
            this.action = action;
        }


        public void Toggle()
        {
            Color temp = this.style.foregroundColorHover;
            this.style.foregroundColorHover = this.style.foregroundColorNormal;
            this.style.foregroundColorNormal = temp;
        }

    }

}
