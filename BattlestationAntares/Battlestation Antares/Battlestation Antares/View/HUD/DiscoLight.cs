using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using HUD.HUD;
using HUD;
using Battlestation_Antares.Tools;

namespace Battlestation_Antares.View.HUD {

    public class DiscoLight : HUDContainer, IUpdatableItem {

        private int rotationSpeed = 0;

        public DiscoLight(Vector2 position, Vector2 size, float depth, int childs) : base( position, size ) {

            this.rotationSpeed = RandomGen.random.Next( 1, 3 ) * ( ( RandomGen.random.Next( 2 ) == 0 ) ? -1 : 1 );

            for ( int i = 0; i < childs; i++ ) {
                if ( depth > 0 ) {

                    DiscoLight newDisco =
                        new DiscoLight(
                            new Vector2( 
                                ((float)RandomGen.random.NextDouble() * size.X - size.X) * 0.9f, 
                                ((float)RandomGen.random.NextDouble() * size.Y - size.Y ) * 0.9f),
                            size * 0.9f, depth - 1, childs);
                    Add( newDisco );

                } else {

                    float lightSize = (float)RandomGen.random.NextDouble() * 0.3f + 0.6f;
                    HUDTexture tex =
                        new HUDTexture(
                            "Sprites//HUD//LampLight", null, 
                            new Vector2(
                                ((float)RandomGen.random.NextDouble() * size.X - size.X / 2) * 0.7f, 
                                ((float)RandomGen.random.NextDouble() * size.Y - size.Y / 2) * 0.7f),
                            new Vector2( lightSize * HUDService.RenderSize.Y / HUDService.RenderSize.X, lightSize ) );
                    tex.color =
                        new Color(
                            (float)( RandomGen.random.NextDouble() * 0.01 ),
                            (float)( RandomGen.random.NextDouble() * 0.01 ),
                            (float)( RandomGen.random.NextDouble() * 0.01 ),
                            (float)( RandomGen.random.NextDouble() * 0.001 ) );
                    Add( tex );

                }
            }

        }


        public void Update( GameTime gameTime ) {
            this.AbstractRotation += 0.003f * rotationSpeed;
            if ( this.AbstractRotation >= Math.PI * 2 ) {
                this.AbstractRotation = 0;
            }
            foreach ( HUD_Item item in this.AllChilds ) {
                if ( item is DiscoLight ) {
                    ( item as DiscoLight ).Update( gameTime );
                }
            }
        }

        public bool Enabled {
            get {
                return true;
            }
        }
    }

}
