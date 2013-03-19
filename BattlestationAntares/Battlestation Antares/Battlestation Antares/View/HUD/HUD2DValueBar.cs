using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD
{

    public class HUD2DValueBar : HUD2DContainer {
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

        private HUD2DTexture background;

        private HUD2DTexture foreground;

        private HUD2DTexture overlay;

        private Color zeroColor = new Color(0, 255, 0);

        private Color oneColor = new Color(255, 32, 0);

        private float maxHeight;

        private bool flip;

        public HUD2DValueBar( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, bool flip)
            : base( abstractPosition, positionType) {
            this.flip = flip;
            this.abstractSize = abstractSize;
            this.sizeType = sizeType;

            this.background = new HUD2DTexture();
            this.background.positionType = this.sizeType;
            this.background.abstractSize = abstractSize;
            this.background.sizeType = sizeType;

            this.background.color = Color.Black;
            this.background.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            Add( this.background );

            this.foreground = new HUD2DTexture();
            this.foreground.positionType = this.sizeType;
            this.foreground.abstractSize = abstractSize;
            this.foreground.abstractSize *= 0.95f;
            this.foreground.sizeType = sizeType;

            this.foreground.color = Color.White;
            this.foreground.Texture = Antares.content.Load<Texture2D>( "Sprites//Square" );
            Add(this.foreground);

            this.overlay = new HUD2DTexture();
            this.overlay.positionType = this.sizeType;
            this.overlay.abstractSize = abstractSize;
            this.overlay.sizeType = sizeType;
            this.overlay.color = Color.White;
            this.SetNormal();
            if ( flip ) {
                this.overlay.effect = SpriteEffects.FlipVertically;
            }
            Add( this.overlay );

            this.maxHeight = this.foreground.abstractSize.Y;
            setLayerDepth( 0.5f );
        }

        public override void setLayerDepth( float layerDepth ) {
            base.setLayerDepth( layerDepth );
            this.background.layerDepth = this.layerDepth;
            this.overlay.layerDepth = this.layerDepth - 0.02f;
        }

        public override sealed void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch ) {
            float value = this.GetValue();
            value = MathHelper.Clamp( value, 0.0f, 1.0f );

            this.foreground.abstractSize.Y = this.maxHeight * value;

            if ( this.flip ) {
                this.foreground.abstractPosition.Y = ( this.foreground.abstractSize.Y - this.maxHeight ) / 2.0f;
            } else {
                this.foreground.abstractPosition.Y = ( this.maxHeight * ( 1.0f - value ) ) / 2.0f;
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