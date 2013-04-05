using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Model {

    class SkySphere : SpatialObject {

        public SkySphere() : base( "SkySphere", scale: new Vector3(9000) ) {}

        public override void Update( GameTime gameTime ) {
            this.globalPosition = Antares.world.spaceShip.globalPosition * 0.975f;
        }

    }
}
