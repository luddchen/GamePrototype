using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// abstract basis class for views
    /// </summary>
    public abstract class View
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
        public List<HUD2D> allHUD_2D;


        /// <summary>
        /// a list of 3D HUD elements
        /// </summary>
        public List<HUD3D> allHUD_3D;


        /// <summary>
        /// create a new view, 3D disabled
        /// </summary>
        /// <param name="game">the game</param>
        public View(Game1 game)
        {
            this.game = game;
            this.allHUD_2D = new List<HUD2D>();
            this.allHUD_3D = new List<HUD3D>();
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

            // draw 2D HUD elements
            this.game.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);//, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, null);

            foreach (HUD2D element in this.allHUD_2D)
            {
                element.Draw(this.game.spriteBatch);
            }

            this.game.spriteBatch.End();


            // draw 3D HUD elements if 3D is enabled
            if (this.is3D)
            {
                this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

                foreach (HUD3D element in this.allHUD_3D)
                {
                    element.Draw();
                }
            }

        }


        /// <summary>
        /// draw the view content
        /// </summary>
        abstract protected void DrawContent();


        public virtual void Window_ClientSizeChanged()
        {
            foreach (HUD2D element in this.allHUD_2D)
            {
                element.ClientSizeChanged(Vector2.Zero);
            }
        }

    }

}
