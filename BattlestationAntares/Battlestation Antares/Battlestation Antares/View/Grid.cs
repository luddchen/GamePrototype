using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antares.View {
    class Grid {
        private const float SCALE = 600;
        private const int NR_GRIDS = 8;
        private const int Y_POS = -1000;

        private Microsoft.Xna.Framework.Graphics.Model grid;
        private Matrix[] boneTransforms;


        public Grid( String name) {
            this.grid = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( name );
            boneTransforms = new Matrix[this.grid.Bones.Count];
        }


        /// <summary>
        /// draw this element
        /// </summary>
        public void Draw( Camera camera ) {
            for ( int column = 0; column < NR_GRIDS; column++ ) {
                for ( int row = 0; row < NR_GRIDS; row++ ) {
                    calcPosition( column, row );
                    Tools.Draw3D.Draw( grid, boneTransforms, camera.view, camera.projection );
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
            grid.Root.Transform = Matrix.CreateScale( SCALE ) * Matrix.CreateRotationX( MathHelper.PiOver2 * rotationDirection ) * Matrix.CreateTranslation( translation );
            grid.CopyAbsoluteBoneTransformsTo( boneTransforms );
        }
    }
}
