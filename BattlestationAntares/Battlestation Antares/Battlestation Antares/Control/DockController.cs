using System;
using Battlestation_Antares;
using Battlestation_Antares.Control;
using Battlestation_Antaris.View;
using HUD;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control {

    class DockController : SituationController {

        bool undock = true;

        private float rot = 0.0f;

        private float rotSpeed = 0.025f;

        private float distance = 200.0f;

        public DockController( Antares game, HUDView view ) : base( game, view ) {
        }

        public override void onEnter() {
            Antares.world.spaceShip.isVisible = true;
            Antares.world.grid.isVisible = false;
            if ( this.undock ) {
                Antares.world.spaceStation.OpenDock( 100 );
            } else {
                Antares.world.spaceStation.CloseDock( 100 );
            }
        }

        public override void onExit() {
            Antares.world.spaceShip.isVisible = false;
            Antares.world.grid.isVisible = true;
            this.undock = !this.undock;
        }

        public override void Update( GameTime gameTime ) {
            base.Update( gameTime );

            Antares.world.Update( gameTime );
            Antares.world.spaceShip.globalPosition = Antares.world.spaceStation.AirlockCurrentPosition;
            Antares.world.spaceShip.rotation = Matrix.Identity;

            this.rot += this.rotSpeed;
            if ( this.rot >= MathHelper.TwoPi ) {
                this.rot = 0.0f;
                if ( this.undock ) {
                    this.game.switchTo( Situation.COCKPIT );
                } else {
                    this.game.switchTo( Situation.COMMAND );
                }
            }
            ( (DockView)this.view ).Rotation = this.rot;
            if ( this.undock ) {
                ( (DockView)this.view ).Distance = this.distance * (float)Math.Cos( 0.63f - this.rot * 0.1f );
            } else {
                ( (DockView)this.view ).Distance = this.distance * (float)Math.Cos( this.rot * 0.1f );
            }
        }

    }

}
