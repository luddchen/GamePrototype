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

        public HUDTexture( Object texture = null, Color? color = null, Vector2 position = new Vector2(), Vector2 size = new Vector2(), float scale = 1.0f, float rotation = 0.0f ) {
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

            this.AbstractPosition = position;
            this.AbstractSize = size;
            this.color = color ?? Color.White;
            this.AbstractScale = scale;
            this.AbstractRotation = rotation;
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
