using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD
{

    /// <summary>
    /// a Head Up Display String
    /// </summary>
    public class HUD2DString : HUD2D
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


        public Vector2 BackgroundSize
        {
            get
            {
                Vector2 backgroundSize = new Vector2();
                if (this.abstractSize == Vector2.Zero)
                {
                    backgroundSize = this.size * 1.2f;
                }
                else
                {
                    switch (this.sizeType) {

                        case HUDType.ABSOLUT :
                            backgroundSize = this.abstractSize;
                            break;

                        case HUDType.RELATIV :
                            backgroundSize.X = this.abstractSize.X * this.game.GraphicsDevice.Viewport.Width;
                            backgroundSize.Y = this.abstractSize.Y * this.game.GraphicsDevice.Viewport.Height;
                            break;

                        case HUDType.ABSOLUT_RELATIV :
                            backgroundSize.X = this.abstractSize.X;
                            backgroundSize.Y = this.abstractSize.Y * this.game.GraphicsDevice.Viewport.Height;
                            break;

                        case HUDType.RELATIV_ABSOLUT :
                            backgroundSize.X = this.abstractSize.X * this.game.GraphicsDevice.Viewport.Width;
                            backgroundSize.Y = this.abstractSize.Y;
                            break;
                    }

                    backgroundSize *= this.scale;
                }
                return backgroundSize;
            }
        }


        /// <summary>
        /// the origin of the background image texture, if existent
        /// </summary>
        protected Vector2 BackgroundTextureOrigin;


        /// <summary>
        /// the displayed string
        /// </summary>
        public String String { get; set; }


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
        /// size of scaled string
        /// </summary>
        public new Vector2 size
        {
            get { return this.MeasureString * this.scale; }
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        /// <param name="content">game content manager</param>
        public HUD2DString(Game1 game) : base(game)
        {
            this.font = this.game.Content.Load<SpriteFont>("Fonts\\Font");
            this.String = "HUD2DString";
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        /// <param name="text">text to display</param>
        /// <param name="content">game content manager</param>
        public HUD2DString(String text, Game1 game) : base(game)
        {
            this.font = this.game.Content.Load<SpriteFont>("Fonts\\Font");
            this.String = text;
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
        public HUD2DString(String text, SpriteFont font, Vector2? position, Color? color, Color? backgroundColor, float? scale, float? rotation, Game1 game)
            : base(game)
        {
            if (text == null) { this.String = " "; }
            if (text != null) { this.String = text; }

            if (font == null) { this.font = this.game.Content.Load<SpriteFont>("Fonts\\Font"); }
            if (font != null) { this.font = font; }

            this.abstractPosition = position ?? Vector2.Zero;
            this.color = color ?? Color.Beige;
            this.scale = scale ?? 1.0f;
            this.rotation = rotation ?? 0.0f;

            if (backgroundColor != null)
            {
                this.BackgroundColor = (Color)backgroundColor;
                this.BackgroundTexture = this.game.Content.Load<Texture2D>("Sprites\\Square");
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
                    Vector2 bgSize = BackgroundSize;
                    Rectangle dest = 
                        new Rectangle(  (int)this.position.X, (int)this.position.Y, 
                                        (int)bgSize.X, (int)bgSize.Y);

                    spriteBatch.Draw(   this.BackgroundTexture, dest, null, 
                                        this.BackgroundColor, -this.rotation, 
                                        this.BackgroundTextureOrigin, this.effect, this.layerDepth);
                }

                spriteBatch.DrawString( this.font, this.String, this.position, 
                                        this.color, -this.rotation,  this.MeasureString / 2, 
                                        this.scale, this.effect, this.layerDepth - 0.01f);
            }
        }


        /// <summary>
        /// testing intersection with point
        /// </summary>
        /// <param name="point">the test point</param>
        /// <returns>true if there is an intersetion</returns>
        public override bool Intersects(Vector2 point)
        {
            if (rotation != 0)
            {
                return false;
            }

            if (this.BackgroundTexture == null)
            {
                if (point.X < position.X - size.X / 2 || point.X > position.X + size.X / 2 ||
                    point.Y < position.Y - size.Y / 2 || point.Y > position.Y + size.Y / 2)
                {
                    return false;
                }
            }
            else
            {
                if (point.X < position.X - BackgroundSize.X / 2 || point.X > position.X + BackgroundSize.X / 2 ||
                    point.Y < position.Y - BackgroundSize.Y / 2 || point.Y > position.Y + BackgroundSize.Y / 2)
                {
                    return false;
                }
            }
            return true;
        }


    }
}