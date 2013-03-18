using Microsoft.Xna.Framework;
using System;

namespace Battlestation_Antares.View.HUD.CockpitHUD {

    public class Velocity : HUD2DContainer {

        private HUD2DValueBar posVel;

        private HUD2DValueBar negVel;

        public Velocity( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType)
            : base( abstractPosition, positionType) {
            this.abstractSize = abstractSize;
            this.sizeType = sizeType;

            this.posVel = new HUD2DValueBar( new Vector2( 0, -this.abstractSize.Y / 4 ), this.sizeType,
                                            new Vector2( abstractSize.X, abstractSize.Y * 0.48f ), this.sizeType, false);
            this.posVel.GetValue =
                delegate() {
                    return Math.Max( 0, Antares.world.spaceShip.attributes.Engine.CurrentVelocity / Antares.world.spaceShip.attributes.Engine.MaxVelocity );
                };
            this.posVel.SetDiscreteBig();
            this.posVel.SetMaxColor( Color.Yellow );

            this.negVel = new HUD2DValueBar( new Vector2( 0, this.abstractSize.Y / 4 ), this.sizeType,
                                            new Vector2( abstractSize.X, abstractSize.Y * 0.48f ), this.sizeType, true);
            this.negVel.GetValue =
                delegate() {
                    return -Math.Min( 0, Antares.world.spaceShip.attributes.Engine.CurrentVelocity / Antares.world.spaceShip.attributes.Engine.MaxVelocity );
                };
            this.negVel.SetDiscreteBig();
            this.negVel.SetMaxColor(Color.Yellow);

            Add( this.posVel );
            Add( this.negVel );
        }


    }

}