using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// an image that can be drawn in background with respect to its spherical coordinates and a local (viewer) rotation
    /// </summary>
    public class BackgroundImage
    {

        /// <summary>
        /// points to the spherical position
        /// </summary>
        private Matrix rotation;


        /// <summary>
        /// the image texture
        /// </summary>
        private Texture2D texture;


        /// <summary>
        /// the texture origin
        /// </summary>
        private Vector2 Origin;


        /// <summary>
        /// the multiply color for drawing 
        /// </summary>
        private Color color;


        /// <summary>
        /// the output width
        /// </summary>
        private float width;


        /// <summary>
        /// the output height
        /// </summary>
        private float height;


        /// <summary>
        /// the game
        /// </summary>
        private Game1 game;


        /// <summary>
        /// creates a new background image
        /// </summary>
        /// <param name="game"></param>
        public BackgroundImage(Game1 game)
        {
            this.game = game;
            this.color = Color.White;
            this.texture = this.game.Content.Load<Texture2D>("Sprites//Circle");
            this.width = 10;
            this.height = 10;
            this.rotation = Matrix.Identity;

            this.Origin = new Vector2(this.texture.Width / 2, this.texture.Height / 2);
        }


        /// <summary>
        /// creates a new background image
        /// </summary>
        /// <param name="texture">image texture, can be null</param>
        /// <param name="width">width, can be null</param>
        /// <param name="height">height, can be null</param>
        /// <param name="rotation">rotation matrix -- vielleicht hier besser einzelne Werte für rotation</param>
        /// <param name="color">multiply color, can be null</param>
        /// <param name="game">the game</param>
        public BackgroundImage(Texture2D texture, float? width, float? height, Matrix rotation, Color? color, Game1 game)
        {
            this.game = game;
            if (texture == null) { this.texture = this.game.Content.Load<Texture2D>("Sprites//Square"); }
            if (texture != null) { this.texture = texture; }

            this.width = width ?? 10;
            this.height = height ?? 10;
            this.color = color ?? Color.Beige;
            this.rotation = rotation;

            this.Origin = new Vector2(this.texture.Width / 2, this.texture.Height / 2);
        }

        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(SpriteBatch spriteBatch, SpatialObject obj, Camera camera)
        {

            Vector3 rot = Tools.Tools.GetYawPitchRoll(this.rotation, obj.rotation);

            // todo : use camera projection to determine position multiplicator + draw only if on viewport
            Rectangle dest = new Rectangle(
                    (int)(this.game.GraphicsDevice.Viewport.Width / 2 - (rot.Z - Math.PI / 2) * this.game.GraphicsDevice.Viewport.Width * 1.54f),
                    (int)(this.game.GraphicsDevice.Viewport.Height / 2 + (rot.X - Math.PI / 2) * this.game.GraphicsDevice.Viewport.Height * 2.4f),
                    (int)(this.width),
                    (int)(this.height));

            spriteBatch.Draw(this.texture,
                            dest,
                            null,
                            this.color,
                            -rot.Y,
                            this.Origin,
                            SpriteEffects.None,
                            0.0f);
        }

    }

}
