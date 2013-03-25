using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Control;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View.HUD.CommandHUD {
    class MouseTexture : HUDTexture, IUpdatableItem {
        public MouseTexture( Texture2D texture, SituationController controller )
            : base( texture, null, new Vector2( 15f, 15f ), Color.Blue, null, null ) {
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
