using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD
{

    public class HUDValueBar : HUDMaskedContainer {
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

        private HUDTexture foreground;

        private Color zeroColor = new Color(0, 255, 0);

        private Color oneColor = new Color(255, 32, 0);

        private float maxHeight;

        private bool flip;

        public HUDValueBar( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, bool flip)
            : base( abstractPosition, positionType, abstractSize, sizeType) {
            this.flip = flip;

            SetBackground( Color.Black );
            background.IsVisible = true;

            this.foreground = new HUDTexture();
            this.foreground.PositionType = this.SizeType;
            this.foreground.SizeType = sizeType;
            this.foreground.AbstractSize = abstractSize * 0.95f;

            this.foreground.color = Color.White;
            Add(this.foreground);

            this.SetNormal();
            //if ( flip ) {
            //    this.mask.effect = SpriteEffects.FlipVertically;
            //}

            this.maxHeight = this.foreground.AbstractSize.Y;
        }

        public override sealed void Draw( SpriteBatch spriteBatch ) {
            float value = this.GetValue();
            value = MathHelper.Clamp( value, 0.0f, 1.0f );

            this.foreground.AbstractSize = new Vector2( this.foreground.AbstractSize.X, this.maxHeight * value );

            if ( this.flip ) {
                this.foreground.AbstractPosition = new Vector2( this.foreground.AbstractPosition.X, ( this.foreground.AbstractSize.Y - this.maxHeight ) / 2.0f );
            } else {
                this.foreground.AbstractPosition = new Vector2( this.foreground.AbstractPosition.X, ( this.maxHeight * ( 1.0f - value ) ) / 2.0f );
            }

            this.foreground.color = Color.Lerp( this.zeroColor, this.oneColor, this.GetColorMixValue( value ) );

            base.Draw( spriteBatch );
        }


        public void SetMinColor( Color minCol ) {
            this.zeroColor = minCol;
        }

        public void SetMaxColor( Color maxCol ) {
            this.oneColor = maxCol;
        }


        public void SetDiscrete() {
            SetMask( "Sprites//HUD//ValueBar_Discrete" );
        }

        public void SetDiscreteBig() {
            SetMask( "Sprites//HUD//ValueBar_DiscreteBig" );
        }

        public void SetNormal() {
            SetMask( "Sprites//HUD//ValueBar" );
        }

    }

}