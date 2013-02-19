using System;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the Menu controller
    /// </summary>
    class MenuController : SituationController
    {

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController(Game1 game, View.View view) : base(game, view) 
        {
            this.worldUpdate = WorldUpdate.NO_UPDATE;
        }


        /// <summary>
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // experimental -- switch to command situation if space is pressed
            if (this.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from menu to command");
                this.game.switchTo(Situation.COMMAND);
            }
        }

    }

}
