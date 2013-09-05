using Microsoft.Xna.Framework;
using System;

namespace HUD {

    public abstract class HUDUpdateView : HUDView, IUpdateController {

        UpdateController controller;

        public String name = "HUDUpdateView";

        public HUDUpdateView( Color? backgroundColor = null ) : base( backgroundColor) {
            this.controller = new UpdateController();
        }

        public void Register( IUpdatableItem item ) {
            this.controller.Register( item );
        }

        public void Unregister( IUpdatableItem item ) {
            this.controller.Unregister( item );
        }

        public virtual void Update( GameTime gameTime ) {
            this.controller.Update( gameTime );
        }

    }

}
