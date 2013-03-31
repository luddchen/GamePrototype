using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Model {

    class Missile : TactileSpatialObject {
        private int timeout;

        public Missile( TactileSpatialObject parent, float offset ) : base( "Weapon//missile", parent.globalPosition ) {
            this.rotation = parent.rotation;
            this.attributes.Engine.CurrentVelocity = parent.attributes.Missile.ProjectileVelocity;

            this.timeout = 240;

            this.globalPosition = Vector3.Add( this.globalPosition, Vector3.Multiply( this.rotation.Up, offset ) );
        }


        public override void Update( GameTime gameTime ) {
            base.Update( gameTime );
            this.timeout--;

            if ( this.timeout == 0 ) {
                Antares.world.Remove( this );
            }
        }


        public override string ToString() {
            return "Missile";
        }

    }

}