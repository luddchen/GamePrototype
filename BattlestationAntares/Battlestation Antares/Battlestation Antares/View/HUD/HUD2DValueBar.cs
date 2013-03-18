using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD
{

    public class HUD2DValueBar : HUD2DContainer
    {
        public delegate float ValueProvider();

        public ValueProvider GetValue = delegate() { return 0; };

        private HUD2DTexture background;

        private HUD2DTexture foreground;

        private Vector3 zeroColor = new Vector3(0, 255, 0);

        private Vector3 oneColor = new Vector3(255, 32, 0);

        private float maxHeight;

        private bool flip;

        public HUD2DValueBar(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, bool flip, Antares game)
            : base(abstractPosition, positionType, game)
        {
            this.flip = flip;
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


        public override sealed void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch)
        {
            float value = this.GetValue();
            value = MathHelper.Clamp(value, 0.0f, 1.0f);

            this.foreground.abstractSize.Y = this.maxHeight * value;

            if (this.flip)
            {
                this.foreground.abstractPosition.Y = (this.foreground.abstractSize.Y - this.maxHeight) / 2.0f;
            }
            else
            {
                this.foreground.abstractPosition.Y = (this.maxHeight * (1.0f - value)) / 2.0f;
            }

            Vector3 col = (float)(1.0f - value) * this.zeroColor + (float)(value) * this.oneColor;

            this.foreground.color.R = (byte)col.X;
            this.foreground.color.G = (byte)col.Y;
            this.foreground.color.B = (byte)col.Z;

            this.foreground.ClientSizeChanged();

            base.Draw(spritBatch);
        }


        public void SetMinColor(Color minCol)
        {
            this.zeroColor = new Vector3(minCol.R, minCol.G, minCol.B);
        }

        public void SetMaxColor(Color maxCol)
        {
            this.oneColor = new Vector3(maxCol.R, maxCol.G, maxCol.B);
        }

    }

}