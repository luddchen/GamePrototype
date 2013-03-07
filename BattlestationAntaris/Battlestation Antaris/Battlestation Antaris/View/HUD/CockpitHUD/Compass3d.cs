using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.CockpitHUD
{

    /// <summary>
    /// represents an 3D compass as HUD element
    /// </summary>
    public class Compass3d : HUD3D
    {

        /// <summary>
        /// the Graphics device, necessary for viewport aspect ratio
        /// </summary>
        private GraphicsDevice device;


        /// <summary>
        /// the compass 3d model
        /// </summary>
        private Microsoft.Xna.Framework.Graphics.Model model3d;


        /// <summary>
        /// the transformation matrices of the 3d model parts
        /// </summary>
        private Matrix[] boneTransforms;


        /// <summary>
        /// the spatial object (e.g. spaceship) that contains the compass
        /// </summary>
        private SpatialObject source;


        /// <summary>
        /// the targeted 3d point
        /// </summary>
        public Vector3 target;


        /// <summary>
        /// creates a new compass instance
        /// </summary>
        /// <param name="content">game content manager</param>
        /// <param name="device">game graphics device</param>
        public Compass3d(ContentManager content, GraphicsDevice device)
        {
            // init 3d model and its transformation matrices
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models/compass2");
            this.boneTransforms = new Matrix[model3d.Bones.Count];

            this.device = device;
            this.target = new Vector3();    // target vector = zero
        }


        /// <summary>
        /// initialize the compass on the specified spatial object that contains this compass
        /// </summary>
        /// <param name="source"></param>
        public void Initialize(SpatialObject source)
        {
            this.source = source;
        }


        /// <summary>
        /// draw the compass 3d model
        /// </summary>
        public override void Draw()
        {

            // if source is set
            if (this.source != null)
            {
                // get distance vector
                Vector3 pointer = Vector3.Subtract(this.target, this.source.globalPosition);

                // get local rotation
                Vector3 rot = Tools.Tools.GetRotation(pointer, this.source.rotation);

                // rotate, scale and translate the 3d model
                model3d.Root.Transform = Matrix.CreateScale(0.05f)
                                        * Matrix.CreateFromAxisAngle(Vector3.Forward, rot.X)
                                        * Matrix.CreateFromAxisAngle(Vector3.Up, rot.Z)
                                        * Matrix.CreateTranslation(Vector3.Add( Vector3.Multiply(Vector3.Forward, 1.8f) ,
                                                                                Vector3.Multiply(Vector3.Down, -0.3f)));

                // and create transformation matrices for all 3d parts
                model3d.CopyAbsoluteBoneTransformsTo(boneTransforms);

                // draw
                foreach (ModelMesh mesh in model3d.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.World = boneTransforms[mesh.ParentBone.Index];
                        effect.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
                        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 2, this.device.Viewport.AspectRatio, 1, 5000);
                    }

                    mesh.Draw();
                }


            }

        }


    }
}