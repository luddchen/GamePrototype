using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    class CockpitController : SituationController
    {
        
        public CockpitController(Game1 game, View.View view) : base(game, view) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from cockpit to menu");
                this.game.switchTo(Situation.MENU);
            }

            this.game.world.spaceShip.InjectControl( this.game.inputProvider.getInput() );

        }

    }

}
