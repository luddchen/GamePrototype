using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// an object that can be drawn in background with respect to its spherical coordinates and a local (viewer) rotation
    /// </summary>
    public class BackgroundObject
    {

        Microsoft.Xna.Framework.Graphics.Model bgModel;

        Matrix[] boneTransforms;

        Matrix rotation;

        Vector3 color;


        private Game1 game;


        /// <summary>
        /// creates a new background image
        /// </summary>
        /// <param name="game"></param>
        public BackgroundObject(String name, Matrix rotation, Color color, Game1 game)
        {
            this.game = game;
            this.bgModel = this.game.Content.Load<Microsoft.Xna.Framework.Graphics.Model>(name);
            this.rotation = rotation;
            boneTransforms = new Matrix[bgModel.Bones.Count];
            this.color = new Vector3(color.R / 256.0f, color.G / 256.0f, color.B / 256.0f);
        }


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw( Camera camera, int nr)
        {
            bgModel.Root.Transform = Matrix.CreateScale(camera.farClipping / 10)
                        * Matrix.CreateTranslation(Vector3.Forward * (camera.farClipping * 0.99f - nr))
                        * rotation
                        * Matrix.CreateTranslation(this.game.world.spaceShip.globalPosition);

            bgModel.CopyAbsoluteBoneTransformsTo(boneTransforms);

            foreach (ModelMesh mesh in bgModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.LightingEnabled = true;
                    effect.DirectionalLight0.DiffuseColor = this.color;
                    effect.DirectionalLight0.Direction = this.rotation.Forward;
                    effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);

                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = camera.view;
                    effect.Projection = camera.projection;
                }
                mesh.Draw();
            }
        }

    }

}
