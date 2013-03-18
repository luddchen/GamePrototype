using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Transformer : AI_Item {

        public enum TransformerType {
            SCALE,
            SQR,
            SQRT,
            INVERSE
        }

        private HUD2DSlider slider;

        public AI_Transformer( Vector2 abstractPosition, HUDType positionType )
            : base( abstractPosition, positionType ) {
            this.typeString.String = "Transformer";

            AddPort( AI_ItemPort.PortType.INPUT );
            AddPort( AI_ItemPort.PortType.OUTPUT );

            SetSubType( TransformerType.SCALE );
        }


        public override void SetSubType( object subType ) {
            base.SetSubType( subType );

            if ( (TransformerType)subType == TransformerType.SCALE ) {
                this.parameters = new float[1];
                if ( this.slider == null ) {
                    this.slider = new HUD2DSlider( new Vector2( 0, this.abstractSize.Y / 4 ), new Vector2( this.abstractSize.X, this.abstractSize.Y / 2 ) );
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
