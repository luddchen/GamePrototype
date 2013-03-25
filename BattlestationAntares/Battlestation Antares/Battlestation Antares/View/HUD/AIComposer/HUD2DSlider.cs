using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Control;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class HUD2DSlider : HUDContainer {

        public HUDString valueString;

        public HUDButton sliderButton;
        private float sliderButtonZero;
        private float sliderButtonOne;

        private float value;

        public HUD2DSlider( Vector2 abstractPosition, Vector2 abstractSize) : base( abstractPosition, HUDType.ABSOLUT ) {
            this.AbstractSize = abstractSize;
            this.valueString = new HUDString( "0.00" );
            this.valueString.AbstractPosition = new Vector2( 0, -this.AbstractSize.Y * 2.0f );
            this.valueString.AbstractSize = new Vector2( 0, this.AbstractSize.Y * 1.7f);

            SetBackgroundColor( new Color( 60, 64, 56, 64 ) );
            SetBackground( "Sprites//SliderBG" );

            this.sliderButton = new HUDButton( " ", new Vector2( this.sliderButtonZero, 0 ), 1, null );
            this.sliderButtonZero = - this.AbstractSize.X / 2;
            this.sliderButtonOne = this.AbstractSize.X / 2;
            this.sliderButton.PositionType = HUDType.ABSOLUT;
            this.sliderButton.AbstractSize = new Vector2( this.AbstractSize.Y , this.AbstractSize.Y ) * 2;

            this.sliderButton.style = ButtonStyle.SliderButtonStyle();
            this.sliderButton.SetBackground( Antares.content.Load<Texture2D>( "Sprites//Slider" ) );
            this.sliderButton.SetDownAction(
                delegate() {
                    float newPos = Antares.inputProvider.getMousePos().X - this.Position.X;
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
