﻿using Microsoft.Xna.Framework;
using System;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD.CockpitHUD {

    public class Velocity : HUDContainer {

        private HUDValueBar posVel;

        private HUDValueBar negVel;

        public Velocity( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType)
            : base( abstractPosition ) {
            this.AbstractSize = abstractSize;
            this.SizeType = sizeType;

            this.posVel = new HUDValueBar( new Vector2( 0, -this.AbstractSize.Y / 4 ), this.SizeType,
                                            new Vector2( abstractSize.X, abstractSize.Y * 0.48f ), this.SizeType, false);
            this.posVel.GetValue =
                delegate() {
                    return Math.Max( 0, Antares.world.spaceShip.attributes.Engine.CurrentVelocity / Antares.world.spaceShip.attributes.Engine.MaxVelocity );
                };
            this.posVel.SetDiscreteBig();
            this.posVel.SetMaxColor( Color.Yellow );

            this.negVel = new HUDValueBar( new Vector2( 0, this.AbstractSize.Y / 4 ), this.SizeType,
                                            new Vector2( abstractSize.X, abstractSize.Y * 0.48f ), this.SizeType, true);
            this.negVel.GetValue =
                delegate() {
                    return -Math.Min( 0, Antares.world.spaceShip.attributes.Engine.CurrentVelocity / Antares.world.spaceShip.attributes.Engine.MaxVelocity );
                };
            this.negVel.SetDiscreteBig();
            this.negVel.SetMaxColor(Color.Yellow);

            Add( this.posVel );
            Add( this.negVel );
            RenderSizeChanged();
        }


    }

}