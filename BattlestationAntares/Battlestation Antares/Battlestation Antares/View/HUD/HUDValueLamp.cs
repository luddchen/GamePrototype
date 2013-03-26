using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HUD.HUD;
using System;

namespace Battlestation_Antares.View.HUD {

    public class HUDValueLamp : HUDMaskedContainer {
        public delegate Color ColorProvider();

        public ColorProvider GetValue =
            delegate() {
                return Color.Green;
            };

        private HUDTexture foreground;


        public HUDValueLamp( Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType )
            : base( abstractPosition, positionType, abstractSize, sizeType ) {

            SetBackground( "Sprites//Circle", Color.Black );
            SetMask( "Sprites//HUD//Lamp" );

            this.foreground = new HUDTexture( "Sprites//HUD//LampLight" );
            this.foreground.SizeType = sizeType;
            this.foreground.AbstractSize = abstractSize * 2.0f;
            Add( this.foreground );

        }

        public override sealed void Draw( SpriteBatch spriteBatch ) {
            this.foreground.color = this.GetValue();
            base.Draw( spriteBatch );
        }

    }

}