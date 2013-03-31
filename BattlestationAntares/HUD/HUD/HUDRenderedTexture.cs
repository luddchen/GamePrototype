using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace HUD.HUD {
    /// <summary>
    /// a class to pre-render content to a texture
    /// </summary>
    public abstract class HUDRenderedTexture : HUDActionTexture {

        Point renderSize;

        Color backgroundColor;

        RenderTarget2D renderTarget;

        /// <summary>
        /// creates a new rendered texture that can finally be drawn like a normal texture
        /// </summary>
        /// <param name="renderSize"></param>
        /// <param name="backgroundColor"></param>
        public HUDRenderedTexture( Point? renderSize, Color? backgroundColor, IUpdateController controller = null ) 
            : base(controller: controller) 
        {
            this.renderSize = renderSize ?? new Point( 100, 100 );
            this.backgroundColor = backgroundColor ?? Color.Transparent;
            this.renderTarget = new RenderTarget2D( HUDService.Device, this.renderSize.X, this.renderSize.Y );

            Action =
                delegate() {
                    Render();
                };
        }


        /// <summary>
        /// render content to the elements texture
        /// dont override !
        /// </summary>
        private void Render() {
            RenderTarget2D oldTarget = null;
            if ( HUDService.Device.GetRenderTargets().Length > 0 ) {
                oldTarget = (RenderTarget2D)HUDService.Device.GetRenderTargets()[0].RenderTarget;
            }
            HUDService.Device.SetRenderTarget( renderTarget );
            HUDService.Device.Clear( this.backgroundColor );

            DrawContent();

            HUDService.Device.SetRenderTarget( oldTarget );

            this.Texture = (Texture2D)renderTarget;
        }


        /// <summary>
        /// render the content
        /// override for own content
        /// </summary>
        protected abstract void DrawContent();

    }
}