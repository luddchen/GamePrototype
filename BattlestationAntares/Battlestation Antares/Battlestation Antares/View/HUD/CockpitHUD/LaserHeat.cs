using Microsoft.Xna.Framework;
using System;
using HUD.HUD;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Battlestation_Antares.View.HUD.CockpitHUD {

    public class LaserHeat : HUDMaskedContainer {

        private List<Color> colors;

        public LaserHeat( Vector2 abstractPosition, Vector2 abstractSize ) : base( abstractPosition, abstractSize ) {
            this.SetBackground( "Sprites//HUD//ValueBar2BG" , Color.Black );
            this.SetMask( "Sprites//HUD//ValueBar2" , Color.GhostWhite);

            this.colors = new List<Color>();

            for ( int i = 0; i < 8; i++ ) {
                HUDTexture tex = 
                    new HUDTexture( 
                        "Sprites//HUD//Lamp2", new Color( i * 32, 255 - i * 32, 0 ),
                        new Vector2( 0, abstractSize.Y / 2.0f - ( (float)i + 0.5f ) * abstractSize.Y / 8.0f ), 
                        new Vector2( abstractSize.X * 3.6f, abstractSize.Y * 3.6f / 8.0f) );
                colors.Add( tex.color );
                Add( tex );
            }
        }

        public override void Draw( SpriteBatch spriteBatch ) {
            float value = Antares.world.spaceShip.attributes.Laser.CurrentHeat / Antares.world.spaceShip.attributes.Laser.HeatUntilCooldown;

            foreach ( HUD_Item item in this.AllChilds ) {
                int index = this.AllChilds.IndexOf( item );
                float itemValue = (float)index / 8.0f;

                if ( itemValue < value && itemValue > value - 0.125f ) {
                    item.color = Color.Multiply( colors[index], ( value - itemValue ) * 8.0f );
                } else {
                    item.color = colors[index];
                }
                item.IsVisible = ( itemValue < value );
            }

            base.Draw( spriteBatch );
        }

    }

}
