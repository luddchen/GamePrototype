using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HUD.HUD;
using Microsoft.Xna.Framework;
using HUD;

namespace Battlestation_Antaris.View.HUD.CockpitHUD {

    class DockButton : HUDButton {

        static float changeSpeed = 0.03f;

        HUDTexture light;
        Color lightColor = Color.Yellow;

        float lightAmount = 0.0f;

        float lightChange = changeSpeed;

        bool activated = false;

        public DockButton(Vector2 position) : base( "Dock", position, new Vector2( 0.04f, 0.03f ), 0.8f, null ) {
            this.light = new HUDTexture( "Sprites//HUD//LampLight", null, this.AbstractSize * 2.0f );
            this.light.color = Color.Transparent;
            this.Add( this.light );
            SetBackground( "Sprites//Circle" );
        }

        public override void Update( GameTime gameTime ) {
            if ( HUDService.Input.isLeftMouseButtonPressed() && Intersects( HUDService.Input.getMousePos() ) ) {
                this.activated = !this.activated;
                this.light.color = Color.Transparent;
                this.lightAmount = 0.0f;
            }

            if ( this.activated ) {
                this.lightAmount += this.lightChange;
                if ( this.lightAmount >= 0.33f ) {
                    this.lightChange = -changeSpeed;
                }
                if ( this.lightAmount <= 0.0f ) {
                    this.lightChange = changeSpeed;
                }
                this.light.color = Color.Multiply( this.lightColor, this.lightAmount );
            }
            base.Update( gameTime );
        }


        public void Deactivate() {
            this.activated = false;
            this.light.color = Color.Transparent;
            this.lightAmount = 0.0f;
        }

        public bool isActivated() {
            return this.activated;
        }

    }

}
