using System;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD.AIComposer;
using Battlestation_Antares;
using HUD.HUD;
using HUD;

namespace Battlestation_Antaris.View.HUD.AIComposer {
    public class AI_Bank : HUDContainer, IUpdatableItem {

        public static Color NORMAL_COLOR = new Color( 28, 32, 24, 16 );

        public static Color ENABLED_COLOR = new Color( 20, 32, 20, 16 );

        public static Color DISABLED_COLOR = new Color( 32, 20, 20, 16 );

        public Action mouseOverAction;

        public Action mousePressedAction;

        public AI_Bank(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType) : base( abstractPosition ) {
            this.SizeType = sizeType;
            this.AbstractSize = abstractSize;
            SetBackground( new Color( 32, 32, 32, 32 ) );
        }


        public override void Add( HUD_Item element ) {
            if ( element is AI_Item && hasFreePlace( element ) ) {
                base.Add( element );
                arrangeItems();
            }
        }


        public void InsertAt( AI_Item item, float pos ) {
            if (hasFreePlace(item)) {
                int index = (int)(0.5f + (this.AllChilds.Count - 1.0f) * pos);
                Insert( index, item );
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
            float freeWidth = this.AbstractSize.X * HUD_Item.game.RenderSize().X;
            foreach ( HUD_Item item in this.AllChilds ) {
                if ( item is AI_Item ) {
                    freeWidth -= item.AbstractSize.X;
                }
            }
            if ( freeWidth > element.AbstractSize.X ) {
                freePlace = true;
            }
            return freePlace;
        }


        private void arrangeItems() {
            int itemCount = 0;
            foreach ( HUD_Item item in this.AllChilds ) {
                if ( item is AI_Item ) {
                    itemCount++;
                }
            }
            float itemOffset = this.AbstractSize.X / itemCount;
            float newPos = -this.AbstractSize.X / 2 + itemOffset / 2;
            foreach ( HUD_Item item in this.AllChilds ) {
                if ( item is AI_Item ) {
                    item.AbstractPosition = new Vector2(newPos, 0);
                    newPos += itemOffset;
                }
            }
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
                SetBackground( AI_Bank.NORMAL_COLOR );
            }
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }
}
