using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HUD.HUD {

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

        public static IHUDGame game;

        public static IInputProvider inputProvider;

        # region position elements 

            private HUDType positionType = HUDType.RELATIV;

            public virtual HUDType PositionType {
                get {
                    return this.positionType;
                }
                set {
                    this.positionType = value;
                    RenderSizeChanged();
                }
            }

            private Vector2 abstractPosition = Vector2.Zero;

            public virtual Vector2 AbstractPosition {
                get {
                    return this.abstractPosition;
                }
                set {
                    this.abstractPosition = value;
                    RenderSizeChanged();
                }
            }

            private Vector2 position = Vector2.Zero;

            public virtual Vector2 Position {
                get {
                    return this.position;
                }
            }

        # endregion


        # region size elements

            private HUDType sizeType = HUDType.RELATIV;

            public virtual HUDType SizeType {
                get {
                    return this.sizeType;
                }
                set {
                    this.sizeType = value;
                    RenderSizeChanged();
                }
            }

            private Vector2 abstractSize = Vector2.Zero;

            public virtual Vector2 AbstractSize {
                get {
                    return this.abstractSize;
                }
                set {
                    this.abstractSize = value;
                    RenderSizeChanged();
                }
            }

            private Vector2 size = Vector2.Zero;

            public virtual Vector2 Size {
                get {
                    return this.size * this.scale;
                }
            }

            private float abstractScale = 1.0f;

            public virtual float AbstractScale {
                get {
                    return this.abstractScale;
                }
                set {
                    this.abstractScale = value;
                    RenderSizeChanged();
                }
            }

            private float scale = 1.0f;

            public virtual float Scale {
                get {
                    return this.scale;
                }
            }

        # endregion


        #region layer, visibility, intersection and relation elements

            private float layerDepth = 0.5f;

            public virtual float LayerDepth {
                get {
                    return this.layerDepth;
                }
                set {
                    this.layerDepth = value;
                }
            }


            /// <summary>
            /// local visibility
            /// </summary>
            protected bool isVisible = true;

            /// <summary>
            /// global visibility
            /// </summary>
            public virtual bool IsVisible {
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


            protected Rectangle dest = new Rectangle();

            public HUD_Item parent;

        #endregion


        #region other elements

            private float abstractRotation = 0.0f;

            public float AbstractRotation {
                get {
                    return this.abstractRotation;
                }
                set {
                    this.abstractRotation = value;
                    RenderSizeChanged();
                }
            }

            private float rotation = 0.0f;

            public float Rotation {
                get {
                    return this.rotation;
                }
            }

            public SpriteEffects effect = SpriteEffects.None;

            public Color color = Color.White;

        #endregion


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spritBatch">the spritebatch</param>
        public abstract void Draw( SpriteBatch spriteBatch );


        /// <summary>
        /// testing intersection with point
        /// </summary>
        /// <param name="point">the test point</param>
        /// <returns>true if there is an intersetion</returns>
        public virtual bool Intersects( Vector2 point ) {
            if ( this.Rotation == 0 ) {
                return ( Math.Abs( point.X - this.dest.X ) < ( this.dest.Width / 2 ) && Math.Abs( point.Y - this.dest.Y ) < ( this.dest.Height / 2 ) );
            } else {
                Vector2 rotatedPoint = Vector2.Transform( point, Matrix.CreateTranslation( new Vector3( -this.Position, 0 ) ) );
                rotatedPoint = Vector2.Transform( rotatedPoint, Matrix.CreateRotationZ( this.Rotation ) );
                rotatedPoint = Vector2.Transform( rotatedPoint, Matrix.CreateTranslation( new Vector3( this.Position, 0 ) ) );
                return ( Math.Abs( rotatedPoint.X - this.dest.X ) < ( this.dest.Width / 2 ) && Math.Abs( rotatedPoint.Y - this.dest.Y ) < ( this.dest.Height / 2 ) );
            }
        }

        /// <summary>
        /// callback if the game window size change
        /// </summary>
        /// <param name="offset"></param>
        public virtual void RenderSizeChanged() {

            _rotationChanged();
            _scaleChanged();

            // root position
            if ( this.parent != null ) {
                this.position = this.parent.position;
            } else {
                this.position = Vector2.Zero;
            }

            Vector2 rotatedAbstractPosition = this.AbstractPosition;

            if ( this.parent != null ) {
                //  without apect ratio
                //rotatedAbstractPosition = Vector2.Transform( this.AbstractPosition, Matrix.CreateRotationZ( -this.parent.Rotation ) );

                // with aspect ratio
                rotatedAbstractPosition.X = 
                    (float)
                    ( Math.Cos( -this.parent.Rotation ) * this.AbstractPosition.X 
                    - Math.Sin( -this.parent.Rotation ) * this.AbstractPosition.Y * HUD_Item.game.RenderSize().Y / HUD_Item.game.RenderSize().X);
                rotatedAbstractPosition.Y = 
                    (float)
                    ( Math.Sin( -this.parent.Rotation ) * this.AbstractPosition.X * HUD_Item.game.RenderSize().X / HUD_Item.game.RenderSize().Y 
                    + Math.Cos( -this.parent.Rotation ) * this.AbstractPosition.Y );
            }

            // calclate own position
            switch ( this.positionType ) {
                case HUDType.ABSOLUT:
                    this.position += rotatedAbstractPosition;
                    break;

                case HUDType.RELATIV:
                    this.position += Multiply( rotatedAbstractPosition, HUD_Item.game.RenderSize() );
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.position += new Vector2( rotatedAbstractPosition.X, HUD_Item.game.RenderSize().Y * rotatedAbstractPosition.Y );
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.position += new Vector2( HUD_Item.game.RenderSize().X * rotatedAbstractPosition.X, rotatedAbstractPosition.Y );
                    break;
            }

            // calculate own size
            switch ( this.sizeType ) {
                case HUDType.ABSOLUT:
                    this.size = this.abstractSize;
                    break;

                case HUDType.RELATIV:
                    this.size = Multiply( this.abstractSize, HUD_Item.game.RenderSize() );
                    break;

                case HUDType.ABSOLUT_RELATIV:
                    this.size.X = this.abstractSize.X;
                    this.size.Y = this.abstractSize.Y * HUD_Item.game.RenderSize().Y;
                    break;

                case HUDType.RELATIV_ABSOLUT:
                    this.size.X = this.abstractSize.X * HUD_Item.game.RenderSize().X;
                    this.size.Y = this.abstractSize.Y;
                    break;
            }

            // calculate intersection rectangle
            this.dest.X = (int)Position.X;
            this.dest.Y = (int)Position.Y;
            this.dest.Width = (int)Size.X;
            this.dest.Height = (int)Size.Y;

        }

        private void _rotationChanged() {
            this.rotation = this.abstractRotation;
            if ( this.parent != null ) {
                this.rotation += this.parent.Rotation;
            }
        }

        private void _scaleChanged() {
            this.scale = this.abstractScale;
            if ( this.parent != null ) {
                this.scale *= this.parent.Scale;
            }
        }


        /// <summary>
        /// toggle the visibility of this item, independent of parent visibility
        /// </summary>
        public void ToggleVisibility() {
            this.isVisible = !this.isVisible;
        }


        public override string ToString() {
            return this.GetType().Name;
        }


        // helper functions

        public static Vector2 Multiply( Vector2 abstractCoord, Point targetSize ) {
            return new Vector2( abstractCoord.X * targetSize.X, abstractCoord.Y * targetSize.Y );
        }

        public static Vector2 AbstractToConcrete( Vector2 abstractCoord ) {
            return Multiply( abstractCoord, HUD_Item.game.RenderSize() );
        }

    }

}
