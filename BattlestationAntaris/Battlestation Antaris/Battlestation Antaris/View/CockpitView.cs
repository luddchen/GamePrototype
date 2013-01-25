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
        Vector3 camPos = new Vector3(0, -30, 0);    // for testing

        public CockpitView(Controller controller)
            : base(controller)
        {
            this.camera = new Camera(this.controller.game.GraphicsDevice);
        }

        public override void Draw()
        {
            this.controller.game.GraphicsDevice.Clear(Color.Black);

            foreach (SpatialObject spatialObject in this.controller.world.allObjects)
            {
                spatialObject.model3d.Root.Transform = Matrix.CreateTranslation(spatialObject.globalPosition);

                spatialObject.model3d.CopyAbsoluteBoneTransformsTo(spatialObject.boneTransforms);

                foreach (ModelMesh mesh in spatialObject.model3d.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();

                        effect.World = spatialObject.boneTransforms[mesh.ParentBone.Index];
                        effect.View = this.camera.view;
                        effect.Projection = this.camera.projection;
                    }

                    mesh.Draw();
                }
            }

            // for testing
            this.camera.Update(camPos, new Vector3(0.2f, 1.0f, 0.1f), Vector3.UnitZ);
            camPos.Y += 0.5f;
            if (camPos.Y > 30) camPos.Y = -30;

        }

    }

}
