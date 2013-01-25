using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    class CockpitController : SituationController
    {
        
        public CockpitController(Controller controller) : base(controller) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.DOWN, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from cockpit to menu");
                this.controller.switchTo(Situation.menu);
            }
        }

    }

}
