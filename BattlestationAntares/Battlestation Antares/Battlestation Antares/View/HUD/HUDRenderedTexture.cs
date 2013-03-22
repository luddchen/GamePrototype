using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares;

namespace Battlestation_Antaris.View.HUD {
    /// <summary>
    /// a class to pre-render content to a texture
    /// </summary>
    public abstract class HUDRenderedTexture : HUD2DTexture {

        Vector2 renderSize;

        Color backgroundColor;

        RenderTarget2D renderTarget;

        /// <summary>
        /// creates a new rendered texture that can finally be drawn like a normal texture
        /// </summary>
        /// <param name="renderSize"></param>
        /// <param name="backgroundColor"></param>
        public HUDRenderedTexture(Vector2? renderSize, Color? backgroundColor) {
            this.renderSize = renderSize ?? new Vector2( 100, 100 );
            this.backgroundColor = backgroundColor ?? Color.Transparent;
            this.renderTarget = new RenderTarget2D( Antares.graphics.GraphicsDevice, (int)this.renderSize.X, (int)this.renderSize.Y );
        }


        /// <summary>
        /// render content to the elements texture
        /// dont override !
        /// </summary>
        public void Render() {
            RenderTarget2D oldTarget = null;
            if ( Antares.graphics.GraphicsDevice.GetRenderTargets().Length > 0 ) {
                oldTarget = (RenderTarget2D)Antares.graphics.GraphicsDevice.GetRenderTargets()[0].RenderTarget;
            }
            Antares.graphics.GraphicsDevice.SetRenderTarget( renderTarget );
            Antares.graphics.GraphicsDevice.Clear( this.backgroundColor );

            _RenderContent();

            Antares.graphics.GraphicsDevice.SetRenderTarget( oldTarget );

            this.Texture = (Texture2D)renderTarget;
        }


        /// <summary>
        /// render the content
        /// override for own content
        /// </summary>
        protected abstract void _RenderContent();

    }
}