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


        public HUDValueLamp( Vector2 abstractPosition, Vector2 abstractSize )
            : base( abstractPosition, abstractSize ) {

            SetBackground( "Sprites//Circle", Color.Black );
            SetMask( "Sprites//HUD//Lamp" );

            this.foreground = new HUDTexture( "Sprites//HUD//Lamp2" );
            this.foreground.AbstractSize = abstractSize * 3.6f;
            Add( this.foreground );

        }

        public override sealed void Draw( SpriteBatch spriteBatch ) {
            this.foreground.color = this.GetValue();
            base.Draw( spriteBatch );
        }

    }

}