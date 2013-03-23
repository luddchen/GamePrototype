using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD.AIComposer;
using Battlestation_Antares;

namespace Battlestation_Antaris.View.HUD.AIComposer {
    public class AI_Bank : HUDContainer, IUpdatableItem {

        public static Color NORMAL_COLOR = new Color( 28, 32, 24, 16 );

        public static Color ENABLED_COLOR = new Color( 20, 32, 20, 16 );

        public static Color DISABLED_COLOR = new Color( 32, 20, 20, 16 );

        public HUDTexture background;

        public Action mouseOverAction;

        public Action mousePressedAction;

        public AI_Bank(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType) : base( abstractPosition, positionType ) {
            this.abstractSize = abstractSize;
            this.sizeType = sizeType;
            this.background = new HUDTexture();
            this.background.abstractSize = abstractSize;
            this.background.sizeType = sizeType;
            this.background.color = new Color( 32, 32, 32, 32 );

            base.Add( this.background );
            this.setLayerDepth( this.layerDepth );
        }


        public override void Add( HUD_Item element ) {
            if ( element is AI_Item && hasFreePlace( element ) ) {
                base.Add( element );
                arrangeItems();
            }
        }


        public void InsertAt( AI_Item item, float pos ) {
            if (hasFreePlace(item)) {
                int index = (int)(0.5f + (this.allChilds.Count - 1.0f) * pos);
                this.allChilds.Insert( index, item );

                item.parent = this;
                item.setLayerDepth( this.layerDepth - 0.01f );
                item.ClientSizeChanged();
                arrangeItems();
            }
        }


        public override void Remove( HUD_Item element ) {
            base.Remove( element );
            if ( element is AI_Item ) {
                arrangeItems();
            }
        }


        public bool hasFreePlace(HUD_Item element) {
            bool freePlace = false;
            float freeWidth = this.abstractSize.X * Antares.RenderSize.X;
            foreach ( HUD_Item item in this.allChilds ) {
                if ( item is AI_Item ) {
                    freeWidth -= item.abstractSize.X;
                }
            }
            if ( freeWidth > element.abstractSize.X ) {
                freePlace = true;
            }
            return freePlace;
        }


        private void arrangeItems() {
            int itemCount = 0;
            foreach ( HUD_Item item in this.allChilds ) {
                if ( item is AI_Item ) {
                    itemCount++;
                }
            }
            float itemOffset = this.abstractSize.X / itemCount;
            float newPos = -this.abstractSize.X / 2 + itemOffset / 2;
            foreach ( HUD_Item item in this.allChilds ) {
                if ( item is AI_Item ) {
                    item.abstractPosition = new Vector2(newPos, 0);
                    newPos += itemOffset;
                    item.ClientSizeChanged();
                }
            }
        }


        public override void setLayerDepth( float layerDepth ) {
            base.setLayerDepth( layerDepth );
            this.background.layerDepth = this.layerDepth;
        }


        public override bool Intersects( Microsoft.Xna.Framework.Vector2 point ) {
            return this.background.Intersects(point);
        }

        public void Update( GameTime gameTime ) {
            if ( this.Intersects( Antares.inputProvider.getMousePos() ) ) {
                if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                    if ( this.mouseOverAction != null ) {
                        this.mousePressedAction();
                    }
                } else {
                    if ( this.mousePressedAction != null ) {
                        this.mouseOverAction();
                    }
                }
            } else {
                this.background.color = AI_Bank.NORMAL_COLOR;
            }
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }
}
