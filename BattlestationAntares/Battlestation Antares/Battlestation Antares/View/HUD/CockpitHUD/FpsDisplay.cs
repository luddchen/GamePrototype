using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD.CockpitHUD {
    class FpsDisplay : HUD2DContainer {

        private HUD2DString fps;

        private int elapsedTime = 0;

        private int frameCounter = 0;


        public FpsDisplay( Vector2 position, Antares game )
            : base( position, HUDType.ABSOLUT, game ) {
            HUD2DTexture background = new HUD2DTexture( game );
            background.color = new Color( 32, 32, 32, 160 );
            background.abstractSize = new Vector2( 80, 25 );

            HUD2DString text;
            text = new HUD2DString( "FPS : ", this.game );
            text.abstractPosition = new Vector2( -15, 0 );
            text.scale = 0.4f;
            text.layerDepth = 0.4f;

            this.fps = new HUD2DString( "" + frameCounter, this.game );
            this.fps.abstractPosition = new Vector2( 25, 0 );
            this.fps.scale = 0.4f;
            this.fps.layerDepth = 0.4f;

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
