using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;
using Battlestation_Antaris.View.HUD.CockpitHUD;
using Battlestation_Antaris.Tools;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the cockpit view
    /// </summary>
    class CockpitView : View
    {
        /// <summary>
        /// the game view camera
        /// </summary>
        protected Camera camera;

        /// <summary>
        /// the cockpit compass
        /// </summary>
        Compass3d compass;


        TargetInfo targetInfo;

        HUD2DTexture cockpitTexture;


        /// <summary>
        /// a list of background images
        /// </summary>
        List<BackgroundObject> backgroundObjects;

        Skybox skybox;

        Grid grid;

        Microsoft.Xna.Framework.Graphics.Model targetCrossModel;
        Matrix[] targetCrossBoneTransforms;


        /// <summary>
        /// creates a new cockpit view
        /// </summary>
        /// <param name="game">the game</param>
        public CockpitView(Game1 game)
            : base(game)
        {
            this.camera = new Camera(this.game.GraphicsDevice);

            this.compass = new Compass3d(this.game.Content, this.game.GraphicsDevice);
            this.allHUD_3D.Add(this.compass);
            this.is3D = true;

            this.backgroundColor = Color.White;
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
            cockpitTexture = new HUD2DTexture(this.game);
            cockpitTexture.abstractPosition = new Vector2(0.5f, 0.5f);
            cockpitTexture.positionType = HUDType.RELATIV;
            cockpitTexture.abstractSize = new Vector2(1, 1);
            cockpitTexture.sizeType = HUDType.RELATIV;
            cockpitTexture.Texture = game.Content.Load<Texture2D>("Sprites//cockpit3");
            cockpitTexture.layerDepth = 1.0f;
            this.allHUD_2D.Add(cockpitTexture);

            this.allHUD_2D.Add(this.game.world.miniMap);

            this.targetInfo = new TargetInfo(new Vector2(60, 200), HUDType.ABSOLUT, new Vector2(150, 60), HUDType.ABSOLUT, this.game);
            this.allHUD_2D.Add(this.targetInfo);

            HUD2DTexture cross = new HUD2DTexture(this.game.Content.Load<Texture2D>("Sprites//Cross"), new Vector2(0.5f, 0.5f), new Vector2(48, 48), null, 1.0f, 0, this.game);
            cross.positionType = HUDType.RELATIV;
            cross.layerDepth = 0.8f;
            this.allHUD_2D.Add(cross);

            this.allHUD_2D.Add(new Velocity(new Vector2(0.3f, 0.925f), HUDType.RELATIV, new Vector2(15, 0.15f), HUDType.ABSOLUT_RELATIV, game));
            this.allHUD_2D.Add(new LaserHeat(new Vector2(0.35f, 0.925f), HUDType.RELATIV, new Vector2(15, 0.15f), HUDType.ABSOLUT_RELATIV, game));

            this.targetCrossModel = game.Content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models//TargetCross");
            this.targetCrossBoneTransforms = new Matrix[this.targetCrossModel.Bones.Count];

            // background
            for (int i = 0; i < 4; i++)
            {
                addBackgroundObject("Models//BGTest//test2");
            }

            for (int i = 0; i < 1; i++)
            {
                addBackgroundObject("Models//BGTest//test");
            }

            skybox = new Skybox("Models//Skysphere//skysphere", this.game);
            grid = new Grid("Models//Grid//grid", this.game);
        }

        private void addBackgroundObject(String model)
        {
            float yaw = (float)(RandomGen.random.NextDouble() * Math.PI * 2);
            float pitch = (float)(RandomGen.random.NextDouble() * Math.PI);
            float roll = (float)(RandomGen.random.NextDouble() * Math.PI * 2);
            Matrix bgRot = Tools.Tools.YawPitchRoll(Matrix.Identity, yaw, pitch, roll);
            int red = 128 + RandomGen.random.Next(127);
            int green = 128 + RandomGen.random.Next(127);
            int blue = 128 + RandomGen.random.Next(127);
            Color bgColor = new Color(red, green, blue);
            float scale = 1.25f + (float)RandomGen.random.NextDouble() * 1.5f;

            this.backgroundObjects.Add(new BackgroundObject(model, bgRot, scale, bgColor, game));
        }


        /// <summary>
        /// draw cockpit view content
        /// </summary>
        protected override void DrawPreContent()
        {
            this.cockpitTexture.color = this.backgroundColor;

            // init depth buffer
            this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true, DepthBufferWriteEnable = true };
            this.game.GraphicsDevice.BlendState = BlendState.AlphaBlend;

            // init camera
            this.camera.ClampTo(this.game.world.spaceShip);

            // init compass
            if (this.targetInfo.target != null)
            {
                this.compass.target = this.targetInfo.target.globalPosition;
            }
            else
            {
                this.compass.target = this.game.world.spaceStation.globalPosition;
            }

            
            this.skybox.Draw(this.camera);

            // draw background
            int nr = 1;
            foreach (BackgroundObject bg in this.backgroundObjects)
            {
                bg.Draw(this.camera, nr++);
            }

            Tools.Draw3D.Draw(this.game.world.allDrawable, this.camera);

            drawTargetCross();
            this.grid.Draw(this.camera);

        }

        private void drawTargetCross()
        {
            if (this.targetInfo.target != null)
            {
                Vector3 tRot = Tools.Tools.GetRotation(this.targetInfo.target.globalPosition - this.game.world.spaceShip.globalPosition, this.game.world.spaceShip.rotation);
                Matrix crossRot = Tools.Tools.YawPitchRoll(this.game.world.spaceShip.rotation, tRot.Z, tRot.X, tRot.Y);

                Tools.Draw3D.Draw(this.targetCrossModel, this.targetCrossBoneTransforms, 
                                    this.camera.view, this.camera.projection, 
                                    this.targetInfo.target.globalPosition, crossRot, new Vector3(this.targetInfo.target.bounding.Radius));
            }
        }

    }
}
