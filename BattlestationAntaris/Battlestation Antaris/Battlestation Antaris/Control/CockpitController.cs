using System;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// the cockpit controller
    /// </summary>
    class CockpitController : SituationController
    {
        
        /// <summary>
        /// create a new cockpit controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CockpitController(Game1 game, View.View view) : base(game, view) { }


        /// <summary>
        /// update the cockpit controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // experimental -- switch to menu situation if space is pressed
            if (this.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from cockpit to menu");
                this.game.switchTo(Situation.MENU);
            }

            // redirect input
            this.game.world.spaceShip.InjectControl( this.game.inputProvider.getInput() );

        }

    }

}
