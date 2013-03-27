﻿using System;
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
                if ( this.buttonString != null ) {
                    this.buttonString.AbstractSize = value;
                }
                base.AbstractSize = value;
            }
        }

        public override HUDType SizeType {
            set {
                base.SizeType = value;
                this.buttonString.SizeType = value;
            }
        }


        public String Text {
            get {
                return (this.buttonString != null) ? this.buttonString.Text : "";
            }
            set {
                if ( this.buttonString != null ) {
                    this.buttonString.Text = value;
                }
            }
        }


        public HUDButton( String text, Vector2 position, Vector2 size, float scale, IUpdateController controller) : base(position, size) {
            Add( this.buttonString = new HUDString( text, Vector2.Zero, size, null, scale, null, null ) );
            _register( controller );
        }

        public HUDButton( String text, Vector2 position, float scale, IUpdateController controller) : base(position) {
            _initButtonString( text, scale );
            this.AbstractSize = Vector2.Multiply( this.buttonString.Size, new Vector2( 1.2f, 1.0f ) ); // if no size set adapt to string
            _register( controller );
        }


        private void _register( IUpdateController controller ) {
            SetBackground( this.style.backgroundColorNormal );
            if ( controller != null ) {
                controller.Register( this );
            }
        }

        private void _initButtonString(String text, float scale) {
            if ( this.buttonString == null ) {
                Add( this.buttonString = new HUDString( text, scale ) );
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
            if ( this.Intersects( HUD_Item.inputProvider.getMousePos() ) ) {
                if ( HUD_Item.inputProvider.isLeftMouseButtonPressed() ) {
                    this.buttonString.color = this.style.foregroundColorPressed;
                    SetBackground( this.style.backgroundTexturePressed, this.style.backgroundColorPressed );
                    //this.background.scale = this.style.scalePressed * this.overallScale;
                    if ( this.pressedAction != null ) {
                        this.pressedAction();
                    }
                } else {
                    if ( HUD_Item.inputProvider.isLeftMouseButtonDown() && this.downAction != null ) {
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
