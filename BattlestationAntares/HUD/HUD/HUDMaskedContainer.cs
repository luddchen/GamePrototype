using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HUD.HUD {
    public class HUDMaskedContainer : HUDContainer {

        private HUDTexture mask;

        public override HUDType SizeType {
            set {
                this.mask.SizeType = value;
                base.SizeType = value;
            }
        }

        public override Vector2 AbstractSize {
            set {
                this.mask.AbstractSize = value;
                base.AbstractSize = value;
            }
        }

        public override float LayerDepth {
            set {
                this.mask.LayerDepth = value - 0.05f;// maybe do here calculation of min layerDepth
                base.LayerDepth = value;
            }
        }

        public HUDMaskedContainer( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType )
            : base( abstractPosition ) {
            this.mask = new HUDTexture();
            this.mask.parent = this;
            this.mask.LayerDepth = base.LayerDepth - 0.05f;

            this.SizeType = sizeType;
            this.AbstractSize = abstractSize;
        }


        public void SetMask( Object texture, Color color ) {
            if ( texture != null && color != null ) {
                SetMask( texture );
                SetMask( color );
            } else if ( texture != null ) {
                SetMask( texture );
            } else if ( color != null ) {
                SetMask( color );
            } else {
                SetMask( null );
            }
        }


        public void SetMask( Object property ) {
            bool visibility = true;

            if ( property == null ) {
                visibility = false;
            } else {
                if ( property is Texture2D ) {
                    this.mask.Texture = (Texture2D)property;
                } else if ( property is String ) {
                    this.mask.Texture = HUD_Item.game.Content.Load<Texture2D>( (String)property );
                } else if ( property is Color ) {
                    this.mask.color = (Color)property;
                } else {
                    visibility = false;
                }
            }

            this.mask.IsVisible = visibility;
        }


        public override void Draw( SpriteBatch spriteBatch ) {
            if ( this.isVisible ) {
                this.mask.Draw( spriteBatch );
            }
            base.Draw( spriteBatch );
        }


        public override void RenderSizeChanged() {
            base.RenderSizeChanged();
            if ( this.mask != null ) {
                this.mask.RenderSizeChanged();
            }
        }
    }
}
