using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the menu view
    /// </summary>
    class MenuView : View
    {

        /// <summary>
        /// creates a new menu view
        /// </summary>
        /// <param name="game">the game</param>
        public MenuView(Game1 game)
            : base(game)
        {
        }
        

        /// <summary>
        /// draw the view content
        /// </summary>
        protected override void DrawContent()
        {
        }


        /// <summary>
        /// initialize menu view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // test content
            HUD2DString testString = new HUD2DString("Antaris Menu", this.game);
            testString.abstractPosition = new Vector2(0.5f, 0.1f);
            testString.positionType = HUDType.RELATIV;

            this.allHUD_2D.Add(testString);
        }

    }

}
