using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HUD.HUD {

    public class HUDContainer : HUD_Item {

        protected HUDTexture background;

        protected List<HUD_Item> allChilds;

        public override float LayerDepth {
            set {
                foreach ( HUD_Item item in this.allChilds ) {
                    item.LayerDepth = value - 0.01f;
                }
                this.background.LayerDepth = value;
                base.LayerDepth = value;
            }
        }

        public override HUDType SizeType {
            set {
                this.background.SizeType = value;
                base.SizeType = value;
            }
        }

        public override Vector2 AbstractSize {
            set {
                this.background.AbstractSize = value;
                base.AbstractSize = value;
            }
        }


        public HUDContainer( Vector2 position) {
            this.allChilds = new List<HUD_Item>();
            this.background = new HUDTexture();
            this.background.IsVisible = false;
            this.background.parent = this;
            this.AbstractPosition = position;
        }


        public HUDContainer( Vector2 position, Vector2 size ) : this(position) {
            this.AbstractSize = size;
        }


        public virtual void Add( HUD_Item element ) {
            element.parent = this;
            element.LayerDepth = this.LayerDepth - 0.01f;
            element.RenderSizeChanged();
            this.allChilds.Add( element );
        }


        public virtual void Remove( HUD_Item element ) {
            this.allChilds.Remove( element );
        }


        public void Clear() {
            this.allChilds.Clear();
        }


        public void SetBackground( Object texture, Color color ) {
            if ( texture != null && color != null ) {
                SetBackground( texture );
                SetBackground( color );
            } else if ( texture != null ) {
                SetBackground( texture );
            } else if ( color != null ) {
                SetBackground( color );
            } else {
                SetBackground( null );
            }
        }

        public void SetBackground( Object property ) {
            bool visibility = true;

            if ( property == null ) {
                visibility = false;
            } else {
                if ( property is Texture2D ) {
                    this.background.Texture = (Texture2D)property;
                } else if ( property is String ) {
                    this.background.Texture = HUD_Item.game.Content.Load<Texture2D>( (String)property );
                } else if ( property is Color ) {
                    this.background.color = (Color)property;
                } else {
                    visibility = false;
                }
            }

            this.background.IsVisible = visibility;
        }


        public override void Draw( SpriteBatch spriteBatch ) {
            if ( this.isVisible ) {
                this.background.Draw( spriteBatch );
                foreach ( HUD_Item item in this.allChilds ) {
                    item.Draw( spriteBatch );
                }
            }
        }


        public override void RenderSizeChanged() {
            base.RenderSizeChanged();

            if ( this.background != null ) {
                this.background.RenderSizeChanged();
            }

            if ( this.allChilds != null ) {
                foreach ( HUD_Item item in this.allChilds ) {
                    item.RenderSizeChanged();
                }
            }
        }

    }

}
