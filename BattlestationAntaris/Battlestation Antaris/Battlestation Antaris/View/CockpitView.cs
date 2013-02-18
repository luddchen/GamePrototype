using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.View
{

    class CockpitView : View
    {
        Camera camera;

        Compass3d compass;

        Vector3 ambientColor;

        public CockpitView(Game1 game)
            : base(game)
        {
            this.camera = new Camera(this.game.GraphicsDevice);
            this.ambientColor = new Vector3(0.5f, 0.5f, 0.5f);
            this.compass = new Compass3d(this.game.Content, this.game.GraphicsDevice);
            this.allHUD_3D.Add(this.compass);
            this.is3D = true;

            this.backgroundColor = Color.Purple;
        }

        public override void Initialize()
        {
            this.compass.Initialize(this.game.world.spaceShip);

            HUDString testString = new HUDString("Antaris Cockpit : W/S - Engine , A/D - Roll", null, null, null, new Color(100,100,100,100), 0.5f, 0.0f, game.Content);
            testString.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, 30);

            this.allHUD_2D.Add(testString);

            this.allHUD_2D.Add(new ShipAttributesVisualizer(this.game.world.spaceShip, this.game));
        }

        public override void Draw()
        {
            base.Draw();

            this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            this.camera.ClampTo(this.game.world.spaceShip);

            foreach (SpatialObject obj in this.game.world.allObjects)
            {
                if (obj.draw)
                {

                    obj.model3d.Root.Transform = obj.rotation * Matrix.CreateTranslation(obj.globalPosition);

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


            DrawHUD3D();

            DrawHUD2D();

        }

    }

}
