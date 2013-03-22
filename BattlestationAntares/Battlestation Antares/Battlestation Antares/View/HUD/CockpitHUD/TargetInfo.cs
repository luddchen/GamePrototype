using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.View.HUD.CockpitHUD {
    public class TargetInfo : HUDArray {

        private HUDString targetObject;

        private HUDString targetDistance;

        private WorldModel world;

        public SpatialObject target;


        public TargetInfo( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType)
            : base( abstractPosition, positionType, abstractSize, sizeType) 
        {
            this.world = Antares.world;

            targetObject = new HUDString( "");
            targetObject.scale = 0.5f;
            targetDistance = new HUDString( "");
            targetDistance.scale = 0.5f;

            Add( targetObject );
            Add( targetDistance );

            CreateBackground( true );

            this.isVisible = false;
        }

        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch ) {
            this.target = this.world.spaceShip.target;

            if ( target != null ) {
                this.targetObject.String = target.ToString();
                //this.targetDistance.String = String.Format("{0:F0} m", testDist);
                this.isVisible = true;
            } else {
                this.targetObject.String = "";
                this.targetDistance.String = "";
                this.isVisible = false;
            }

            base.Draw( spritBatch );
        }

    }
}
