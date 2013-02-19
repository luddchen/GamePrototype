using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// abstract basis class for views
    /// </summary>
    abstract class View
    {

        /// <summary>
        /// if the view contains 3D elements
        /// </summary>
        public bool is3D;


        /// <summary>
        /// background color of this view
        /// </summary>
        public Color backgroundColor;


        /// <summary>
        /// the game
        /// </summary>
        public Game1 game;


        /// <summary>
        /// a list of 2D HUD elements
        /// </summary>
        public List<HUDElement2D> allHUD_2D;


        /// <summary>
        /// a list of 3D HUD elements
        /// </summary>
        public List<HUDElement3D> allHUD_3D;


        /// <summary>
        /// create a new view, 3D disabled
        /// </summary>
        /// <param name="game">the game</param>
        public View(Game1 game)
        {
            this.game = game;
            this.allHUD_2D = new List<HUDElement2D>();
            this.allHUD_3D = new List<HUDElement3D>();
            this.is3D = false;
            this.backgroundColor = Color.Black;
        }


        /// <summary>
        /// initialize content
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// draw content
        /// HUD 
        /// </summary>
        public void Draw()
        {
            this.game.GraphicsDevice.Clear(this.backgroundColor);

            // draw content
            DrawContent();


            // draw 3D HUD elements if 3D is enabled
            if (this.is3D)
            {
                this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

                foreach (HUDElement3D element in this.allHUD_3D)
                {
                    element.Draw();
                }
            }


            // draw 2D HUD elements
            this.game.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (HUDElement2D element in this.allHUD_2D)
            {
                element.Draw(this.game.spriteBatch);
            }

            this.game.spriteBatch.End();

        }


        /// <summary>
        /// draw the view content
        /// </summary>
        abstract protected void DrawContent();

    }

}
