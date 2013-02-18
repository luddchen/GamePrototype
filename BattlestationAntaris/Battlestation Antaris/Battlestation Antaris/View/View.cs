using System;
using System.Collections.Generic;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View
{

    abstract class View
    {

        public bool is3D;

        public Color backgroundColor;

        public Game1 game;

        public List<HUDElement2D> allHUD_2D;

        public List<HUDElement3D> allHUD_3D;

        public View(Game1 game)
        {
            this.game = game;
            this.allHUD_2D = new List<HUDElement2D>();
            this.allHUD_3D = new List<HUDElement3D>();
            this.is3D = false;
            this.backgroundColor = Color.Black;
        }

        public abstract void Initialize();

        public virtual void Draw()
        {
            this.game.GraphicsDevice.Clear(this.backgroundColor);


            // 2D HUD
            //DrawHUD2D();

            // 3D HUD
            //DrawHUD3D();

        }

        protected void DrawHUD2D()
        {
            this.game.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            foreach (HUDElement2D element in this.allHUD_2D)
            {
                element.Draw(this.game.spriteBatch);
            }

            this.game.spriteBatch.End();
        }

        protected void DrawHUD3D()
        {
            if (this.is3D)
            {
                this.game.GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

                //this.game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                foreach (HUDElement3D element in this.allHUD_3D)
                {
                    element.Draw();
                }
            }
        }

    }

}
