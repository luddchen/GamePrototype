using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD {

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


        public HUDContainer( Vector2 abstractPosition, HUDType positionType) {
            this.allChilds = new List<HUD_Item>();
            this.background = new HUDTexture();
            this.background.IsVisible = false;
            this.background.parent = this;
            this.PositionType = positionType;
            this.AbstractPosition = abstractPosition;
        }


        public virtual void Add( HUD_Item element ) {
            element.parent = this;
            element.LayerDepth = this.LayerDepth - 0.01f;
            element.ClientSizeChanged();
            this.allChilds.Add( element );
        }


        public virtual void Remove( HUD_Item element ) {
            this.allChilds.Remove( element );
        }


        public void Clear() {
            this.allChilds.Clear();
        }


        public void SetBackground( Texture2D texture ) {
            if ( texture == null ) {
                this.background.IsVisible = false;
            } else {
                this.background.IsVisible = true;
                this.background.Texture = texture;
            }
        }

        public void SetBackground( String textureName ) {
            SetBackground( ( textureName == null ) ? null : Antares.content.Load<Texture2D>( textureName ) );
        }

        public void SetBackgroundColor( Color color ) {
            this.background.IsVisible = true;
            this.background.color = color;
        }


        public override void Draw( SpriteBatch spriteBatch ) {
            if ( this.isVisible ) {
                this.background.Draw( spriteBatch );
                foreach ( HUD_Item item in this.allChilds ) {
                    item.Draw( spriteBatch );
                }
            }
        }


        public override void ClientSizeChanged() {
            base.ClientSizeChanged();

            if ( this.background != null ) {
                this.background.ClientSizeChanged();
            }

            if ( this.allChilds != null ) {
                foreach ( HUD_Item item in this.allChilds ) {
                    item.ClientSizeChanged();
                }
            }
        }

    }

}
