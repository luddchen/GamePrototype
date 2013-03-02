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
    class CockpitView : GameView
    {

        /// <summary>
        /// the cockpit compass
        /// </summary>
        Compass3d compass;


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

            this.allHUD_2D.Add(new MiniMap2D(new Vector2(0.85f, 0.5f), HUDType.RELATIV, this.game));

            Random random = new Random();

            // background
            for (int i = 0; i < 120; i++)
            {
                float yaw = (float)(random.NextDouble() * Math.PI * 2);
                float pitch = (float)(random.NextDouble() * Math.PI);
                float roll = (float)(random.NextDouble() * Math.PI * 2);
                Matrix bgRot = Tools.Tools.YawPitchRoll(Matrix.Identity, yaw, pitch, roll);
                int red = 128 + random.Next(127);
                int green = 128 + random.Next(127);
                int blue = 128 + random.Next(127);
                Color bgColor = new Color(red, green, blue);
                float scale = 0.25f + (float)random.NextDouble() * 1.25f;

                this.backgroundObjects.Add(new BackgroundObject("Models//BGTest//test2", bgRot, scale, bgColor, game));
            }

            for (int i = 0; i < 40; i++)
            {
                float yaw = (float)(random.NextDouble() * Math.PI * 2);
                float pitch = (float)(random.NextDouble() * Math.PI);
                float roll = (float)(random.NextDouble() * Math.PI * 2);
                Matrix bgRot = Tools.Tools.YawPitchRoll(Matrix.Identity, yaw, pitch, roll);
                int red = 128 + random.Next(127);
                int green = 128 + random.Next(127);
                int blue = 128 + random.Next(127);
                Color bgColor = new Color(red, green, blue);
                float scale = 0.25f + (float)random.NextDouble() * 1.25f;

                this.backgroundObjects.Add(new BackgroundObject("Models//BGTest//test", bgRot, scale, bgColor, game));
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

            base.DrawContent();
        }
    }
}
