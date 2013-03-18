using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.View {

    /// <summary>
    /// an object that can be drawn in background with respect to its spherical coordinates and a local (viewer) rotation
    /// </summary>
    public class BackgroundObject {

        Microsoft.Xna.Framework.Graphics.Model bgModel;

        Matrix[] boneTransforms;

        Matrix rotation;

        Vector3 color;

        float scale;

        private Antares game;


        /// <summary>
        /// creates a new background image
        /// </summary>
        /// <param name="game"></param>
        public BackgroundObject( String name, Matrix rotation, float scale, Color color, Antares game ) {
            this.game = game;
            this.scale = scale;
            this.bgModel = this.game.Content.Load<Microsoft.Xna.Framework.Graphics.Model>( name );
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
                            * Matrix.CreateTranslation( this.game.world.spaceShip.globalPosition );

            Tools.Draw3D.Draw( this.bgModel, this.boneTransforms, camera.view, camera.projection, world );
        }

    }

}
