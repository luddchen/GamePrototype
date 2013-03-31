using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Control;
using HUD.HUD;
using HUD;
using System;

namespace Battlestation_Antares.View.HUD.CommandHUD {
    class MouseTexture : HUDTexture, IUpdatableItem {
        public MouseTexture( Object texture, SituationController controller )
            : base( texture, Color.Blue, size: new Vector2( 0.01f, 0.01f ) ) {
            this.PositionType = HUDType.ABSOLUT;
            this.isVisible = false;

            if ( controller != null ) {
                controller.Register( this );
            }
        }

        public void Update( GameTime gameTime ) {
            this.AbstractPosition = Antares.inputProvider.getMousePos();
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }
}
