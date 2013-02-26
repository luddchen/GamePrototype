using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the cockpit view
    /// </summary>
    class CockpitView : View
    {

        /// <summary>
        /// the cockpit camera
        /// </summary>
        Camera camera;


        /// <summary>
        /// the cockpit compass
        /// </summary>
        Compass3d compass;


        /// <summary>
        /// ambient light color for testing
        /// </summary>
        Vector3 ambientColor;


        /// <summary>
        /// a list of background images
        /// </summary>
        List<BackgroundObject> backgroundObjects;


        // test ----------------------
        Microsoft.Xna.Framework.Graphics.Model bgModel;

        Matrix[] boneTransforms;

        Matrix rotation;


        /// <summary>
        /// creates a new cockpit view
        /// </summary>
        /// <param name="game">the game</param>
        public CockpitView(Game1 game)
            : base(game)
        {
            this.camera = new Camera(this.game.GraphicsDevice);
            this.ambientColor = new Vector3(0.5f, 0.5f, 0.5f);
            this.compass = new Compass3d(this.game.Content, this.game.GraphicsDevice);
            this.allHUD_3D.Add(this.compass);
            this.is3D = true;

            this.backgroundColor = Color.Black;
            this.backgroundObjects = new List<BackgroundObject>();
        }


        /// <summary>
        /// initialize cockpit view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // 3D HUD
            this.compass.Initialize(this.game.world.spaceShip);


            // 2D HUD
            HUD2DString testString = new HUD2DString("Antaris Cockpit : W/S - Engine , A/D - Roll", null, null, null, new Color(32, 32, 32, 64), 0.45f, 0.0f, this.game);
            testString.abstractPosition = new Vector2(0.5f, 0.05f);
            testString.positionType = HUDType.RELATIV;
            this.allHUD_2D.Add(testString);

            HUD2DTexture cokpitTexture = new HUD2DTexture(this.game);
            cokpitTexture.abstractPosition = new Vector2(0.5f, 0.5f);
            cokpitTexture.positionType = HUDType.RELATIV;
            cokpitTexture.abstractSize = new Vector2(1, 1);
            cokpitTexture.sizeType = HUDType.RELATIV;
            cokpitTexture.Texture = game.Content.Load<Texture2D>("Sprites//cockpit3");
            cokpitTexture.layerDepth = 1.0f;
            this.allHUD_2D.Add(cokpitTexture);

            this.allHUD_2D.Add(new ShipAttributesVisualizer(0.1f, 0.7f, this.game.world.spaceShip, this.game));

            Random random = new Random();

            // background
            for (int i = 0; i < 80; i++)
            {
                float yaw = (float)(random.NextDouble() * Math.PI * 2);
                float pitch = (float)(random.NextDouble() * Math.PI);
                float roll = (float)(random.NextDouble() * Math.PI * 2);
                Matrix bgRot = Tools.Tools.YawPitchRoll(Matrix.Identity, yaw, pitch, roll);
                int red = 128 + random.Next(127);
                int green = 128 + random.Next(127);
                int blue = 128 + random.Next(127);
                Color bgColor = new Color(red, green, blue);

                this.backgroundObjects.Add(new BackgroundObject("Models//BGTest//test2", bgRot, bgColor, game));
            }

            for (int i = 0; i < 5; i++)
            {
                float yaw = (float)(random.NextDouble() * Math.PI * 2);
                float pitch = (float)(random.NextDouble() * Math.PI);
                float roll = (float)(random.NextDouble() * Math.PI * 2);
                Matrix bgRot = Tools.Tools.YawPitchRoll(Matrix.Identity, yaw, pitch, roll);
                int red = 128 + random.Next(127);
                int green = 128 + random.Next(127);
                int blue = 128 + random.Next(127);
                Color bgColor = new Color(red, green, blue);

                this.backgroundObjects.Add(new BackgroundObject("Models//BGTest//test", bgRot, bgColor, game));
            }

        }


        /// <summary>
        /// draw cockpit view content
        /// </summary>
        protected override void DrawContent()
        {
            // init depth buffer
            this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true, DepthBufferWriteEnable = true };
            this.game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            // init camera
            this.camera.ClampTo(this.game.world.spaceShip);

            // draw background
            int nr = 1;
            foreach (BackgroundObject bg in this.backgroundObjects)
            {
                bg.Draw(this.camera, nr++);
            }

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

                
                // draw shield -> testing
                if (obj is SpaceStation || obj is Turret || obj is Radar)
                {
                    shield.model3d.Root.Transform = obj.rotation * Matrix.CreateScale(obj.bounding.Radius) 
                                                    * Matrix.CreateTranslation(obj.globalPosition + obj.bounding.Center);
                    shield.model3d.CopyAbsoluteBoneTransformsTo(shield.boneTransforms);

                    foreach (ModelMesh mesh in shield.model3d.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            setLightning(effect);

                            effect.World = shield.boneTransforms[mesh.ParentBone.Index];
                            effect.View = this.camera.view;
                            effect.Projection = this.camera.projection;
                        }
                        mesh.Draw();
                    }
                }
            }


            // draw world weapons
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

    }

}
