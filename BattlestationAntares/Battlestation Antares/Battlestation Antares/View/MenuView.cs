using Microsoft.Xna.Framework;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the menu view
    /// </summary>
    class MenuView : HUDView {

        public MenuView( Color? backgroundColor ) : base( backgroundColor ) { }

        /// <summary>
        /// initialize menu view HUD and content
        /// </summary>
        public override void Initialize() {
            HUDString testString = new HUDString( "Antares Menu", new Vector2( 0.5f, 0.1f ), new Vector2( 0.2f, 0.075f ) );
            this.Add( testString );
        }

    }

}
