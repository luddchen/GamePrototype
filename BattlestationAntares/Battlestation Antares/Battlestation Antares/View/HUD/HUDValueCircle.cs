using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD {

    public class HUDValueCircle : HUDContainer {
        public delegate float ValueProvider();

        public ValueProvider GetValue =
            delegate() {
                return 0;
            };

        public delegate float ColorMixValue(float input);

        public ColorMixValue GetColorMixValue =
            delegate( float input) {
                return input;
            };

        private HUDTexture foreground;

        private HUDTexture overlay;

        private Color zeroColor = new Color( 0, 255, 0 );

        private Color oneColor = new Color( 255, 32, 0 );


        public HUDValueCircle( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType) : base( abstractPosition ) {
            this.SizeType = sizeType;
            this.AbstractSize = abstractSize;

            SetBackgroundColor( Color.Black );
            SetBackground( "Sprites//Circle" );

            this.foreground = new HUDTexture();
            this.foreground.PositionType = SizeType;
            this.foreground.SizeType = sizeType;
            this.foreground.AbstractSize = abstractSize * 0.95f;

            this.foreground.color = Color.White;
            this.foreground.Texture = Antares.content.Load<Texture2D>( "Sprites//Circle" );
            Add( this.foreground );

            this.overlay = new HUDTexture();
            this.overlay.PositionType = SizeType;
            this.overlay.SizeType = sizeType;
            this.overlay.AbstractSize = abstractSize;
            this.overlay.color = Color.White;
            this.SetNormal();
            Add( this.overlay );
        }

        public override float LayerDepth {
            set {
                base.LayerDepth = value;
                this.overlay.LayerDepth = value - 0.02f;
            }
        }

        public override sealed void Draw( SpriteBatch spritBatch ) {
            float value = this.GetValue();
            value = MathHelper.Clamp( value, 0.0f, 1.0f );

            this.foreground.scale = ( value + 1.0f ) / 2;
            this.foreground.ClientSizeChanged(); // until change scale attribute in HUD_Item
            this.foreground.color = Color.Lerp( zeroColor, oneColor, this.GetColorMixValue( value ) );

            base.Draw( spritBatch );
        }


        public void SetMinColor( Color minCol ) {
            this.zeroColor = minCol;
        }

        public void SetMaxColor( Color maxCol ) {
            this.oneColor = maxCol;
        }


        public void SetNormal() {
            this.overlay.Texture = Antares.content.Load<Texture2D>( "Sprites//HUD//ValueCircle" );
        }

    }

}