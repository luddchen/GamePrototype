﻿using Microsoft.Xna.Framework;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    class AI_Mixer : AI_Item {

        public enum MixerType {
            AVG,
            MULTIPLY,
            MIN,
            MAX
        }

        private HUD2DSlider slider;

        public AI_Mixer() : base() {
            this.typeString.Text = "Mixer";
            this.slider = new HUD2DSlider( new Vector2( 0, this.AbstractSize.Y / 4 ), new Vector2( this.AbstractSize.X * 0.9f, this.AbstractSize.Y / 10 ) );
            this.slider.IsVisible = false;
            this.Add( this.slider );

            AddPort( AI_ItemPort.PortType.INPUT );
            AddPort( AI_ItemPort.PortType.INPUT );
            AddPort( AI_ItemPort.PortType.OUTPUT );

            SetSubType( MixerType.AVG );
        }


        public override void SetSubType( object subType ) {
            base.SetSubType( subType );

            if ( (MixerType)subType == MixerType.AVG ) {
                this.parameters = new float[1];
                this.slider.IsVisible = true;

            } else {
                this.parameters = null;
                this.slider.IsVisible = false;
            }
        }

        public override void SetParameter( int index, float value ) {
            base.SetParameter( index, value );

            if ( index == 0 && this.slider != null ) {
                this.slider.SetValue( value );
            }
        }

        public override float GetParameter( int index ) {
            if ( index == 0 && this.slider != null ) {
                return this.slider.GetValue();
            }
            return base.GetParameter( index );
        }


        public override void Update( GameTime gameTime ) {
            base.Update( gameTime );
            if ( this.slider != null && this.slider.IsVisible) {
                this.slider.Update( gameTime );
            }
        }
        

    }

}
