using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.View.HUD
{

    public class HUD2DButton : HUD2DString
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

        public HUD2DButton(String text, Vector2 position, float scale, Game1 game) : base(text, game)
        {
            this.abstractPosition = position;
            this.overallScale = scale;
            this.scale = scale;

            this.color = HUD2DButton.foregroundColorNormal;
            this.BackgroundColor = HUD2DButton.backgroundColorNormal;
            this.BackgroundTexture = this.game.Content.Load<Texture2D>("Sprites\\SquareRound");
            this.BackgroundTextureOrigin = new Vector2(BackgroundTexture.Width / 2, BackgroundTexture.Height / 2);
        }

        public bool isUpdatedClicked(InputProvider input)
        {
            bool clicked = false;

            if (this.Intersects(input.getMousePos()))
            {
                if (input.isMouseButtonPressed())
                {
                    this.color = HUD2DButton.foregroundColorPressed;
                    this.BackgroundColor = HUD2DButton.backgroundColorPressed;
                    this.scale = HUD2DButton.scalePressed * this.overallScale;
                    clicked = true;
                }
                else
                {
                    this.color = HUD2DButton.foregroundColorHover;
                    this.BackgroundColor = HUD2DButton.backgroundColorHover;
                    this.scale = HUD2DButton.scaleHover * this.overallScale;
                }
            }
            else
            {
                this.color = HUD2DButton.foregroundColorNormal;
                this.BackgroundColor = HUD2DButton.backgroundColorNormal;
                this.scale = HUD2DButton.scaleNormal * this.overallScale;
            }

            return clicked;
        }

    }

}
