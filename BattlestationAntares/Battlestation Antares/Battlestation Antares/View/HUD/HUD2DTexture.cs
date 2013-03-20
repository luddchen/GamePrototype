using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD {

    /// <summary>
    /// a Head Up Display Texture
    /// </summary>
    public class HUD2DTexture : HUD2D {
        private Texture2D texture;

        /// <summary>
        /// name of this element
        /// </summary>
        public String Name;

        /// <summary>
        /// texture of this element
        /// </summary>
        public Texture2D Texture {
            get {
                return this.texture;
            }
            set {
                this.texture = value;
                this.Origin = new Vector2( this.texture.Width / 2, this.texture.Height / 2 );
            }
        }

        /// <summary>
        /// origin of the elements texture
        /// </summary>
        public Vector2 Origin;


        /// <summary>
        /// constructs a Head Up Display Element Texture
        /// </summary>
        /// <param name="content"></param>
        public HUD2DTexture() {
            this.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            this.abstractSize = new Vector2( 10, 10 );
        }

        public HUD2DTexture( Texture2D texture, Vector2? position, Vector2? size, Color? color, float? scale, float? rotation) {
            if ( texture == null ) {
                this.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            }
            if ( texture != null ) {
                this.Texture = texture;
            }

            this.abstractPosition = position ?? Vector2.Zero;
            this.abstractSize = size ?? new Vector2( 10, 10 );
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
                Rectangle dest = new Rectangle(
                        (int)position.X,
                        (int)position.Y,
                        (int)( size.X * scale ),
                        (int)( size.Y * scale ) );

                spriteBatch.Draw( this.Texture,
                                dest,
                                null,
                                this.color,
                                -this.rotation,
                                this.Origin,
                                this.effect,
                                this.layerDepth );
            }
        }

        /// <summary>
        /// testing intersection with point
        /// </summary>
        /// <param name="point">the test point</param>
        /// <returns>true if there is an intersetion</returns>
        public override bool Intersects( Vector2 point ) {
            if ( point.X < position.X - scale * size.X / 2 || point.X > position.X + scale * size.X / 2 ||
                point.Y < position.Y - scale * size.Y / 2 || point.Y > position.Y + scale * size.Y / 2 ) {
                return false;
            }
            return true;
        }


        public override void ClientSizeChanged() {
            base.ClientSizeChanged();

            switch ( this.sizeType ) {
                case HUDType.ABSOLUT:
                    this.size = this.abstractSize;
                    break;

                case HUDType.RELATIV:
                    this.size.X = this.abstractSize.X * Antares.renderSize.X;
                    this.size.Y = this.abstractSize.Y * Antares.renderSize.Y;
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.size.X = this.abstractSize.X;
                    this.size.Y = this.abstractSize.Y * Antares.renderSize.Y;
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.size.X = this.abstractSize.X * Antares.renderSize.X;
                    this.size.Y = this.abstractSize.Y;
                    break;
            }
        }

    }
}
