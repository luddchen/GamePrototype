using Microsoft.Xna.Framework;
using System;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD.CockpitHUD {

    public class LaserHeat : HUDValueBar {

        public LaserHeat( Vector2 abstractPosition, Vector2 abstractSize )
            : base( abstractPosition, abstractSize, false) {
            this.SetDiscrete();
            this.GetValue =
                delegate() {
                    return Antares.world.spaceShip.attributes.Laser.CurrentHeat / Antares.world.spaceShip.attributes.Laser.HeatUntilCooldown;
                };
            this.GetColorMixValue =
                delegate(float input) {
                    return (float)Math.Pow(input, 3);
                };
        }

    }

}
