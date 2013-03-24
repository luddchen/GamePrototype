using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD.CockpitHUD {
    class FpsDisplay : HUDContainer {

        private HUDString fps;

        private int elapsedTime = 0;

        private int frameCounter = 0;


        public FpsDisplay( Vector2 position) : base( position, HUDType.ABSOLUT) {
            HUDTexture background = new HUDTexture();
            background.color = new Color( 32, 32, 32, 160 );
            background.abstractSize = new Vector2( 80, 25 );

            HUDString text;
            text = new HUDString( "FPS : " );
            text.abstractPosition = new Vector2( -15, 0 );
            text.scale = 0.4f;
            text.LayerDepth = 0.4f;

            this.fps = new HUDString( "" + frameCounter );
            this.fps.abstractPosition = new Vector2( 25, 0 );
            this.fps.scale = 0.4f;
            this.fps.LayerDepth = 0.4f;

            Add( background );
            Add( text );
            Add( this.fps );
        }


        public void Update( GameTime gameTime ) {
            this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            this.frameCounter++;

            if ( this.elapsedTime > 1000 ) {
                fps.String = "" + frameCounter;
                this.frameCounter = 0;
                this.elapsedTime = 0;
            }
        }

    }
}
