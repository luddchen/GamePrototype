using System;
using Microsoft.Xna.Framework;

namespace HUD.HUD {
    public class HUDActionTexture : HUDTexture, IUpdatableItem {

        private Action action;


        public Action Action {
            set {
                this.action = value;
            }
        }


        public HUDActionTexture( Object texture = null, Color? color = null, Vector2 position = new Vector2(), Vector2 size = new Vector2(), float scale = 1.0f, float rotation = 0.0f, Action action = null, IUpdateController controller = null )
            : base(texture, color, position, size, scale, rotation)
        {
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
