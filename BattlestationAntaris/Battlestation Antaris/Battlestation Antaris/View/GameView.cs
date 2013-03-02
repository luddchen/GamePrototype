using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antaris.View
{
    abstract class GameView : View
    {
        /// <summary>
        /// the game view camera
        /// </summary>
        protected Camera camera;

        /// <summary>
        /// ambient light color for testing
        /// </summary>
        Vector3 ambientColor;

        public GameView(Game1 game) : base(game)
        {
            this.camera = new Camera(this.game.GraphicsDevice);
            this.ambientColor = new Vector3(0.5f, 0.5f, 0.5f);
        }

        protected override void DrawContent()
        {
            drawWorldObjects();
            drawWorldWeapons();
        }

        protected void drawWorldObjects() {
            SpatialObject shield = this.game.world.Shield;

            // draw world objects
            foreach (SpatialObject obj in this.game.world.allObjects)
            {
                if (obj.isVisible)
                {
                    obj.model3d.Root.Transform = obj.rotation * Matrix.CreateTranslation(obj.globalPosition);
                    obj.model3d.CopyAbsoluteBoneTransformsTo(obj.boneTransforms);

                    foreach (ModelMesh mesh in obj.model3d.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            setLightning(effect);

                            effect.World = obj.boneTransforms[mesh.ParentBone.Index];
                            effect.View = this.camera.view;
                            effect.Projection = this.camera.projection;
                        }
                        mesh.Draw();
                    }
                }

                
                //// draw shield -> testing
                //if (obj is SpaceStation || obj is Turret || obj is Radar)
                //{
                //    shield.model3d.Root.Transform = obj.rotation * Matrix.CreateScale(obj.bounding.Radius) 
                //                                    * Matrix.CreateTranslation(obj.globalPosition + obj.bounding.Center);
                //    shield.model3d.CopyAbsoluteBoneTransformsTo(shield.boneTransforms);

                //    foreach (ModelMesh mesh in shield.model3d.Meshes)
                //    {
                //        foreach (BasicEffect effect in mesh.Effects)
                //        {
                //            setLightning(effect);

                //            effect.World = shield.boneTransforms[mesh.ParentBone.Index];
                //            effect.View = this.camera.view;
                //            effect.Projection = this.camera.projection;
                //        }
                //        mesh.Draw();
                //    }
                //}
            }
        }

        private void setLightning(BasicEffect effect)
        {
            //effect.EnableDefaultLighting();

            effect.LightingEnabled = true;
            effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 0.5f);
            effect.DirectionalLight0.Direction = new Vector3(1, 1, -1);
            effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
            effect.AmbientLightColor = this.ambientColor;
            //effect.EmissiveColor = new Vector3(0, 0, 0.1f);
            //effect.Alpha = 0.66f;

            //effect.FogEnabled = true;
            //effect.FogColor = Color.Red.ToVector3();
            //effect.FogStart = 200.0f;
            //effect.FogEnd = 210.0f;
        }

        protected void drawWorldWeapons()
        {
            foreach (SpatialObject obj in this.game.world.allWeapons)
            {
                if (obj.isVisible)
                {
                    obj.model3d.Root.Transform = obj.rotation * Matrix.CreateTranslation(obj.globalPosition);
                    obj.model3d.CopyAbsoluteBoneTransformsTo(obj.boneTransforms);

                    foreach (ModelMesh mesh in obj.model3d.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            setLightning(effect);

                            effect.World = obj.boneTransforms[mesh.ParentBone.Index];
                            effect.View = this.camera.view;
                            effect.Projection = this.camera.projection;
                        }
                        mesh.Draw();
                    }
                }
            }
        }
    }

}
