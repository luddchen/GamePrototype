using Microsoft.Xna.Framework;

namespace HUD.HUD {

    public enum LayoutDirection {
        HORIZONTAL,
        VERTICAL
    }


    public class HUDArray : HUDContainer {

        public static float ABSOLUT_BORDER = 2;

        public static float RELATIV_BORDER = 0.001f;


        public Vector2 borderSize;

        public LayoutDirection direction = LayoutDirection.VERTICAL;

        public override HUDType SizeType {
            set {
                switch ( value ) {
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

                base.SizeType = value;
            }
        }


        public HUDArray( Vector2 position, Vector2 size ) : base( position, size ) { }


        public override void Add( HUD_Item item ) {
            item.SizeType = this.SizeType;
            item.PositionType = this.SizeType;
            base.Add( item );
            Arrange();
        }


        public override void Remove( HUD_Item item ) {
            base.Remove( item );
            Arrange();
        }


        private void Arrange() {
            Vector2 itemSize = this.AbstractSize;
            Vector2 itemPosition = new Vector2();

            if ( this.direction == LayoutDirection.VERTICAL ) {
                itemSize.Y = itemSize.Y / this.AllChilds.Count;
                itemPosition.Y = -( this.AbstractSize.Y / 2 ) + ( itemSize.Y / 2 );
            } else {
                itemSize.X = itemSize.X / this.AllChilds.Count;
                itemPosition.X = -( this.AbstractSize.X / 2 ) + ( itemSize.X / 2 );
            }

            foreach ( HUD_Item item in this.AllChilds ) {
                item.AbstractSize = itemSize - this.borderSize;
                item.AbstractPosition = itemPosition;

                if ( this.direction == LayoutDirection.VERTICAL ) {
                    itemPosition.Y += itemSize.Y;
                } else {
                    itemPosition.X += itemSize.X;
                }
            }
        }

    }

}
