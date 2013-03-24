using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD {

    public class HUDContainer : HUD_Item {

        protected List<HUD_Item> allChilds;

        public new float LayerDepth {
            set {
                base.LayerDepth = value;
                foreach ( HUD_Item item in this.allChilds ) {
                    item.LayerDepth = this.layerDepth - 0.01f;
                }
            }
            get {
                return base.LayerDepth;
            }
        }


        public HUDContainer( Vector2 abstractPosition, HUDType positionType) {
            this.allChilds = new List<HUD_Item>();
            this.AbstractPosition = abstractPosition;
            this.positionType = positionType;
        }


        public virtual void Add( HUD_Item element ) {
            element.parent = this;
            element.LayerDepth = this.layerDepth - 0.01f;
            this.allChilds.Add( element );
            element.ClientSizeChanged();
        }


        public virtual void Remove( HUD_Item element ) {
            this.allChilds.Remove( element );
        }


        public void Clear() {
            this.allChilds.Clear();
        }


        public override void Draw( SpriteBatch spritBatch ) {
            if ( this.isVisible ) {
                foreach ( HUD_Item item in this.allChilds ) {
                    item.Draw( spritBatch );
                }
            }
        }


        public override void ClientSizeChanged() {
            base.ClientSizeChanged();

            foreach ( HUD_Item item in this.allChilds ) {
                item.ClientSizeChanged();
            }
        }


        //public override bool Intersects( Vector2 point ) {
        //    bool intersects = false;
        //    foreach ( HUD_Item item in this.allChilds ) {
        //        if ( item.Intersects( point ) ) {
        //            intersects = true;
        //        }
        //    }
        //    return intersects;
        //}


    }

}
