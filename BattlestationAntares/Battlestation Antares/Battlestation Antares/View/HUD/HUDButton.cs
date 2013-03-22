using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Control;
using Battlestation_Antaris;

namespace Battlestation_Antares.View.HUD {

    public class HUDButton : HUDString, IUpdatableItem {

        public ButtonStyle style;

        private float overallScale;

        private Action pressedAction;
        private Action downAction;


        public HUDButton( String text, Vector2 position, float scale, SituationController controller)
            : base( text) {
            this.abstractPosition = position;
            this.overallScale = scale;
            this.scale = scale;
            this.style = ButtonStyle.DefaultButtonStyle();

            this.color = this.style.foregroundColorNormal;
            this.BackgroundColor = this.style.backgroundColorNormal;
            SetBackgroundTexture( "Sprites//builder_button" );

            if ( controller != null ) {
                controller.Register( this );
            }
        }

        public void SetBackgroundTexture( String background ) {
            this.BackgroundTexture = Antares.content.Load<Texture2D>( background );
            this.BackgroundTextureOrigin = new Vector2( BackgroundTexture.Width / 2, BackgroundTexture.Height / 2 );
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


        void IUpdatableItem.Update( GameTime gameTime ) {
            if ( this.Intersects( Antares.inputProvider.getMousePos() ) ) {
                if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                    this.color = this.style.foregroundColorPressed;
                    this.BackgroundColor = this.style.backgroundColorPressed;
                    this.scale = this.style.scalePressed * this.overallScale;
                    if ( this.pressedAction != null ) {
                        this.pressedAction();
                    }
                } else {
                    if ( Antares.inputProvider.isLeftMouseButtonDown() && this.downAction != null ) {
                        this.downAction();
                    }
                    this.color = this.style.foregroundColorHover;
                    this.BackgroundColor = this.style.backgroundColorHover;
                    this.scale = this.style.scaleHover * this.overallScale;
                }
            } else {
                this.color = this.style.foregroundColorNormal;
                this.BackgroundColor = this.style.backgroundColorNormal;
                this.scale = this.style.scaleNormal * this.overallScale;
            }
        }

        bool IUpdatableItem.Enabled {
            get {
                return this.IsVisible;
            }
        }
    }

}
