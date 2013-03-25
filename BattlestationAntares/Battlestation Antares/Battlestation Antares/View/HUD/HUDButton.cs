using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Control;
using Battlestation_Antaris;

namespace Battlestation_Antares.View.HUD {

    public class HUDButton : HUDContainer, IUpdatableItem {

        public ButtonStyle style;

        private float overallScale;

        private Action pressedAction;
        private Action downAction;

        private HUDString buttonString;

        public override Vector2 AbstractSize {
            set {
                base.AbstractSize = value;
                this.buttonString.AbstractSize = value;
            }
        }

        public override HUDType SizeType {
            set {
                base.SizeType = value;
                this.buttonString.SizeType = value;
            }
        }

        public HUDButton( String text, Vector2 position, float scale, SituationController controller) : base(position, HUDType.RELATIV) {
            this.buttonString = new HUDString( text );
            this.buttonString.scale = scale;
            Add( this.buttonString );
            this.AbstractSize = Vector2.Multiply( this.buttonString.Size, new Vector2( 1.2f, 1.0f ) ); // if no size set adapt to string

            this.AbstractPosition = position;
            this.overallScale = 1.0f;
            this.style = ButtonStyle.DefaultButtonStyle();

            SetBackgroundColor( this.style.backgroundColorNormal );
            SetBackground( Antares.content.Load<Texture2D>( "Sprites//builder_button" ) );

            if ( controller != null ) {
                controller.Register( this );
            }
        }


        public void SetPressedAction( Action action ) {
            this.pressedAction = action;
        }

        public void SetDownAction( Action action ) {
            this.downAction = action;
        }


        public void Toggle() {
            Color temp = this.style.foregroundColorHover;
            this.style.foregroundColorHover = this.style.foregroundColorNormal;
            this.style.foregroundColorNormal = temp;
        }


        public void Update( GameTime gameTime ) {
            if ( this.Intersects( Antares.inputProvider.getMousePos() ) ) {
                if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                    this.buttonString.color = this.style.foregroundColorPressed;
                    SetBackgroundColor( this.style.backgroundColorPressed );
                    this.background.scale = this.style.scalePressed * this.overallScale;
                    if ( this.pressedAction != null ) {
                        this.pressedAction();
                    }
                } else {
                    if ( Antares.inputProvider.isLeftMouseButtonDown() && this.downAction != null ) {
                        this.downAction();
                    }
                    this.buttonString.color = this.style.foregroundColorHover;
                    SetBackgroundColor( this.style.backgroundColorHover );
                    this.background.scale = this.style.scaleHover * this.overallScale;
                }

            } else {
                this.buttonString.color = this.style.foregroundColorNormal;
                SetBackgroundColor( this.style.backgroundColorNormal );
                this.background.scale = this.style.scaleNormal * this.overallScale;
            }
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }

}
