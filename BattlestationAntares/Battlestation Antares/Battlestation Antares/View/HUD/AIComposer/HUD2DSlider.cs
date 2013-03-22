using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Control;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class HUD2DSlider : HUDContainer {

        public HUDString valueString;

        public HUDTexture sliderBackground;

        public HUDButton sliderButton;
        private float sliderButtonZero;
        private float sliderButtonOne;

        private float value;

        public HUD2DSlider( Vector2 abstractPosition, Vector2 abstractSize, SituationController controller ) : base( abstractPosition, HUDType.ABSOLUT ) {
            this.valueString = new HUDString( "0.00" );
            this.valueString.scale = 0.5f;
            this.valueString.abstractPosition = new Vector2( -abstractSize.X * 0.375f, 0 );

            this.sliderBackground = new HUDTexture();
            this.sliderBackground.abstractPosition = new Vector2( abstractSize.X * 0.1f, 0 );
            this.sliderBackground.abstractSize = new Vector2( abstractSize.X * 0.7f, 6 );
            this.sliderBackground.color = new Color( 60, 64, 56, 64 );
            this.sliderBackground.Texture = Antares.content.Load<Texture2D>( "Sprites//SliderBG" );

            this.sliderButton = new HUDButton( "", Vector2.Zero, 0.5f, controller );
            this.sliderButtonZero = abstractSize.X * 0.1f - this.sliderBackground.abstractSize.X / 2;
            this.sliderButtonOne = abstractSize.X * 0.1f + this.sliderBackground.abstractSize.X / 2;

            this.sliderButton.abstractPosition = new Vector2( this.sliderButtonZero, 0 );
            this.sliderButton.abstractSize = new Vector2( abstractSize.Y * 0.8f, abstractSize.Y * 0.8f );
            this.sliderButton.style = ButtonStyle.SliderButtonStyle();
            this.sliderButton.SetBackgroundTexture( "Sprites//Slider" );
            this.sliderButton.SetDownAction(
                delegate() {
                    float newPos = Antares.inputProvider.getMousePos().X - this.position.X;
                    if ( newPos >= this.sliderButtonZero && newPos <= this.sliderButtonOne ) {
                        SetValue( ( newPos - this.sliderButtonZero ) / this.sliderBackground.abstractSize.X );
                    }
                } );

            this.Add( this.valueString );
            this.Add( this.sliderBackground );
            this.Add( this.sliderButton );
        }

        public void SetValue( float newValue ) {
            this.sliderButton.abstractPosition.X = newValue * this.sliderBackground.abstractSize.X + this.sliderButtonZero;
            this.sliderButton.ClientSizeChanged();
            this.value = newValue;
            this.valueString.String = String.Format( "{0:F2}", this.value );
        }

        public float GetValue() {
            return this.value;
        }


    }

}
