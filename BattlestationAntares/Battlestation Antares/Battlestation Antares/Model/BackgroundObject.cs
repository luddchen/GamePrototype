using System;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// an object that can be drawn in background with respect to its spherical coordinates and a local (viewer) rotation
    /// </summary>
    public class BackgroundObject {

        Microsoft.Xna.Framework.Graphics.Model bgModel;

        Matrix[] boneTransforms;

        Matrix rotation;

        Vector3 color;

        float scale;


        /// <summary>
        /// creates a new background image
        /// </summary>
        /// <param name="game"></param>
        public BackgroundObject( String name, Matrix rotation, float scale, Color color) {
            this.scale = scale;
            this.bgModel = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( name );
            this.rotation = rotation;
            boneTransforms = new Matrix[bgModel.Bones.Count];
            this.color = new Vector3( color.R / 256.0f, color.G / 256.0f, color.B / 256.0f );
        }


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw( Camera camera, int nr ) {
            Matrix world = Matrix.CreateScale( camera.farClipping * scale / 10 )
                            * Matrix.CreateTranslation( Vector3.Forward * ( camera.farClipping * 0.9f - nr ) )
                            * rotation
                            * Matrix.CreateTranslation( Antares.world.spaceShip.globalPosition );

            Tools.Draw3D.Draw( this.bgModel, this.boneTransforms, camera.view, camera.projection, world );
        }

    }

}
