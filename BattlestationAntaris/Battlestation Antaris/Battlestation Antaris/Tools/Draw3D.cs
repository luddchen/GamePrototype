using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.Model;
using Battlestation_Antaris.View;

namespace Battlestation_Antaris.Tools
{

    public class Draw3D
    {

        public static void Draw(Microsoft.Xna.Framework.Graphics.Model model, Matrix[] boneTransforms, 
                                Matrix view, Matrix projection)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    lighting(effect);

                    effect.World = boneTransforms[mesh.ParentBone.Index];
                    effect.View = view;
                    effect.Projection = projection;
                }
                mesh.Draw();
            }
        }


        public static void Draw(Microsoft.Xna.Framework.Graphics.Model model, Matrix[] boneTransforms,
                                Matrix view, Matrix projection, Matrix world)
        {
            model.Root.Transform = world;
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            Draw(model, boneTransforms, view, projection);
        }


        public static void Draw(Microsoft.Xna.Framework.Graphics.Model model, Matrix[] boneTransforms, 
                                Matrix view, Matrix projection,
                                Vector3 translation, Matrix rotation, Vector3 scale)
        {
            model.Root.Transform = Matrix.CreateScale(scale) * rotation * Matrix.CreateTranslation(translation);
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);

            Draw(model, boneTransforms, view, projection);
        }



        public static void Draw(SpatialObject obj, Camera camera)
        {
            if (obj.isVisible)
            {
                Draw(obj.model3d, obj.boneTransforms, camera.view, camera.projection, obj.globalPosition, obj.rotation, obj.scale);
            }
        }


        public static void Draw(List<SpatialObject> allObjects, Camera camera)
        {
            foreach (SpatialObject obj in allObjects)
            {
                Draw(obj, camera);
            }
        }


        public delegate void Lighting(BasicEffect effect);


        public static Lighting lighting = DefaultLighting;


        private static void DefaultLighting(BasicEffect effect)
        {
            effect.EnableDefaultLighting();
        }


        private static void Lighting1(BasicEffect effect)
        {
            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 0.5f);
            effect.DirectionalLight0.Direction = new Vector3(1, 1, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
            effect.AmbientLightColor = new Vector3(0.5f, 0.5f, 0.5f);
            //effect.EmissiveColor = new Vector3(0, 0, 0.1f);
            //effect.Alpha = 0.66f;

            //effect.FogEnabled = true;
            //effect.FogColor = Color.Red.ToVector3();
            //effect.FogStart = 200.0f;
            //effect.FogEnd = 210.0f;
        }

    }

}
