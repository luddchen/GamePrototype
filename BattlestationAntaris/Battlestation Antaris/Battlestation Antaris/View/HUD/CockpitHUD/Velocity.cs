using Microsoft.Xna.Framework;
using System;

namespace Battlestation_Antaris.View.HUD.CockpitHUD
{

    public class Velocity : HUD2DContainer
    {

        private HUD2DValueBar posVel;

        private HUD2DValueBar negVel;

        public Velocity(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.abstractSize = abstractSize;
            this.sizeType = sizeType;

            this.posVel = new HUD2DValueBar(new Vector2(0, -this.abstractSize.Y / 4), this.sizeType, 
                                            new Vector2(abstractSize.X, abstractSize.Y / 2), this.sizeType, false, game);
            this.posVel.GetValue =
                delegate()
                {
                    return Math.Max(0, this.game.world.spaceShip.attributes.Engine.CurrentVelocity / this.game.world.spaceShip.attributes.Engine.MaxVelocity);
                };


            this.negVel = new HUD2DValueBar(new Vector2(0, this.abstractSize.Y / 4), this.sizeType,
                                            new Vector2(abstractSize.X, abstractSize.Y / 2), this.sizeType, true, game);
            this.negVel.GetValue =
                delegate()
                {
                    return -Math.Min(0, this.game.world.spaceShip.attributes.Engine.CurrentVelocity / this.game.world.spaceShip.attributes.Engine.MaxVelocity);
                };


            Add(this.posVel);
            Add(this.negVel);
        }


    }

}