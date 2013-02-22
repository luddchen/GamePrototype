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
        List<BackgroundImage> backgroundImages;


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
            this.backgroundImages = new List<BackgroundImage>();
        }


        /// <summary>
        /// initialize cockpit view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // 3D HUD
            this.compass.Initialize(this.game.world.spaceShip);


            // 2D HUD
            HUDString testString = new HUDString("Antaris Cockpit : W/S - Engine , A/D - Roll", null, null, null, new Color(32,32,32,64), 0.45f, 0.0f, game.Content);
            testString.Position = new Vector2(game.GraphicsDevice.Viewport.Width / 2, 15);

            this.allHUD_2D.Add(testString);

            this.allHUD_2D.Add(new ShipAttributesVisualizer(this.game.world.spaceShip, this.game));

            Random random = new Random();

            // background
            Texture2D galaxy = this.game.Content.Load<Texture2D>("Sprites//Galaxy");
            Texture2D erde = this.game.Content.Load<Texture2D>("Sprites//Erde2");
            for (int i = 0; i < 80; i++)
            {
                float width = 200 + random.Next(600);
                float height = 200 + random.Next(600);
                float yaw = (float)(random.NextDouble() * Math.PI * 2);
                float pitch = (float)(random.NextDouble() * Math.PI);
                float roll = (float)(random.NextDouble() * Math.PI * 2);
                int red = 128 + random.Next(127);
                int green = 128 + random.Next(127);
                int blue = 128 + random.Next(127);
                int alpha = 32 + random.Next(32);
                Matrix bgRot = Tools.Tools.YawPitchRoll(Matrix.Identity, yaw, pitch, roll);
                Color bgColor = new Color(red, green, blue, alpha);

                this.backgroundImages.Add( new BackgroundImage(galaxy, width, height, bgRot, bgColor, game));
            }

            for (int i = 0; i < 5; i++)
            {
                float width = 400;
                float height = 400;
                float yaw = (float)(random.NextDouble() * Math.PI * 2);
                float pitch = (float)(random.NextDouble() * Math.PI);
                float roll = (float)(random.NextDouble() * Math.PI * 2);
                int red = 128 + random.Next(127);
                int green = 128 + random.Next(127);
                int blue = 128 + random.Next(127);
                int alpha = 32 + random.Next(32);
                Matrix bgRot = Tools.Tools.YawPitchRoll(Matrix.Identity, yaw, pitch, roll);
                Color bgColor = new Color(red, green, blue);

                this.backgroundImages.Add(new BackgroundImage(erde, width, height, bgRot, bgColor, game));
            }

        }


        /// <summary>
        /// draw cockpit view content
        /// </summary>
        protected override void DrawContent()
        {

            // draw background
            this.game.spriteBatch.Begin();

                foreach (BackgroundImage bg in this.backgroundImages)
                {
                    bg.Draw(this.game.spriteBatch, this.game.world.spaceShip, this.camera);
                }

            this.game.spriteBatch.End();


            // init depth buffer
            this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

            // init camera
            this.camera.ClampTo(this.game.world.spaceShip);

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
