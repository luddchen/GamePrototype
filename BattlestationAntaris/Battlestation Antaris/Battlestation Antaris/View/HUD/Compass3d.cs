﻿using System;
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

        public Vector3 target;

        private GraphicsDevice device;

        public Compass3d(ContentManager content, GraphicsDevice device)
        {
            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models/compass2");
            this.boneTransforms = new Matrix[model3d.Bones.Count];

            this.device = device;
            this.target = new Vector3();
        }

        public void Initialize(SpatialObject source)
        {
            this.source = source;
        }

        public override void Draw()
        {

            if (this.source != null)
            {

                Vector3 pointer = Vector3.Subtract(this.target, this.source.globalPosition);

                double forward = Vector3.Dot(pointer, this.source.rotation.Forward);
                double right = Vector3.Dot(pointer, this.source.rotation.Right);
                double up = Vector3.Dot(pointer, this.source.rotation.Up);

                double rotZ = Math.Atan2(forward, right);

                double planeDist = Math.Sqrt(forward * forward + right * right);

                double rotX = Math.Atan2(planeDist, up);

                model3d.Root.Transform = Matrix.CreateScale(0.1f)
                                        * Matrix.CreateFromAxisAngle(Vector3.Forward, (float)rotX)
                                        * Matrix.CreateFromAxisAngle(Vector3.Up, (float)rotZ)
                                        * Matrix.CreateTranslation(Vector3.Add( Vector3.Multiply(Vector3.Forward, 1.75f) ,
                                                                                Vector3.Multiply(Vector3.Down, 0.2f)));

                model3d.CopyAbsoluteBoneTransformsTo(boneTransforms);

                foreach (ModelMesh mesh in model3d.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.World = boneTransforms[mesh.ParentBone.Index];
                        effect.View = Matrix.CreateLookAt(Vector3.Zero, Vector3.Forward, Vector3.Up);
                        effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 2, this.device.Viewport.AspectRatio, 1, 5000); ;
                    }

                    mesh.Draw();
                }


            }

        }


    }
}


// ============= old code ==============

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Battlestation_Antaris.Model;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.Graphics;

//namespace Battlestation_Antaris.View.HUD
//{
//    public class Compass3d : HUDElement
//    {

//        public Microsoft.Xna.Framework.Graphics.Model model3d;

//        public Matrix[] boneTransforms;

//        private SpatialObject source;
//        private SpatialObject destination;

//        private GraphicsDevice device;

//        public Compass3d(ContentManager content, GraphicsDevice device)
//        {
//            this.model3d = content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models/compass");
//            this.boneTransforms = new Matrix[model3d.Bones.Count];

//            this.device = device;
//        }

//        public void Initialize(SpatialObject source, SpatialObject destination)
//        {
//            this.source = source;
//            this.destination = destination;
//        }

//        public override void Draw()
//        {
//            // protostate -- shows actually only space orientation of the ship
//            // ================================================================

//            //this.device.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

//            if (this.source != null && this.destination != null)
//            {

//                Vector3 direction = Vector3.Subtract(this.destination.globalPosition, this.source.globalPosition);
//                float distance = direction.Length();

//                if (distance > 0)
//                {
//                    Console.Out.WriteLine("*");

//                    direction.Normalize();

//                    model3d.Root.Transform = Matrix.CreateScale(0.1f)
//                                            * Matrix.CreateFromAxisAngle(Vector3.Right, (float)Math.PI / 2) 
//                                            * Matrix.CreateTranslation(Vector3.Add( Vector3.Multiply(this.source.rotation.Forward, 1.75f) ,
//                                                                                    Vector3.Multiply(this.source.rotation.Down, 0.2f)));

//                    model3d.CopyAbsoluteBoneTransformsTo(boneTransforms);

//                    foreach (ModelMesh mesh in model3d.Meshes)
//                    {
//                        foreach (BasicEffect effect in mesh.Effects)
//                        {
//                            effect.EnableDefaultLighting();

//                            effect.World = boneTransforms[mesh.ParentBone.Index];
//                            effect.View = Matrix.CreateLookAt(Vector3.Zero, this.source.rotation.Forward, this.source.rotation.Up); // check up vector ...
//                            effect.Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4 / 2, this.device.Viewport.AspectRatio, 1, 5000); ;
//                        }

//                        mesh.Draw();
//                    }
//                }
//            }

//        }


//    }
//}