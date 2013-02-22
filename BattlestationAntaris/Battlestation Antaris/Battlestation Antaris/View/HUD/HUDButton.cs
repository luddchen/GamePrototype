using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.View.HUD
{

    public class HUDButton : HUDString
    {

        private static Color backgroundColorNormal = new Color(50, 50, 100, 128);

        private static Color backgroundColorHover = new Color(50, 50, 100, 128);

        private static Color backgroundColorPressed = new Color(50, 50, 100, 128);

        private static Color foregroundColorNormal = Color.White;

        private static Color foregroundColorHover = new Color(255, 255, 128);

        private static Color foregroundColorPressed = new Color(128, 255, 128);

        private static float scaleNormal = 1.0f;

        private static float scaleHover = 1.04f;

        private static float scalePressed = 0.96f;

        private float overallScale;

        public HUDButton(String text, Vector2 position, float scale, ContentManager content) : base(text, content)
        {
            this.Position = position;
            this.overallScale = scale;
            this.Scale = scale;

            this.Color = HUDButton.foregroundColorNormal;
            this.BackgroundColor = HUDButton.backgroundColorNormal;
            this.BackgroundTexture = content.Load<Texture2D>("Sprites\\SquareRound");
            this.BackgroundTextureOrigin = new Vector2(BackgroundTexture.Width / 2, BackgroundTexture.Height / 2);
        }

        public bool isUpdatedClicked(InputProvider input)
        {
            bool clicked = false;

            if (this.Intersects(input.getMousePos()))
            {
                if (input.isMouseButtonPressed())
                {
                    this.Color = HUDButton.foregroundColorPressed;
                    this.BackgroundColor = HUDButton.backgroundColorPressed;
                    this.Scale = HUDButton.scalePressed * this.overallScale;
                    clicked = true;
                }
                else
                {
                    this.Color = HUDButton.foregroundColorHover;
                    this.BackgroundColor = HUDButton.backgroundColorHover;
                    this.Scale = HUDButton.scaleHover * this.overallScale;
                }
            }
            else
            {
                this.Color = HUDButton.foregroundColorNormal;
                this.BackgroundColor = HUDButton.backgroundColorNormal;
                this.Scale = HUDButton.scaleNormal * this.overallScale;
            }

            return clicked;
        }

    }

}
