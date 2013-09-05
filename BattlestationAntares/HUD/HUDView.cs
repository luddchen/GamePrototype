using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using HUD.HUD;

namespace HUD {

    /// <summary>
    /// abstract basis class for views
    /// </summary>
    public abstract class HUDView {

        /// <summary>
        /// render target of this view
        /// </summary>
        public RenderTarget2D renderTarget;


        /// <summary>
        /// background color of this view
        /// </summary>
        private Color backgroundColor;


        /// <summary>
        /// spritebatch of this view
        /// </summary>
        private SpriteBatch spriteBatch;


        /// <summary>
        /// a list of HUD elements
        /// </summary>
        private List<HUD_Item> allItems;


        /// <summary>
        /// create a new view
        /// </summary>
        /// <param name="backgroundColor">background color of this view</param>
        public HUDView(Color? backgroundColor = null) {
            this.allItems = new List<HUD_Item>();
            this.backgroundColor = backgroundColor ?? Color.Transparent;
            this.spriteBatch = new SpriteBatch( HUDService.Device );
        }


        /// <summary>
        /// initialize content
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// draw content
        /// </summary>
        public void Draw() {
            this._initRenderTarget();
            HUDService.Device.SetRenderTarget( this.renderTarget );
            HUDService.Device.Clear( this.backgroundColor );

            DrawPreContent();

            // draw HUD elements
            this.spriteBatch.Begin(
                SpriteSortMode.BackToFront,
                BlendState.AlphaBlend,
                SamplerState.LinearClamp,
                DepthStencilState.DepthRead,
                RasterizerState.CullNone,
                null,
                Matrix.Identity);

            foreach ( HUD_Item element in this.allItems ) {
                element.Draw( this.spriteBatch );
            }

            this.spriteBatch.End();

            DrawPostContent();

            HUDService.Device.SetRenderTarget( null );
        }


        /// <summary>
        /// draw content behind the HUD
        /// </summary>
        protected virtual void DrawPreContent() {
        }


        /// <summary>
        /// draw content in front of the HUD
        /// </summary>
        protected virtual void DrawPostContent() {
        }


        public virtual void Add( HUD_Item item ) {
            this.allItems.Add( item );
        }


        public void AddRange( IEnumerable<HUD_Item> collection ) {
            this.allItems.AddRange( collection );
        }


        public void Remove( HUD_Item item ) {
            this.allItems.Remove( item );
        }


        public void Clear() {
            this.allItems.Clear();
        }


        public virtual void Window_ClientSizeChanged() {
            foreach ( HUD_Item element in this.allItems ) {
                element.RenderSizeChanged();
            }
        }


        private void _initRenderTarget() {
            if ( this.renderTarget == null 
                || this.renderTarget.Width != HUDService.RenderSize.X
                || this.renderTarget.Height != HUDService.RenderSize.Y
                || this.renderTarget.MultiSampleCount != HUDService.MultiSampleCount )
            {
                if ( this.renderTarget != null ) {
                    this.renderTarget.Dispose();
                }
                this.renderTarget = 
                    new RenderTarget2D(
                        HUDService.Device, HUDService.RenderSize.X, HUDService.RenderSize.Y, true,
                        HUDService.Device.DisplayMode.Format, DepthFormat.Depth24 ,
                        HUDService.MultiSampleCount, RenderTargetUsage.PlatformContents );

                this.Window_ClientSizeChanged();
            }
        }

    }

}
