using Battlestation_Antares.View;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Model {

    class Grid : SpatialObject {

        private const float SCALE = 600;
        private const int NR_GRIDS = 8;
        private const int Y_POS = -1000;

        public Grid() : base( "Grid" ) {}

        /// <summary>
        /// draw this element
        /// </summary>
        public override void Draw( Camera camera ) {
            if ( this.isVisible ) {
                for ( int column = 0; column < NR_GRIDS; column++ ) {
                    for ( int row = 0; row < NR_GRIDS; row++ ) {
                        calcPosition( column, row );
                        Tools.Draw3D.Draw( model, boneTransforms, camera.view, camera.projection );
                    }
                }
            }
        }

        private void calcPosition( int column, int row ) {
            int rotationDirection = 1;
            Vector3 translation = Antares.world.spaceShip.globalPosition;
            translation.Y = Y_POS;
            translation.X = ( (int)( translation.X / ( 2 * SCALE ) ) ) * 2 * SCALE + 2 * SCALE * column - SCALE * NR_GRIDS;
            translation.Z = ( (int)( translation.Z / ( 2 * SCALE ) ) ) * 2 * SCALE + 2 * SCALE * row - SCALE * NR_GRIDS;
            if ( Antares.world.spaceShip.globalPosition.Y >= Y_POS ) {
                rotationDirection = -1;
            }
            model.Root.Transform = Matrix.CreateScale( SCALE ) * Matrix.CreateRotationX( MathHelper.PiOver2 * rotationDirection ) * Matrix.CreateTranslation( translation );
            model.CopyAbsoluteBoneTransformsTo( boneTransforms );
        }
    }
}
