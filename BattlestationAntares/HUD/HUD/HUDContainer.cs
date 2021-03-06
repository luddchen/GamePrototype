﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.ObjectModel;

namespace HUD.HUD {

    public class HUDContainer : HUD_Item {

        private HUDTexture background;

        private List<HUD_Item> allChilds;

        public ReadOnlyCollection<HUD_Item> AllChilds {
            get {
                return allChilds.AsReadOnly();
            }
        }

        public override float LayerDepth {
            set {
                foreach ( HUD_Item item in this.allChilds ) {
                    item.LayerDepth = value - 0.01f;
                }
                _initBackground();
                this.background.LayerDepth = value;
                base.LayerDepth = value;
            }
        }

        public override HUDType SizeType {
            set {
                _initBackground();
                this.background.SizeType = value;
                base.SizeType = value;
            }
        }

        public override Vector2 AbstractSize {
            set {
                _initBackground();
                this.background.AbstractSize = value;
                base.AbstractSize = value;
            }
        }

        public override float AbstractScale {
            set {
                _initBackground();
                base.AbstractScale = value;
            }
        }


        public HUDContainer( Vector2 position = new Vector2(), Vector2 size = new Vector2() ) {
            this.allChilds = new List<HUD_Item>();
            this.AbstractPosition = position;
            this.AbstractSize = size;
            _initBackground();
        }


        public virtual void Add( HUD_Item item ) {
            item.Parent = this;
            item.LayerDepth = this.LayerDepth - 0.01f;
            item.RenderSizeChanged();
            this.allChilds.Add( item );
        }

        public virtual void Insert( int index, HUD_Item item ) {
            item.Parent = this;
            item.LayerDepth = this.LayerDepth - 0.01f;
            item.RenderSizeChanged();
            this.allChilds.Insert( index, item );
        }

        public virtual void Remove( HUD_Item item ) {
            this.allChilds.Remove( item );
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

        /// <summary>
        /// set a background property
        /// Color, Texture or Texture-Name String makes background visible, NULL makes it invisible
        /// </summary>
        /// <param name="property"></param>
        public void SetBackground( Object property ) {
            _initBackground();

            bool visibility = true;

            if ( property == null ) {
                visibility = false;
            } else {
                if ( property is Texture2D ) {
                    this.background.Texture = (Texture2D)property;
                } else if ( property is String ) {
                    this.background.Texture = HUDService.Content.Load<Texture2D>( (String)property );
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


        private void _initBackground() {
            if ( this.background == null ) {
                this.background = new HUDTexture();
                this.background.Parent = this;
                this.background.IsVisible = false;
                this.background.LayerDepth = this.LayerDepth;
                this.background.AbstractSize = this.AbstractSize;
            }
        }

        public int ItemCount() {
            return this.allChilds.Count;
        }

    }

}
