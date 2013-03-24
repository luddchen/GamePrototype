using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD {

    public enum HUDType {
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
    public abstract class HUD_Item {

        public HUDType positionType;
        public Vector2 abstractPosition;
        protected Vector2 position;
        public Vector2 Position {
            get {
                return this.position;
            }
        }

        public HUDType sizeType;
        public Vector2 abstractSize;
        public Vector2 size;

        public float scale;

        public float rotation;

        public SpriteEffects effect;

        public Color color;

        protected float layerDepth = 0.5f;

        public float LayerDepth {
            get {
                return this.layerDepth;
            }
            set {
                this.layerDepth = value;
            }
        }


        protected bool isVisible;

        public bool IsVisible {
            set {
                this.isVisible = value;
            }
            get {
                if ( this.isVisible ) {
                    return ( this.parent != null ) ? this.parent.IsVisible : true;
                }

                return false;
            }
        }

        public HUD_Item parent;


        public HUD_Item() {
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
        public abstract void Draw( SpriteBatch spritBatch );


        /// <summary>
        /// check if a point intersects with this HUD element
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public abstract bool Intersects( Vector2 point );



        /// <summary>
        /// callback if the game window size change
        /// </summary>
        /// <param name="offset"></param>
        public virtual void ClientSizeChanged() {
            if ( this.parent != null ) {
                this.position = this.parent.position;
            } else {
                this.position = Vector2.Zero;
            }

            switch ( this.positionType ) {
                case HUDType.RELATIV:
                    this.position += Vector2.Multiply( this.abstractPosition, Antares.RenderSize ); // component wise mul
                    break;

                case HUDType.ABSOLUT:
                    this.position += new Vector2( this.abstractPosition.X, this.abstractPosition.Y );
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.position += new Vector2( this.abstractPosition.X, Antares.RenderSize.Y * this.abstractPosition.Y );
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.position += new Vector2( Antares.RenderSize.X * this.abstractPosition.X, this.abstractPosition.Y );
                    break;
            }
        }


        public void ToggleVisibility() {
            this.isVisible = !this.isVisible;
        }


        public override string ToString() {
            return "HUD2DItem : pos = " + this.position + " , size = " + this.size + " , scale = " + this.scale + " , layer = " + this.layerDepth;
        }

    }

}
