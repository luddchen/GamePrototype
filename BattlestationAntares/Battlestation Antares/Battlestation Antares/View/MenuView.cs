using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD.AIComposer;
using System;
using Battlestation_Antaris.View.HUD;

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

            HUD2DString testString = new HUD2DString( "Antares Menu" );
            testString.abstractPosition = new Vector2( 0.5f, 0.1f );
            testString.positionType = HUDType.RELATIV;

            this.allHUD_2D.Add( testString );
        }

    }

}
