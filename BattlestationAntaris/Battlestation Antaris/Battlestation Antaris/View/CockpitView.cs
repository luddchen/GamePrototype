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

        public CockpitView(Controller controller)
            : base(controller)
        {
            this.camera = new Camera(this.controller.game.GraphicsDevice);
        }

        public override void Draw()
        {
            base.Draw();

            this.controller.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            this.controller.game.GraphicsDevice.Clear(Color.Black);

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
                            effect.EnableDefaultLighting();

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
