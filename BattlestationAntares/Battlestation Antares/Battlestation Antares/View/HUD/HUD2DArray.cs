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


    public class HUD2DArray : HUD2DContainer {
        public static Color BACKGROUND_COLOR = new Color( 24, 32, 32, 128 );

        public static Color BACKGROUND_COLOR_HOVER = new Color( 28, 36, 36, 192 );

        public static float ABSOLUT_BORDER = 10;

        public static float RELATIV_BORDER = 0.005f;


        public Vector2 borderSize;

        public LayoutDirection direction;

        private HUD2DTexture background;


        public HUD2DArray( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType)
            : base( abstractPosition, positionType) {
            this.abstractSize = abstractSize;
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


        public override void Add( HUD2D element ) {
            base.Add( element );

            element.sizeType = this.sizeType;
            element.positionType = this.sizeType;

            Arrange();

        }


        public override void Remove( HUD2D element ) {
            base.Remove( element );
            Arrange();
        }


        private void Arrange() {
            Vector2 itemSize = this.abstractSize;
            Vector2 itemPosition = new Vector2();

            if ( this.direction == LayoutDirection.VERTICAL ) {
                itemSize.Y = itemSize.Y / this.allChilds.Count;
                itemPosition.Y = -( this.abstractSize.Y / 2 ) + ( itemSize.Y / 2 );
            } else {
                itemSize.X = itemSize.X / this.allChilds.Count;
                itemPosition.X = -( this.abstractSize.X / 2 ) + ( itemSize.X / 2 );
            }

            foreach ( HUD2D item in this.allChilds ) {
                item.abstractSize = itemSize - this.borderSize;

                item.abstractPosition = itemPosition;

                if ( this.direction == LayoutDirection.VERTICAL ) {
                    itemPosition.Y += itemSize.Y;
                } else {
                    itemPosition.X += itemSize.X;
                }

                item.ClientSizeChanged();
            }
        }


        public override void ClientSizeChanged() {

            switch ( this.sizeType ) {
                case HUDType.RELATIV:
                    this.size = new Vector2( Antares.graphics.GraphicsDevice.Viewport.Width * this.abstractSize.X,
                                             Antares.graphics.GraphicsDevice.Viewport.Height * this.abstractSize.Y );
                    break;

                case HUDType.ABSOLUT:
                    this.size = new Vector2( this.abstractSize.X,
                                                 this.abstractSize.Y );
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.size = new Vector2( this.abstractSize.X,
                                                 Antares.graphics.GraphicsDevice.Viewport.Height * this.abstractSize.Y );
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.size = new Vector2( Antares.graphics.GraphicsDevice.Viewport.Width * this.abstractSize.X,
                                                 this.abstractSize.Y );
                    break;
            }

            base.ClientSizeChanged();

            if ( this.background != null ) {
                this.background.ClientSizeChanged();
            }
        }


        public void CreateBackground( bool create ) {
            if ( create ) {
                this.background = new HUD2DTexture();
                this.background.sizeType = this.sizeType;
                this.background.abstractSize = this.abstractSize;
                this.background.color = HUD2DArray.BACKGROUND_COLOR;
                this.background.setLayerDepth( this.layerDepth );
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


        public override bool Intersects( Vector2 point ) {
            if ( point.X < position.X - scale * size.X / 2 || point.X > position.X + scale * size.X / 2 ||
                point.Y < position.Y - scale * size.Y / 2 || point.Y > position.Y + scale * size.Y / 2 ) {
                return false;
            }
            return true;
        }


        public bool IsUpdatedHovered( InputProvider input ) {
            bool hover = false;
            if ( Intersects( input.getMousePos() ) ) {
                hover = true;
            }

            if ( this.background != null ) {
                if ( hover ) {
                    this.background.color = HUD2DArray.BACKGROUND_COLOR_HOVER;
                } else {
                    this.background.color = HUD2DArray.BACKGROUND_COLOR;
                }
            }

            return hover;
        }


    }

}
