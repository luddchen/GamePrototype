using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Control;

namespace Battlestation_Antares.View.HUD {
    public enum LayoutDirection {
        HORIZONTAL,
        VERTICAL
    }


    public class HUDArray : HUDContainer {
        public static Color BACKGROUND_COLOR = new Color( 24, 32, 32, 128 );

        public static Color BACKGROUND_COLOR_HOVER = new Color( 28, 36, 36, 192 );

        public static float ABSOLUT_BORDER = 2;

        public static float RELATIV_BORDER = 0.001f;


        public Vector2 borderSize;

        public LayoutDirection direction;

        private HUDTexture background;


        public HUDArray( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType)
            : base( abstractPosition, positionType) {
            this.AbstractSize = abstractSize;
            this.sizeType = sizeType;

            this.direction = LayoutDirection.VERTICAL;

            switch ( this.sizeType ) {

                case HUDType.ABSOLUT:
                    this.borderSize = new Vector2( ABSOLUT_BORDER, ABSOLUT_BORDER );
                    break;

                case HUDType.RELATIV:
                    this.borderSize = new Vector2( RELATIV_BORDER, RELATIV_BORDER );
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.borderSize = new Vector2( ABSOLUT_BORDER, RELATIV_BORDER );
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.borderSize = new Vector2( RELATIV_BORDER, ABSOLUT_BORDER );
                    break;
            }
        }


        public override void Add( HUD_Item element ) {
            base.Add( element );

            element.sizeType = this.sizeType;
            element.positionType = this.sizeType;

            Arrange();

        }


        public override void Remove( HUD_Item element ) {
            base.Remove( element );
            Arrange();
        }


        private void Arrange() {
            Vector2 itemSize = this.AbstractSize;
            Vector2 itemPosition = new Vector2();

            if ( this.direction == LayoutDirection.VERTICAL ) {
                itemSize.Y = itemSize.Y / this.allChilds.Count;
                itemPosition.Y = -( this.AbstractSize.Y / 2 ) + ( itemSize.Y / 2 );
            } else {
                itemSize.X = itemSize.X / this.allChilds.Count;
                itemPosition.X = -( this.AbstractSize.X / 2 ) + ( itemSize.X / 2 );
            }

            foreach ( HUD_Item item in this.allChilds ) {
                item.AbstractSize = itemSize - this.borderSize;

                item.AbstractPosition = itemPosition;

                if ( this.direction == LayoutDirection.VERTICAL ) {
                    itemPosition.Y += itemSize.Y;
                } else {
                    itemPosition.X += itemSize.X;
                }

                item.ClientSizeChanged();
            }
        }


        public override void ClientSizeChanged() {

            base.ClientSizeChanged();

            if ( this.background != null ) {
                this.background.ClientSizeChanged();
            }
        }


        public void CreateBackground( bool create ) {
            if ( create ) {
                this.background = new HUDTexture();
                this.background.sizeType = this.sizeType;
                this.background.AbstractSize = this.AbstractSize;
                this.background.color = HUDArray.BACKGROUND_COLOR;
                this.background.LayerDepth = this.layerDepth;
                this.background.parent = this;

                this.background.ClientSizeChanged();
            } else {
                background = null;
            }
        }


        public override void Draw( SpriteBatch spritBatch ) {
            if ( this.isVisible && this.background != null ) {
                this.background.Draw( spritBatch );
            }

            base.Draw( spritBatch );
        }

    }

}
