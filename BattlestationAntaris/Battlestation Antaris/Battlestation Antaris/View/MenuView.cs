﻿using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD.AIComposer;
using System;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the menu view
    /// </summary>
    class MenuView : View
    {

        private AI_Container aiContainer;

        /// <summary>
        /// creates a new menu view
        /// </summary>
        /// <param name="game">the game</param>
        public MenuView(Game1 game)
            : base(game)
        {
        }
        
                /// <summary>
        /// draw the view content
        /// </summary>
        protected override void DrawPreContent()
        {
            // dirty update
            this.aiContainer.Update();
        }

        /// <summary>
        /// draw the view post content
        /// </summary>
        protected override void DrawPostContent()
        {
            this.aiContainer.DrawConnections();
        }


        /// <summary>
        /// initialize menu view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // test content
            HUD2DTexture testTex = new HUD2DTexture(this.game);
            testTex.abstractPosition = new Vector2(0.5f, 0.4f);
            testTex.positionType = HUDType.RELATIV;
            testTex.abstractSize = new Vector2(1f, 1f);
            testTex.sizeType = HUDType.RELATIV;
            testTex.Texture = game.Content.Load<Texture2D>("Sprites//battlestation");
            testTex.layerDepth = 1.0f;

            this.allHUD_2D.Add(testTex);

            HUD2DString testString = new HUD2DString("Antaris Menu", this.game);
            testString.abstractPosition = new Vector2(0.5f, 0.1f);
            testString.positionType = HUDType.RELATIV;

            this.allHUD_2D.Add(testString);

            this.aiContainer = new AI_Container(this.game);
            this.allHUD_2D.Add(this.aiContainer);
        }

    }

}
