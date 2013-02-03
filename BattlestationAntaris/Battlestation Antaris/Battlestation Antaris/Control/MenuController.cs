using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    class MenuController : SituationController
    {

        public MenuController(Game1 game, View.View view) : base(game, view) 
        {
            this.worldUpdate = WorldUpdate.NO_UPDATE;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from menu to command");
                this.game.switchTo(Situation.COMMAND);
            }
        }

    }

}
