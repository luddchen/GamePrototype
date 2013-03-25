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

        public void SetMask( Texture2D texture ) {
            if ( texture == null ) {
                this.mask.IsVisible = false;
            } else {
                this.mask.IsVisible = true;
                this.mask.Texture = texture;
            }
        }

        public void SetMask( String textureName ) {
            SetMask( ( textureName == null ) ? null : HUD_Item.game.Content.Load<Texture2D>( textureName ) );
        }

        public void SetMaskColor( Color color ) {
            this.mask.IsVisible = true;
            this.mask.color = color;
        }

        public override void Draw( SpriteBatch spriteBatch ) {
            if ( this.isVisible ) {
                this.mask.Draw( spriteBatch );
            }
            base.Draw( spriteBatch );
        }

        public override void ClientSizeChanged() {
            base.ClientSizeChanged();
            if ( this.mask != null ) {
                this.mask.ClientSizeChanged();
            }
        }
    }
}
