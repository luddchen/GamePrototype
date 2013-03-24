using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Battlestation_Antares.View.HUD {

    public class HUDValueLamp : HUDContainer {
        public delegate Color ColorProvider();

        public ColorProvider GetValue =
            delegate() {
                return Color.Green;
            };

        private HUDTexture foreground;

        private HUDTexture overlay;


        public HUDValueLamp( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType )
            : base( abstractPosition, positionType ) {
            this.SizeType = sizeType;
            this.AbstractSize = abstractSize;

            SetBackgroundColor( Color.Black );
            SetBackground( "Sprites//Circle" );

            this.foreground = new HUDTexture();
            this.foreground.PositionType = this.SizeType;
            this.foreground.AbstractSize = abstractSize * 0.95f;
            this.foreground.SizeType = sizeType;
            this.foreground.color = Color.White;
            this.foreground.Texture = Antares.content.Load<Texture2D>( "Sprites//Circle" );
            Add( this.foreground );

            this.overlay = new HUDTexture();
            this.overlay.PositionType = this.SizeType;
            this.overlay.AbstractSize = abstractSize;
            this.overlay.SizeType = sizeType;
            this.overlay.color = Color.White;
            this.SetNormal();
            Add( this.overlay );
            this.overlay.LayerDepth = base.LayerDepth - 0.02f;
        }

        public new float LayerDepth {
            set {
                base.LayerDepth = value;
                this.overlay.LayerDepth = value - 0.02f;
            }
        }

        public override sealed void Draw( SpriteBatch spriteBatch ) {
            this.foreground.color = this.GetValue();
            base.Draw( spriteBatch );
        }

        public void SetNormal() {
            this.overlay.Texture = Antares.content.Load<Texture2D>( "Sprites//HUD//Lamp" );
        }

    }

}