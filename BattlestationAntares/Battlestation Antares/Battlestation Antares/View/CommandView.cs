using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the command view
    /// </summary>
    class CommandView : View {

        public CommandView( Color? backgroundColor ) : base( backgroundColor ) {
        }

        /// <summary>
        /// initialize cammand view HUD and content
        /// </summary>
        public override void Initialize() {
            // test content
            HUDString testString = new HUDString( "Antares Command");
            testString.abstractPosition = new Vector2( 0.5f, 0.1f );
            testString.positionType = HUDType.RELATIV;

            this.Add( testString );
            this.Add( Antares.world.miniMap );
        }

    }

}
