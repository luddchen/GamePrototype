using System;
using System.Collections.Generic;

namespace Battlestation_Antaris.Control
{

    class CockpitController : SituationController
    {
        
        public CockpitController(Controller controller) : base(controller) { }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.SPACE, ControlState.PRESSED))
            {
                Console.Out.WriteLine("switch from cockpit to menu");
                this.controller.switchTo(Situation.menu);
            }

            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.UP, ControlState.DOWN))
            {
                this.controller.spaceShip.ship.rotateX((float)(Math.PI / 360));
            }

            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.DOWN, ControlState.DOWN))
            {
                this.controller.spaceShip.ship.rotateX(-(float)(Math.PI / 360));
            }

            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.LEFT, ControlState.DOWN))
            {
                this.controller.spaceShip.ship.rotateZ((float)(Math.PI / 360));
                //this.controller.spaceShip.ship.rotateY(-(float)(Math.PI / 360));
            }

            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.RIGHT, ControlState.DOWN))
            {
                this.controller.spaceShip.ship.rotateZ(-(float)(Math.PI / 360));
                //this.controller.spaceShip.ship.rotateY((float)(Math.PI / 360));
            }

            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.SPEEDUP, ControlState.DOWN))
            {
                this.controller.spaceShip.ship.speed += 0.01f;
            }

            if (this.controller.game.inputProvider.isKeyOnState(ControlKey.SPEEDDOWN, ControlState.DOWN))
            {
                this.controller.spaceShip.ship.speed -= 0.01f;
            }

        }

    }

}
