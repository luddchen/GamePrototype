using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;
using HUD.HUD;
using Battlestation_Antaris.Model;
using System;

namespace Battlestation_Antares.View.HUD.CockpitHUD {

    class TargetInfo : HUDArray {

        private HUDString targetObject;

        public TactileSpatialObject target;


        public TargetInfo( Vector2 abstractPosition, Vector2 abstractSize )
            : base( abstractPosition, abstractSize ) 
        {
            targetObject = new HUDString( " " );
            Add( targetObject );
            this.isVisible = false;
        }

        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch ) {
            this.target = Antares.world.spaceShip.target;

            if ( target != null ) {
                if ( target.attributes.Hull.CurrentHealthPoints / target.attributes.Hull.MaxHealthPoints > 0 ) {
                    this.targetObject.Text =
                        target.ToString() + " : "
                        + String.Format( " Shield {0:F0} %", 100.0f * target.attributes.Shield.CurrentHealthPoints / target.attributes.Shield.MaxHealthPoints )
                        + String.Format( " Hull {0:F0} %", 100f * target.attributes.Hull.CurrentHealthPoints / target.attributes.Hull.MaxHealthPoints );
                } else {
                    this.targetObject.Text = target.ToString() + " is dead ";
                }
                this.isVisible = true;
            } else {
                this.targetObject.Text = " ";
                //this.targetDistance.Text = " ";
                this.isVisible = false;
            }

            base.Draw( spritBatch );
        }

    }
}
