using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD
{
    public class Compass3d : HUDElement
    {

        public Microsoft.Xna.Framework.Graphics.Model model3d;

        public Matrix[] boneTransforms;

        private SpatialObject source;
        private SpatialObject destination;

        private GraphicsDevice device;

        public Compass3d(ContentManager content, GraphicsDevice device)
        {
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models/compass");
            this.boneTransforms = new Matrix[model3d.Bones.Count];

            this.device = device;
        }

        public void Initialize(SpatialObject source, SpatialObject destination)
        {
            this.source = source;
            this.destination = destination;
        }

        public override void Draw()
        {
            // protostate -- shows actually only space orientation of the ship
            // ================================================================

            //this.device.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            if (this.source != null && this.destination != null)
            {

                Vector3 direction = Vector3.Subtract(this.destination.globalPosition, this.source.globalPosition);
                float distance = direction.Length();

                if (distance > 0)
                {
                    Console.Out.WriteLine("*");

                    direction.Normalize();

                    model3d.Root.Transform = Matrix.CreateScale(0.1f)
                                            * Matrix.CreateFromAxisAngle(Vector3.Right, (float)Math.PI / 2) 
                                            * Matrix.CreateTranslation(Vector3.Add( Vector3.Multiply(this.source.rotation.Forward, 1.75f) ,
                                                                                    Vector3.Multiply(this.source.rotation.Down, 0.2f)));

                    model3d.CopyAbsoluteBoneTransformsTo(boneTransforms);

                    foreach (ModelMesh mesh in model3d.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();

                            effect.World = boneTransforms[mesh.ParentBone.Index];
                            effect.View = Matrix.CreateLookAt(Vector3.Zero, this.source.rotation.Forward, this.source.rotation.Up); // check up vector ...
                            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 2, this.device.Viewport.AspectRatio, 1, 5000); ;
                        }

                        mesh.Draw();
                    }
                }
            }

        }


    }
}
