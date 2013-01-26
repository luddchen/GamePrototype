using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    class MenuController : SituationController
    {

        public MenuController(Controller controller) : base(controller) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from menu to command");
                this.controller.switchTo(Situation.command);
            }
        }

    }

}
