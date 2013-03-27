using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HUD.HUD {

    /// <summary>
    /// a Head Up Display Texture
    /// </summary>
    public class HUDTexture : HUD_Item {

        private Texture2D texture;

        public Texture2D Texture {
            get {
                return this.texture;
            }
            set {
                this.texture = value;
                this.origin = new Vector2( this.texture.Width / 2, this.texture.Height / 2 );
            }
        }

        private Vector2 origin;


        public HUDTexture() : this(null, null, null, null, null, null) { }


        public HUDTexture(Object texture) : this( texture, null, null, null, null, null ) { }


        public HUDTexture( Object texture, Vector2? position, Vector2? size ) : this(texture, position, size, null, null, null) { }


        public HUDTexture( Object texture, Vector2? position, Vector2? size, Color? color, float? scale, float? rotation) {
            if ( texture == null ) {
                this.Texture = HUDService.DefaultTexture;
            } else {
                if ( texture is Texture2D ) {
                    this.Texture = (Texture2D)texture;
                } else if ( texture is String ) {
                    this.Texture = HUDService.Content.Load<Texture2D>( (String)texture );
                } else {
                    this.Texture = HUDService.DefaultTexture;
                }
            }

            this.AbstractPosition = position ?? Vector2.Zero;
            this.AbstractSize = size ?? new Vector2();
            this.color = color ?? Color.White;
            this.AbstractScale = scale ?? 1.0f;
            this.AbstractRotation = rotation ?? 0.0f;
        }

        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public override void Draw( SpriteBatch spriteBatch ) {
            if ( this.isVisible ) {
                spriteBatch.Draw( this.Texture, this.dest, null, this.color, -this.Rotation, this.origin, this.effect, this.LayerDepth );
            }
        }
    }
}
