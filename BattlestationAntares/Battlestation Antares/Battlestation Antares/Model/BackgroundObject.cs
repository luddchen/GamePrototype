using System;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// an object that can be drawn in background with respect to its spherical coordinates and a local (viewer) rotation
    /// </summary>
    class BackgroundObject : SpatialObject {

        /// <summary>
        /// creates a new background image
        /// </summary>
        /// <param name="game"></param>
        public BackgroundObject( String modelName, Matrix rotation, float scale) : base(modelName, rotation: rotation ) {
            this.scale = new Vector3( 800 * scale );
        }

        public override void Update( GameTime gameTime ) {
            this.globalPosition = Vector3.Transform(
                Vector3.Forward,
                Matrix.CreateTranslation( -Vector3.Forward * 9000f ) * rotation 
                * Matrix.CreateTranslation( Antares.world.spaceShip.globalPosition * 0.9f ) );
        }

    }

}
