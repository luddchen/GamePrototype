using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antaris;
using Battlestation_Antares.Control;

namespace Battlestation_Antares.View.HUD.CommandHUD {
    class MouseTexture : HUDTexture, IUpdatableItem {
        public MouseTexture( Texture2D texture, SituationController controller )
            : base( texture, null, new Microsoft.Xna.Framework.Vector2( 15f, 15f ), Color.Blue, null, null ) {
            this.positionType = HUDType.ABSOLUT;
            this.isVisible = false;

            if ( controller != null ) {
                controller.Register( this );
            }
        }

        public void Update( GameTime gameTime ) {
            this.abstractPosition = Antares.inputProvider.getMousePos();
            ClientSizeChanged();
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }
}
