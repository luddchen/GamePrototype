using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD {

    public class HUD2DContainer : HUD2D {

        // list of all childs
        public List<HUD2D> allChilds;



        public HUD2DContainer( Vector2 abstractPosition, HUDType positionType, Antares game )
            : base( game ) {
            this.abstractPosition = abstractPosition;

            this.positionType = positionType;

            this.allChilds = new List<HUD2D>();
        }


        public virtual void Add( HUD2D element ) {
            element.parent = this;
            element.setLayerDepth( this.layerDepth - 0.01f );
            this.allChilds.Add( element );
            element.ClientSizeChanged();
        }


        public virtual void Remove( HUD2D element ) {
            this.allChilds.Remove( element );
        }


        public void Clear() {
            this.allChilds.Clear();
        }


        public override void setLayerDepth( float layerDepth ) {
            base.setLayerDepth( layerDepth );
            foreach ( HUD2D item in this.allChilds ) {
                item.setLayerDepth( this.layerDepth - 0.01f );
            }
        }


        public override void Draw( SpriteBatch spritBatch ) {
            if ( this.isVisible ) {
                foreach ( HUD2D item in this.allChilds ) {
                    item.Draw( spritBatch );
                }
            }
        }


        public override void ClientSizeChanged() {
            base.ClientSizeChanged();

            foreach ( HUD2D item in this.allChilds ) {
                item.ClientSizeChanged();
            }
        }


        public override bool Intersects( Vector2 point ) {
            bool intersects = false;
            foreach ( HUD2D item in this.allChilds ) {
                if ( item.Intersects( point ) ) {
                    intersects = true;
                }
            }
            return intersects;
        }


    }

}
