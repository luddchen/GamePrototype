using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.CockpitHUD
{

    class LaserHeat : HUD2DContainer
    {

        private HUD2DTexture background;

        private HUD2DTexture foreground;

        private float maxHeight;

        public LaserHeat(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.abstractSize = abstractSize;
            this.sizeType = sizeType;

            this.background = new HUD2DTexture(game);
            this.background.positionType = this.sizeType;
            this.background.abstractSize = abstractSize;
            this.background.sizeType = sizeType;
            this.background.color = Color.White;
            this.background.layerDepth = this.layerDepth;
            this.background.Texture = game.Content.Load<Texture2D>("Sprites//SquareBorder");
            Add(this.background);

            this.foreground = new HUD2DTexture(game);
            this.foreground.positionType = this.sizeType;
            this.foreground.abstractSize = abstractSize;
            this.foreground.sizeType = sizeType;
            this.foreground.color = Color.Green;
            Add(this.foreground);

            this.maxHeight = this.foreground.abstractSize.Y;
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch)
        {
            float value = this.game.world.spaceShip.attributes.Laser.CurrentHeat / this.game.world.spaceShip.attributes.Laser.HeatUntilCooldown;
            value = MathHelper.Clamp(value, 0.0f, 1.0f);

            this.foreground.abstractSize.Y = this.maxHeight * value;

            this.foreground.abstractPosition.Y = (this.maxHeight * (1.0f - value)) / 2.0f;

            this.foreground.color.R = (byte)(Math.Pow(value, 2) * 250.0f);
            this.foreground.color.G = (byte)(Math.Sqrt(1.0f - value) * 250.0f);

            this.foreground.ClientSizeChanged();

            base.Draw(spritBatch);
        }


    }

}
