﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Model;
using Battlestation_Antares.View;
using System;
using Battlestation_Antaris.Model;
using System.Collections.ObjectModel;

namespace Battlestation_Antares.Tools {

    class Draw3D {

        public static void Draw( Microsoft.Xna.Framework.Graphics.Model model, Matrix[] boneTransforms,
                                Matrix view, Matrix projection ) {
            foreach ( ModelMesh mesh in model.Meshes ) {
                foreach ( BasicEffect effect in mesh.Effects ) {
                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;
                }

                mesh.Draw();
            }
        }


        public static void Draw( Microsoft.Xna.Framework.Graphics.Model model, Matrix[] boneTransforms,
                                Matrix view, Matrix projection, Matrix world ) {
            model.Root.Transform = world;
            model.CopyAbsoluteBoneTransformsTo( boneTransforms );

            Draw( model, boneTransforms, view, projection );
        }


        public static void Draw( Microsoft.Xna.Framework.Graphics.Model model, Matrix[] boneTransforms,
                                Matrix view, Matrix projection,
                                Vector3 translation, Matrix rotation, Vector3 scale ) {
            model.Root.Transform = Matrix.CreateScale( scale ) * rotation * Matrix.CreateTranslation( translation );
            model.CopyAbsoluteBoneTransformsTo( boneTransforms );

            Draw( model, boneTransforms, view, projection );
        }



        public static void Draw( SpatialObject obj, Camera camera ) {
            obj.Draw(camera);
        }


        public static void Draw( ReadOnlyCollection<SpatialObject> allObjects, Camera camera ) {
            foreach ( SpatialObject obj in allObjects ) {
                Draw( obj, camera );
            }
        }


        public static void DefaultLighting( BasicEffect effect ) {
            effect.EnableDefaultLighting();
        }


        public static void Lighting1( BasicEffect effect ) {
            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3( 0.8f, 0.75f, 0.6f );
            effect.DirectionalLight0.Direction = new Vector3( 1, 1, -1 );
            effect.DirectionalLight0.SpecularColor = new Vector3( 1, 1, 0.9f );
            effect.AmbientLightColor = new Vector3( 0.4f, 0.4f, 0.4f );
            effect.PreferPerPixelLighting = true;

            effect.FogEnabled = true;
            effect.FogColor = new Vector3( 0.15f, 0.125f, 0.12f );
            effect.FogStart = 100f;
            effect.FogEnd = 10000f;
        }


    }

}
