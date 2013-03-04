using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.View.HUD
{

    public class HUD2DButton : HUD2DString
    {

        public Color backgroundColorNormal = new Color(32, 48, 48, 160);

        public Color backgroundColorHover = new Color(40, 64, 64, 192);

        public Color backgroundColorPressed = new Color(32, 48, 48, 255);

        public Color foregroundColorNormal = Color.White;

        public Color foregroundColorHover = new Color(255, 255, 128);

        public Color foregroundColorPressed = new Color(128, 255, 128);

        private static float scaleNormal = 1.0f;

        private static float scaleHover = 0.99f;

        private static float scalePressed = 0.96f;

        private float overallScale;

        public HUD2DButton(String text, Vector2 position, float scale, Game1 game) : base(text, game)
        {
            this.abstractPosition = position;
            this.overallScale = scale;
            this.scale = scale;

            this.color = this.foregroundColorNormal;
            this.BackgroundColor = this.backgroundColorNormal;
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
                    this.color = this.foregroundColorPressed;
                    this.BackgroundColor = this.backgroundColorPressed;
                    this.scale = HUD2DButton.scalePressed * this.overallScale;
                    clicked = true;
                }
                else
                {
                    this.color = this.foregroundColorHover;
                    this.BackgroundColor = this.backgroundColorHover;
                    this.scale = HUD2DButton.scaleHover * this.overallScale;
                }
            }
            else
            {
                this.color = this.foregroundColorNormal;
                this.BackgroundColor = this.backgroundColorNormal;
                this.scale = HUD2DButton.scaleNormal * this.overallScale;
            }

            return clicked;
        }

    }

}
