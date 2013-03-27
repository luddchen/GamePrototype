using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HUD.HUD {

    /// <summary>
    /// a Head Up Display String
    /// </summary>
    public class HUDString : HUD_Item {

        /// <summary>
        /// the Font
        /// </summary>
        protected SpriteFont font;


        private String text;

        /// <summary>
        /// the displayed text
        /// </summary>
        public String Text {
            get {
                return this.text;
            }
            set {
                this.text = value;
                this.measureString = this.font.MeasureString( value );
            }
        }


        protected float fontSize = 1.0f;

        /// <summary>
        /// the width and height of the unsized string (depends on font)
        /// </summary>
        protected Vector2 measureString;


        /// <summary>
        /// size of scaled string
        /// </summary>
        public override Vector2 Size {
            get {
                return this.measureString * this.Scale * this.fontSize;
            }
        }


        public HUDString() : this(null, null, null, null, null, null, null) { }


        public HUDString(String text, float? scale) : this(text, null, null, null, scale, null, null) {}


        public HUDString( String text, Color? color, float? scale ) : this( text, null, null, color, scale, null, null ) { }


        public HUDString( String text, Vector2? position, Vector2? size ) : this(text, position, size, null, null, null, null) { }


        public HUDString( String text) : this(text, null, null, null, null, null, null) { }


        public HUDString( String text, Vector2? position, Vector2? size, Color? color, float? scale, float? rotation, SpriteFont font) {
            if ( font == null ) {
                this.font = HUD_Item.game.DefaultFont;
            }
            if ( font != null ) {
                this.font = font;
            }

            if ( text == null ) {
                this.Text = "HUD String";
            }
            if ( text != null ) {
                this.Text = text;
            }

            this.AbstractPosition = position ?? Vector2.Zero;
            this.AbstractSize = size ?? this.measureString;
            this.color = color ?? Color.Beige;
            this.AbstractScale = scale ?? 1.0f;
            this.AbstractRotation = rotation ?? 0.0f;
        }


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public override void Draw( SpriteBatch spriteBatch ) {
            if ( isVisible ) {
                spriteBatch.DrawString( this.font, this.Text, this.Position,
                                        this.color, -this.Rotation, this.measureString / 2,
                                        this.Scale * this.fontSize, this.effect, this.LayerDepth );
            }
        }


        /// <summary>
        /// testing intersection with point
        /// </summary>
        /// <param name="point">the test point</param>
        /// <returns>true if there is an intersetion</returns>
        public override bool Intersects( Vector2 point ) {
            return ( Math.Abs( Position.X - point.X ) < Size.X / 2 && Math.Abs( Position.Y - point.Y ) < Size.Y / 2 );
        }


        public override void RenderSizeChanged() {
            base.RenderSizeChanged();
            this.fontSize = HUD_Item.AbstractToConcrete(this.AbstractSize).Y / this.measureString.Y;
        }

    }
}