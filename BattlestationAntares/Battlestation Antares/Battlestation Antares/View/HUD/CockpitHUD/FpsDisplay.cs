using Microsoft.Xna.Framework;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD.CockpitHUD {
    class FpsDisplay : HUDArray {

        private HUDString fps;

        private int elapsedTime = 0;

        private int frameCounter = 0;


        public FpsDisplay( Vector2 position) : base( position, new Vector2(0.06f, 0.02f) ) {
            this.borderSize = new Vector2( 0.003f, 0.003f );
            this.direction = LayoutDirection.HORIZONTAL;

            HUDString text;
            text = new HUDString( "FPS : " );

            this.fps = new HUDString( "" + frameCounter );

            Add( text );
            Add( this.fps );
        }


        public void Update( GameTime gameTime ) {
            this.elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            this.frameCounter++;

            if ( this.elapsedTime > 1000 ) {
                fps.Text = "" + frameCounter;
                this.frameCounter = 0;
                this.elapsedTime = 0;
            }
        }

    }
}
