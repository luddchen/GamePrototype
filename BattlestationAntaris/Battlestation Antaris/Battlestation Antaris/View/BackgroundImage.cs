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

    public class BackgroundImage
    {

        // points to the spherical position
        Matrix rotation;

        Texture2D texture;

        Vector2 Origin;

        Color color;

        float width;

        float height;

        Game1 game;


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

            double forward = Vector3.Dot(this.rotation.Forward, obj.rotation.Forward);
            double right = Vector3.Dot(this.rotation.Forward, obj.rotation.Right);
            double up = Vector3.Dot(this.rotation.Forward, obj.rotation.Up);

            double rotZ = Math.Atan2(forward, right);

            double planeDist = Math.Sqrt(forward * forward + right * right);

            double rotX = Math.Atan2(planeDist, up);

            // todo : insert roll + use camera projection to determine position multiplicator
            Rectangle dest = new Rectangle(
                    (int)(this.game.GraphicsDevice.Viewport.Width / 2 - (rotZ - Math.PI/2) * this.game.GraphicsDevice.Viewport.Width * 1.54f),
                    (int)(this.game.GraphicsDevice.Viewport.Height / 2 + (rotX - Math.PI/2) * this.game.GraphicsDevice.Viewport.Height * 2.4f),
                    (int)(this.width),
                    (int)(this.height));
            spriteBatch.Draw(this.texture,
                            dest,
                            null,
                            this.color,
                            0,
                            this.Origin,
                            SpriteEffects.None,
                            0.0f);
        }

    }

}
