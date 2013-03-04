using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the command view
    /// </summary>
    class CommandView : View
    {

        /// <summary>
        /// creates a new command view
        /// </summary>
        /// <param name="game"></param>
        public CommandView(Game1 game)
            : base(game)
        {

        }

        /// <summary>
        /// initialize cammand view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // test content
            HUD2DString testString = new HUD2DString("Antaris Command", this.game);
            testString.abstractPosition = new Vector2(0.5f, 0.1f);
            testString.positionType = HUDType.RELATIV;

            this.allHUD_2D.Add(testString);
            this.allHUD_2D.Add(this.game.world.miniMap);
        }

        /// <summary>
        /// draw the command view content
        /// </summary>
        protected override void DrawContent()
        {

            // init depth buffer
            this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true, DepthBufferWriteEnable = true };
            this.game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

        }
    }

}
