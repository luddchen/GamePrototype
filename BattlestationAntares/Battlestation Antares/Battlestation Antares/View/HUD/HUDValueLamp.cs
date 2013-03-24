﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Battlestation_Antares.View.HUD {

    public class HUDValueLamp : HUDContainer {
        public delegate Color ColorProvider();

        public ColorProvider GetValue =
            delegate() {
                return Color.Green;
            };

        //private HUDTexture background;

        private HUDTexture foreground;

        private HUDTexture overlay;


        public HUDValueLamp( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType )
            : base( abstractPosition, positionType ) {
            this.AbstractSize = abstractSize;
            this.SizeType = sizeType;

            this.background = new HUDTexture();
            this.background.positionType = this.SizeType;
            this.background.AbstractSize = abstractSize;
            this.background.SizeType = sizeType;
            this.background.color = Color.Black;
            this.background.Texture = Antares.content.Load<Texture2D>( "Sprites//Circle" );
            Add( this.background );

            this.foreground = new HUDTexture();
            this.foreground.positionType = this.SizeType;
            this.foreground.AbstractSize = abstractSize * 0.95f;
            this.foreground.SizeType = sizeType;
            this.foreground.color = Color.White;
            this.foreground.Texture = Antares.content.Load<Texture2D>( "Sprites//Circle" );
            Add( this.foreground );

            this.overlay = new HUDTexture();
            this.overlay.positionType = this.SizeType;
            this.overlay.AbstractSize = abstractSize;
            this.overlay.SizeType = sizeType;
            this.overlay.color = Color.White;
            this.SetNormal();
            Add( this.overlay );

            LayerDepth = 0.5f;
        }

        public new float LayerDepth {
            set {
                base.LayerDepth = value;
                this.background.LayerDepth = this.layerDepth;
                this.overlay.LayerDepth = this.layerDepth - 0.02f;
            }
            get {
                return base.LayerDepth;
            }
        }

        public override sealed void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch ) {
            this.foreground.color = this.GetValue();
            base.Draw( spritBatch );
        }

        public void SetNormal() {
            this.overlay.Texture = Antares.content.Load<Texture2D>( "Sprites//HUD//Lamp" );
        }

    }

}