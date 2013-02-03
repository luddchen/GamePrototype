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

            if (this.game.inputProvider.isKeyOnState(ControlKey.UP, ControlState.DOWN))
            {
                this.game.world.spaceShip.rotateX((float)(Math.PI / 360));
            }

            if (this.game.inputProvider.isKeyOnState(ControlKey.DOWN, ControlState.DOWN))
            {
                this.game.world.spaceShip.rotateX(-(float)(Math.PI / 360));
            }

            if (this.game.inputProvider.isKeyOnState(ControlKey.LEFT, ControlState.DOWN))
            {
                this.game.world.spaceShip.rotateZ((float)(Math.PI / 360));
            }

            if (this.game.inputProvider.isKeyOnState(ControlKey.RIGHT, ControlState.DOWN))
            {
                this.game.world.spaceShip.rotateZ(-(float)(Math.PI / 360));
            }

            if (this.game.inputProvider.isKeyOnState(ControlKey.SPEEDUP, ControlState.DOWN))
            {
                this.game.world.spaceShip.speed += 0.01f;
            }

            if (this.game.inputProvider.isKeyOnState(ControlKey.SPEEDDOWN, ControlState.DOWN))
            {
                this.game.world.spaceShip.speed -= 0.01f;
            }

        }

    }

}
