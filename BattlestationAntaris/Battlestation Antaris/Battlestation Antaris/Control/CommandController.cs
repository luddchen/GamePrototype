using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    class CommandController : SituationController
    {

        public CommandController(Game1 game, View.View view) : base(game, view) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from command to cockpit");
                this.game.switchTo(Situation.COCKPIT);
            }
        }

    }

}
