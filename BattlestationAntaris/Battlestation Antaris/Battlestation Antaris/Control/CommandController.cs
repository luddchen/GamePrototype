using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    class CommandController : SituationController
    {

        public CommandController(Controller controller) : base(controller) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.DOWN, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from command to cockpit");
                this.controller.switchTo(Situation.cockpit);
            }
        }

    }

}
