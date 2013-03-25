using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HUD.HUD {
    /// <summary>
    /// a class to pre-render content to a texture
    /// </summary>
    public abstract class HUDRenderedTexture : HUDTexture {

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
            this.renderTarget = new RenderTarget2D( HUD_Item.game.GraphicsDevice, (int)this.renderSize.X, (int)this.renderSize.Y );
        }


        /// <summary>
        /// render content to the elements texture
        /// dont override !
        /// </summary>
        public void Render() {
            RenderTarget2D oldTarget = null;
            if ( HUD_Item.game.GraphicsDevice.GetRenderTargets().Length > 0 ) {
                oldTarget = (RenderTarget2D)HUD_Item.game.GraphicsDevice.GetRenderTargets()[0].RenderTarget;
            }
            HUD_Item.game.GraphicsDevice.SetRenderTarget( renderTarget );
            HUD_Item.game.GraphicsDevice.Clear( this.backgroundColor );

            _RenderContent();

            HUD_Item.game.GraphicsDevice.SetRenderTarget( oldTarget );

            this.Texture = (Texture2D)renderTarget;
        }


        /// <summary>
        /// render the content
        /// override for own content
        /// </summary>
        protected abstract void _RenderContent();

    }
}