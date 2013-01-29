using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View
{

    class CockpitView : View
    {
        Camera camera;

        Vector3 ambientColor;

        public CockpitView(Controller controller)
            : base(controller)
        {
            this.camera = new Camera(this.controller.game.GraphicsDevice);
            this.ambientColor = new Vector3(0.5f, 0.5f, 0.5f);
        }

        public override void Draw()
        {
            base.Draw();

            this.controller.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            this.controller.game.GraphicsDevice.Clear(Color.Khaki);

            this.camera.ClampTo(this.controller.spaceShip.ship);

            foreach (SpatialObject obj in this.controller.world.allObjects)
            {
                if (obj.draw)
                {

                    obj.model3d.Root.Transform = Matrix.CreateTranslation(obj.globalPosition);

                    obj.model3d.CopyAbsoluteBoneTransformsTo(obj.boneTransforms);

                    foreach (ModelMesh mesh in obj.model3d.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            //effect.EnableDefaultLighting();

                            effect.LightingEnabled = true;
                            effect.DirectionalLight0.DiffuseColor = new Vector3(1.0f, 1.0f, 0.5f);
                            effect.DirectionalLight0.Direction = new Vector3(1, 1, -1);
                            effect.DirectionalLight0.SpecularColor = new Vector3(1, 1, 1);
                            effect.AmbientLightColor = this.ambientColor;
                            //effect.EmissiveColor = new Vector3(0, 0, 0.1f);

                            //effect.FogEnabled = true;
                            //effect.FogColor = Color.Red.ToVector3();
                            //effect.FogStart = 200.0f;
                            //effect.FogEnd = 210.0f;

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
