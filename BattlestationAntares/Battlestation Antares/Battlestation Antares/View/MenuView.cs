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

        /// <summary>
        /// draw the view content
        /// </summary>
        protected override void DrawPreContent() {
        }

        /// <summary>
        /// draw the view post content
        /// </summary>
        protected override void DrawPostContent() {
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
