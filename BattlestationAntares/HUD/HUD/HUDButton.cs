using System;
using Microsoft.Xna.Framework;

namespace HUD.HUD {

    public class HUDButton : HUDContainer, IUpdatableItem {

        public ButtonStyle style = ButtonStyle.DefaultButtonStyle();

        //private float overallScale = 1.0f;

        private Action pressedAction;
        private Action downAction;

        private HUDString buttonString;

        public override Vector2 AbstractSize {
            set {
                _initButtonString();
                this.buttonString.AbstractSize = value;
                base.AbstractSize = value;
            }
        }

        public override HUDType SizeType {
            set {
                _initButtonString();
                this.buttonString.SizeType = value;
                base.SizeType = value;
            }
        }


        public String Text {
            get {
                return (this.buttonString != null) ? this.buttonString.Text : "";
            }
            set {
                _initButtonString();
                this.buttonString.Text = value;
            }
        }


        public HUDButton( String text, Vector2 position, Vector2 size, float scale, IUpdateController controller) : base(position, size) {
            Text = text;
            this.buttonString.AbstractScale = scale;
            _register( controller );
        }

        public HUDButton( String text, Vector2 position, float scale, IUpdateController controller) : base(position) {
            _initButtonString();
            Text = text;
            this.buttonString.AbstractScale = scale;
            this.buttonString.AbstractScale = scale;
            this.AbstractSize = Vector2.Multiply( this.buttonString.Size, new Vector2( 1.2f, 1.0f ) ); // if no size set it in adaptation to th text
            _register( controller );
        }


        private void _register( IUpdateController controller ) {
            SetBackground( this.style.backgroundColorNormal );
            if ( controller != null ) {
                controller.Register( this );
            }
        }

        private void _initButtonString() {
            if ( this.buttonString == null ) {
                Add( this.buttonString = new HUDString( " " ) );
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


        public virtual void Update( GameTime gameTime ) {
            if ( this.Intersects( HUDService.Input.getMousePos() ) ) {
                if ( HUDService.Input.isLeftMouseButtonPressed() ) {
                    this.buttonString.color = this.style.foregroundColorPressed;
                    SetBackground( this.style.backgroundTexturePressed, this.style.backgroundColorPressed );
                    //this.background.scale = this.style.scalePressed * this.overallScale;
                    if ( this.pressedAction != null ) {
                        this.pressedAction();
                    }
                } else {
                    if ( HUDService.Input.isLeftMouseButtonDown() && this.downAction != null ) {
                        this.downAction();
                    }
                    this.buttonString.color = this.style.foregroundColorHover;
                    SetBackground( this.style.backgroundTextureHover, this.style.backgroundColorHover );
                    //this.background.scale = this.style.scaleHover * this.overallScale;
                }

            } else {
                this.buttonString.color = this.style.foregroundColorNormal;
                SetBackground( this.style.backgroundTextureNormal, this.style.backgroundColorNormal );
                //this.background.scale = this.style.scaleNormal * this.overallScale;
            }
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }

}
