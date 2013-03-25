using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;

namespace Battlestation_Antares.View {

    /// <summary>
    /// the menu view
    /// </summary>
    class MenuView : View {

        public MenuView( Color? backgroundColor ) : base( backgroundColor ) {
        }


        /// <summary>
        /// initialize menu view HUD and content
        /// </summary>
        public override void Initialize() {

            HUDString testString = new HUDString( "Antares Menu" );
            testString.PositionType = HUDType.RELATIV;
            testString.AbstractPosition = new Vector2( 0.5f, 0.1f );
            testString.SizeType = HUDType.RELATIV;
            testString.AbstractSize = new Vector2( 0.2f, 0.075f );

            this.Add( testString );
        }

    }

}
