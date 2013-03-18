using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD.CockpitHUD {

    public class LaserHeat : HUD2DValueBar {

        public LaserHeat( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, Antares game )
            : base( abstractPosition, positionType, abstractSize, sizeType, false, game ) {
            this.GetValue =
                delegate() {
                    return this.game.world.spaceShip.attributes.Laser.CurrentHeat / this.game.world.spaceShip.attributes.Laser.HeatUntilCooldown;
                };
        }

    }

}
