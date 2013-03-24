using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD
{

    public class HUDValueBar : HUDContainer {
        public delegate float ValueProvider();

        public ValueProvider GetValue = 
            delegate() {
                return 0;
            };

        public delegate float ColorMixValue( float input );

        public ColorMixValue GetColorMixValue =
            delegate( float input ) {
                return input;
            };

        //private HUDTexture background;

        private HUDTexture foreground;

        private HUDTexture overlay;

        private Color zeroColor = new Color(0, 255, 0);

        private Color oneColor = new Color(255, 32, 0);

        private float maxHeight;

        private bool flip;

        public HUDValueBar( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, bool flip)
            : base( abstractPosition, positionType) {
            this.flip = flip;
            this.AbstractSize = abstractSize;
            this.SizeType = sizeType;

            this.background = new HUDTexture();
            this.background.positionType = this.SizeType;
            this.background.AbstractSize = abstractSize;
            this.background.SizeType = sizeType;

            this.background.color = Color.Black;
            this.background.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            Add( this.background );

            this.foreground = new HUDTexture();
            this.foreground.positionType = this.SizeType;
            this.foreground.AbstractSize = abstractSize * 0.95f;
            this.foreground.SizeType = sizeType;

            this.foreground.color = Color.White;
            this.foreground.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            Add(this.foreground);

            this.overlay = new HUDTexture();
            this.overlay.positionType = this.SizeType;
            this.overlay.AbstractSize = abstractSize;
            this.overlay.SizeType = sizeType;
            this.overlay.color = Color.White;
            this.SetNormal();
            if ( flip ) {
                this.overlay.effect = SpriteEffects.FlipVertically;
            }
            Add( this.overlay );

            this.maxHeight = this.foreground.AbstractSize.Y;
            LayerDepth = 0.5f;
        }

        public new float LayerDepth {
            set {
                base.LayerDepth = value;
                this.background.LayerDepth = this.layerDepth;
                this.overlay.LayerDepth = this.layerDepth - 0.02f;
            }
            get {
                return base.LayerDepth;
            }
        }

        public override sealed void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch ) {
            float value = this.GetValue();
            value = MathHelper.Clamp( value, 0.0f, 1.0f );

            this.foreground.AbstractSize = new Vector2( this.foreground.AbstractSize.X, this.maxHeight * value );

            if ( this.flip ) {
                this.foreground.AbstractPosition = new Vector2( this.foreground.AbstractPosition.X, ( this.foreground.AbstractSize.Y - this.maxHeight ) / 2.0f );
            } else {
                this.foreground.AbstractPosition = new Vector2( this.foreground.AbstractPosition.X, ( this.maxHeight * ( 1.0f - value ) ) / 2.0f );
            }

            this.foreground.color = Color.Lerp( this.zeroColor, this.oneColor, this.GetColorMixValue( value ) );

            this.foreground.ClientSizeChanged();

            base.Draw( spritBatch );
        }


        public void SetMinColor( Color minCol ) {
            this.zeroColor = minCol;
        }

        public void SetMaxColor( Color maxCol ) {
            this.oneColor = maxCol;
        }


        public void SetDiscrete() {
            this.overlay.Texture = Antares.content.Load<Texture2D>( "Sprites//HUD//ValueBar_Discrete" );
        }

        public void SetDiscreteBig() {
            this.overlay.Texture = Antares.content.Load<Texture2D>( "Sprites//HUD//ValueBar_DiscreteBig" );
        }

        public void SetNormal() {
            this.overlay.Texture = Antares.content.Load<Texture2D>( "Sprites//HUD//ValueBar" );
        }

    }

}