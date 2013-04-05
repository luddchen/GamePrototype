using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.Model {

    class Laser : TactileSpatialObject {

        public TactileSpatialObject parent;

        private float upOffset;

        private float rightOffset;

        private float forwardOffset = 0;

        // only for test the visual illusion
        private static float FUN_FACTOR = 1.0f;

        public Laser( TactileSpatialObject parent, float upOffset, float rightOffset ) : base( "Laser", parent.globalPosition ) {
            this.parent = parent;
            this.rotation = parent.rotation;
            this.upOffset = upOffset;
            this.rightOffset = rightOffset;
            this.attributes.Engine.CurrentVelocity = this.parent.attributes.Laser.ProjectileVelocity;
        }

        protected override void _initMiniMapIcon() {
            this.miniMapIcon.color = MiniMap.WEAPON_COLOR;
            this.miniMapIcon.AbstractScale = 0.4f;
        }


        public override void Update( GameTime gameTime ) {

            this.rotation = this.parent.rotation;

            this.forwardOffset += this.attributes.Engine.CurrentVelocity;

            this.globalPosition = this.parent.globalPosition
                                + this.rotation.Forward * this.forwardOffset
                                + this.rotation.Up * this.upOffset
                                + this.rotation.Right * this.rightOffset;

            if ( this.forwardOffset < this.parent.attributes.Laser.Range / 2.0f ) {
                this.scale.Z = this.forwardOffset * FUN_FACTOR;
            } else {
                this.scale.Z = (this.parent.attributes.Laser.Range - this.forwardOffset) * FUN_FACTOR;

                // remove if out of range
                if ( this.forwardOffset > this.parent.attributes.Laser.Range ) {
                    Antares.world.Remove( this );
                }
            }
        }

    }

}
