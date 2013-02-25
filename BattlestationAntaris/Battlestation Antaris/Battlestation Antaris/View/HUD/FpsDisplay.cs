using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD
{
    class FpsDisplay : HUDTexture
    {

        private HUDString text;

        private HUDString fps;

        private int elapsedTime = 0;

        private int frameCounter = 0;


        public FpsDisplay(Vector2 position, ContentManager content) : base(content)
        {
            this.Color = new Color(32, 32, 32, 160);
            this.Width = 80;
            this.Height = 25;
            this.Position = position;

            this.text = new HUDString("FPS : ", content);
            this.text.Position = new Vector2(position.X - 15, position.Y);
            this.text.Scale = 0.4f;
            this.text.layerDepth = 0.4f;

            this.fps = new HUDString(""+frameCounter, content);
            this.fps.Position = new Vector2(position.X + 25, position.Y);
            this.fps.Scale = 0.4f;
            this.fps.layerDepth = 0.4f;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            this.text.Draw(spriteBatch);
            this.fps.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            this.frameCounter++;

            if (this.elapsedTime > 1000)
            {
                fps.String = "" + frameCounter;
                this.frameCounter = 0;
                this.elapsedTime = 0;
            }
        }

    }
}
