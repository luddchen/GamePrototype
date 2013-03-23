using System;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Control;

namespace Battlestation_Antaris.View.HUD {
    public class HUDActionTexture : HUDTexture, IUpdatableItem {

        public Action action;


        public HUDActionTexture( Action action, SituationController controller ) {
            this.action = action;
            if ( controller != null ) {
                controller.Register( this );
            }
        }


        public void Update( GameTime gameTime ) {
            if ( this.action != null ) {
                this.action();
            }
        }


        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }
}
