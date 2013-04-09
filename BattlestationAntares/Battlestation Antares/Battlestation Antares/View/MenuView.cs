using Microsoft.Xna.Framework;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the menu view
    /// </summary>
    class MenuView : HUDView {

        public MenuView( Color? backgroundColor  = null ) : base( backgroundColor ) { }

        /// <summary>
        /// initialize menu view HUD and content
        /// </summary>
        public override void Initialize() {
            this.Add( new HUDTexture( "Sprites//Title01", position: new Vector2(0.5f, 0.12f), size: new Vector2(0.8f, 0.2f) ) );
        }

    }

}
