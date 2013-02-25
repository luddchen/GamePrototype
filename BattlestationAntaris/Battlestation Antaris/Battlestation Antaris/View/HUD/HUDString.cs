using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// a Head Up Display String
    /// </summary>
    public class HUDString : HUDElement2D
    {

        /// <summary>
        /// the Font
        /// </summary>
        protected SpriteFont font;


        /// <summary>
        /// the width and height of the displayed string
        /// </summary>
        protected Vector2 measureString;


        /// <summary>
        /// the multiply color of the background image, if existent
        /// </summary>
        protected Color BackgroundColor;


        /// <summary>
        /// the background image texture
        /// </summary>
        protected Texture2D BackgroundTexture;


        /// <summary>
        /// the origin of the background image texture, if existent
        /// </summary>
        protected Vector2 BackgroundTextureOrigin;


        /// <summary>
        /// the displayed string
        /// </summary>
        public String String { get; set; }


        /// <summary>
        /// color of this element
        /// </summary>
        public Color Color { get; set; }


        /// <summary>
        /// scale of this element
        /// </summary>
        public float Scale { get; set; }


        /// <summary>
        /// get width of this element
        /// </summary>
        public float Width
        {
            get { return this.MeasureString.X * this.Scale; }
            set { }
        }


        /// <summary>
        /// get height of this element
        /// </summary>
        public float Height
        {
            get { return this.MeasureString.Y * this.Scale; }
            set { }
        }


        /// <summary>
        /// rotation of this element
        /// </summary>
        public float Rotation { get; set; }


        /// <summary>
        /// if this element visible or not
        /// </summary>
        public bool isVisible { get; set; }


        /// <summary>
        /// size of unscaled String
        /// </summary>
        protected Vector2 MeasureString
        {
            get
            {
                this.measureString = this.font.MeasureString(this.String);
                return this.measureString;
            }
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        /// <param name="content">game content manager</param>
        public HUDString(ContentManager content)
        {
            this.font = content.Load<SpriteFont>("Fonts\\Linds");
            this.Position = Vector2.Zero;
            this.String = "Antaris";
            this.Color = Color.Beige;
            this.Scale = 1.0f;
            this.isVisible = true;
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        /// <param name="text">text to display</param>
        /// <param name="content">game content manager</param>
        public HUDString(String text, ContentManager content)
        {
            this.font = content.Load<SpriteFont>("Fonts\\Linds");
            this.Position = Vector2.Zero;
            this.String = text;
            this.Color = Color.White;
            this.Scale = 1.0f;
            this.isVisible = true;
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        /// <param name="text">text to display</param>
        /// <param name="font">font</param>
        /// <param name="position">position</param>
        /// <param name="color">color</param>
        /// <param name="backgroundColor">background multiply color, can be null (disables the background)</param>
        /// <param name="scale">scale</param>
        /// <param name="rotation">rotation</param>
        /// <param name="content">game content manager</param>
        public HUDString(String text, SpriteFont font, Vector2? position, Color? color, Color? backgroundColor, float? scale, float? rotation, ContentManager content)
        {
            if (text == null) { this.String = " "; }
            if (text != null) { this.String = text; }

            if (font == null) { this.font = content.Load<SpriteFont>("Fonts\\Linds"); }
            if (font != null) { this.font = font; }

            this.Position = position ?? Vector2.Zero;
            this.Color = color ?? Color.Beige;
            this.Scale = scale ?? 1.0f;
            this.Rotation = rotation ?? 0.0f;
            this.isVisible = true;

            if (backgroundColor != null)
            {
                this.BackgroundColor = (Color)backgroundColor;
                this.BackgroundTexture = content.Load<Texture2D>("Sprites\\SquareRound");
                this.BackgroundTextureOrigin = new Vector2(BackgroundTexture.Width / 2, BackgroundTexture.Height / 2);
            }
        }


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isVisible)
            {
                if (this.BackgroundTexture != null)
                {
                    Rectangle dest = new Rectangle((int)this.Position.X, (int)this.Position.Y, (int)(this.Width * 1.2f), (int)this.Height);
                    spriteBatch.Draw(this.BackgroundTexture, dest, null, this.BackgroundColor, -this.Rotation, this.BackgroundTextureOrigin, SpriteEffects.None, 0.02f);
                }
                spriteBatch.DrawString(this.font, this.String, this.Position, this.Color, -this.Rotation, this.MeasureString / 2, this.Scale, SpriteEffects.None, 0.01f);
            }
        }


        /// <summary>
        /// testing intersection with point
        /// </summary>
        /// <param name="point">the test point</param>
        /// <returns>true if there is an intersetion</returns>
        public bool Intersects(Vector2 point)
        {
            if (Rotation != 0)
            {
                return false;
            }
            if (point.X < Position.X - Width / 2 || point.X > Position.X + Width / 2 ||
                point.Y < Position.Y - Height / 2 || point.Y > Position.Y + Height / 2)
            {
                return false;
            }
            return true;
        }


        public override void Window_ClientSizeChanged(Viewport viewport) {
        }
    }
}