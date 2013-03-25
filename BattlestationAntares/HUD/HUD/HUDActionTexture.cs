using System;
using Microsoft.Xna.Framework;

namespace HUD.HUD {
    public class HUDActionTexture : HUDTexture, IUpdatableItem {

        public Action action;


        public HUDActionTexture( Action action, IUpdateController controller ) {
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
