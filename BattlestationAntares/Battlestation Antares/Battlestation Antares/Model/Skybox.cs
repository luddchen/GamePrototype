using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Model {

    class Skybox : SpatialObject {

        public Skybox() : base( "Skysphere//skysphere", scale: new Vector3(9000) ) {}

        public override void Update( GameTime gameTime ) {
            this.globalPosition = Antares.world.spaceShip.globalPosition * 0.975f;
        }

    }
}
