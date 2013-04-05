using Microsoft.Xna.Framework;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the command view
    /// </summary>
    class CommandView : HUDView {

        public CommandView( Color? backgroundColor = null ) : base( backgroundColor ) { }

        /// <summary>
        /// initialize cammand view HUD and content
        /// </summary>
        public override void Initialize() {
            // test content
            HUDString testString = new HUDString( "Antares Command", null, null, position: new Vector2( 0.5f, 0.1f ) );
            this.Add( testString );
            this.Add( Antares.world.miniMapRenderer );
        }

    }

}
