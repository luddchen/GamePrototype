﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Mixer : AI_Item {

        public enum MixerType {
            AVG,
            MULTIPLY,
            MIN,
            MAX
        }

        private HUD2DSlider slider;

        public AI_Mixer( Vector2 abstractPosition, HUDType positionType, Antares game )
            : base( abstractPosition, positionType, game ) {
            this.typeString.String = "Mixer";

            AddPort( AI_ItemPort.PortType.INPUT );
            AddPort( AI_ItemPort.PortType.INPUT );
            AddPort( AI_ItemPort.PortType.OUTPUT );

            SetSubType( MixerType.AVG );
        }


        public override void SetSubType( object subType ) {
            base.SetSubType( subType );

            if ( (MixerType)subType == MixerType.AVG ) {
                this.parameters = new float[1];
                if ( this.slider == null ) {
                    this.slider = new HUD2DSlider( new Vector2( 0, this.abstractSize.Y / 4 ), new Vector2( this.abstractSize.X, this.abstractSize.Y / 2 ), game );
                    this.Add( this.slider );
                }
                this.slider.isVisible = true;

            } else {
                this.parameters = null;
                if ( this.slider != null ) {
                    this.slider.isVisible = false;
                }
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

    }

}