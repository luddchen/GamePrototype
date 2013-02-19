using System;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the command controller
    /// </summary>
    class CommandController : SituationController
    {

        /// <summary>
        /// create a new command controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CommandController(Game1 game, View.View view) : base(game, view) { }


        /// <summary>
        /// update the command controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // experimental -- switch to copckpit situation if space is pressed
            if (this.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from command to cockpit");
                this.game.switchTo(Situation.COCKPIT);
            }
        }

    }

}
