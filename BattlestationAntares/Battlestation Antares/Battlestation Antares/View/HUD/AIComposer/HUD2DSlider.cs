using System;
using Microsoft.Xna.Framework;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class HUD2DSlider : HUDContainer {

        public HUDString valueString;

        public HUDButton sliderButton;
        private float sliderButtonZero;
        private float sliderButtonOne;

        private float value;

        public HUD2DSlider( Vector2 abstractPosition, Vector2 abstractSize) : base( abstractPosition ) {
            this.AbstractSize = abstractSize;
            this.valueString = new HUDString( "0.00" );
            this.valueString.AbstractPosition = new Vector2( 0, -this.AbstractSize.Y * 2.0f );
            this.valueString.AbstractSize = new Vector2( 0, this.AbstractSize.Y * 1.7f);

            SetBackground( "Sprites//SliderBG", new Color( 60, 64, 56, 64 ) );

            this.sliderButton = new HUDButton();
            this.sliderButtonZero = - this.AbstractSize.X / 2;
            this.sliderButtonOne = this.AbstractSize.X / 2;
            this.sliderButton.AbstractSize = new Vector2( this.AbstractSize.Y , this.AbstractSize.Y ) * 2;

            this.sliderButton.style = ButtonStyle.SliderButtonStyle();
            this.sliderButton.SetBackground( "Sprites//Slider" );
            this.sliderButton.SetDownAction(
                delegate() {
                    float newPos = HUD_Item.ConcreteToAbstract( Antares.inputProvider.getMousePos() - this.Position ).X / this.Scale;
                    if ( newPos >= this.sliderButtonZero && newPos <= this.sliderButtonOne ) {
                        SetValue( ( newPos - this.sliderButtonZero ) / this.AbstractSize.X );
                    }
                } );

            this.Add( this.valueString );
            this.Add( this.sliderButton );
            SetValue( 0.5f );
        }

        public void SetValue( float newValue ) {
            this.sliderButton.AbstractPosition = 
                new Vector2( newValue * this.AbstractSize.X + this.sliderButtonZero, this.sliderButton.AbstractPosition.Y );
            this.value = newValue;
            this.valueString.Text = String.Format( "{0:F2}", this.value );
        }

        public float GetValue() {
            return this.value;
        }

        public void Update(GameTime gameTime) {
            this.sliderButton.Update( gameTime );
        }


    }

}
