using System;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// an object that can be drawn in background with respect to its spherical coordinates and a local (viewer) rotation
    /// </summary>
    class BackgroundObject : SpatialObject {

        private Vector3 global;

        private Matrix updateTransform;

        private bool doTransform = false;

        /// <summary>
        /// creates a new background image
        /// </summary>
        /// <param name="game"></param>
        public BackgroundObject( String modelName, Matrix rotation, float scale, Matrix? updateTransform) : base(modelName, rotation: rotation ) {
            if ( updateTransform.HasValue ) {
                this.updateTransform = updateTransform.Value;
                this.doTransform = true;
            }
            this.scale = new Vector3( 700 * scale );
            this.global = Vector3.Transform( Vector3.Forward, Matrix.CreateTranslation( -Vector3.Forward * 5000f ) * rotation );
        }

        public override void Update( GameTime gameTime ) {
            this.globalPosition = Vector3.Transform( this.global, Matrix.CreateTranslation( Antares.world.spaceShip.globalPosition * 0.9f ) );
            if ( this.doTransform ) {
                this.rotation *= this.updateTransform;
            }
        }

    }

}
