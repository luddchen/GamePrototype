using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD {

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


        /// <summary>
        /// constructs a HUD Texture
        /// </summary>
        /// <param name="content"></param>
        public HUDTexture() {
            this.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            this.AbstractSize = new Vector2( 10, 10 );
        }

        public HUDTexture( Texture2D texture, Vector2? position, Vector2? size, Color? color, float? scale, float? rotation) {
            if ( texture == null ) {
                this.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            }
            if ( texture != null ) {
                this.Texture = texture;
            }

            this.AbstractPosition = position ?? Vector2.Zero;
            this.AbstractSize = size ?? new Vector2( 10, 10 );
            this.color = color ?? Color.White;
            this.scale = scale ?? 1.0f;
            this.rotation = rotation ?? 0.0f;
        }

        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public override void Draw( SpriteBatch spriteBatch ) {
            if ( isVisible ) {
                spriteBatch.Draw( this.Texture, this.dest, null, this.color, -this.rotation, this.origin, this.effect, this.layerDepth );
            }
        }
    }
}
