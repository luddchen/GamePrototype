using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD
{

    public enum HUDType
    {
        /// <summary>
        /// x and y absolut
        /// </summary>
        ABSOLUT,

        /// <summary>
        /// x and y relative
        /// </summary>
        RELATIV,

        /// <summary>
        /// x absolut, y relativ
        /// </summary>
        ABSOLUT_RELATIV,

        /// <summary>
        /// x relativ, y absolut
        /// </summary>
        RELATIV_ABSOLUT
    }


    /// <summary>
    /// abstract basis class for 2D HUD elements
    /// </summary>
    public abstract class HUD2D
    {
        protected Game1 game;

        public HUDType positionType;
        public Vector2 abstractPosition;
        protected Vector2 position;

        public HUDType sizeType;
        public Vector2 abstractSize;
        public Vector2 size;

        public float scale;

        public float rotation;

        public SpriteEffects effect;

        public Color color;


        public float layerDepth = 0.5f;

        public bool isVisible;


        public HUD2D(Game1 game)
        {
            this.game = game;

            this.positionType = HUDType.ABSOLUT;
            this.abstractPosition = Vector2.Zero;
            this.position = Vector2.Zero;

            this.sizeType = HUDType.ABSOLUT;
            this.abstractSize = Vector2.Zero;
            this.size = Vector2.Zero;

            this.isVisible = true;
            this.scale = 1.0f;
            this.rotation = 0.0f;
            this.effect = SpriteEffects.None;
            this.color = Color.White;
        }


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spritBatch">the spritebatch</param>
        public abstract void Draw(SpriteBatch spritBatch);


        /// <summary>
        /// check if a point intersects with this HUD element
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract bool Intersects(Vector2 point);



        /// <summary>
        /// callback if the game window size change
        /// </summary>
        /// <param name="offset"></param>
        public virtual void ClientSizeChanged(Vector2 offset)
        {
            this.position = offset;

            switch (this.positionType)
            {
                case HUDType.RELATIV:
                    this.position += new Vector2(this.game.GraphicsDevice.Viewport.Width * this.abstractPosition.X,
                                                 this.game.GraphicsDevice.Viewport.Height * this.abstractPosition.Y);
                    break;

                case HUDType.ABSOLUT:
                    this.position += new Vector2(this.abstractPosition.X, 
                                                 this.abstractPosition.Y);
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.position += new Vector2(this.abstractPosition.X,
                                                 this.game.GraphicsDevice.Viewport.Height * this.abstractPosition.Y);
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.position += new Vector2(this.game.GraphicsDevice.Viewport.Width * this.abstractPosition.X,
                                                 this.abstractPosition.Y);
                    break;
            }
        }


        public override string ToString()
        {
            return "HUD2DItem : pos = " + this.position + " , size = " + this.size + " , scale = " + this.scale + " , layer = " + this.layerDepth;
        }

    }

}
