using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;

namespace Battlestation_Antares.View {

    /// <summary>
    /// abstract basis class for views
    /// </summary>
    public abstract class View {

        /// <summary>
        /// render target of this view
        /// </summary>
        public RenderTarget2D renderTarget;


        /// <summary>
        /// background color of this view
        /// </summary>
        private Color backgroundColor;


        /// <summary>
        /// a list of 2D HUD elements
        /// </summary>
        public List<HUD2D> allHUD_2D;


        /// <summary>
        /// create a new view
        /// </summary>
        /// <param name="backgroundColor">background color of this view</param>
        public View(Color? backgroundColor) {
            this.allHUD_2D = new List<HUD2D>();
            this.backgroundColor = backgroundColor ?? Color.Transparent;
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
            Antares.graphics.GraphicsDevice.SetRenderTarget( this.renderTarget );
            Antares.graphics.GraphicsDevice.Clear( this.backgroundColor);

            // draw content behind HUD
            DrawPreContent();

            // draw 2D HUD elements
            Antares.spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend );//, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, null);

            foreach ( HUD2D element in this.allHUD_2D ) {
                element.Draw( Antares.spriteBatch );
            }

            Antares.spriteBatch.End();

            // draw content in front of HUD
            DrawPostContent();

            Antares.graphics.GraphicsDevice.SetRenderTarget( null );
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


        public virtual void Window_ClientSizeChanged() {
            foreach ( HUD2D element in this.allHUD_2D ) {
                element.ClientSizeChanged();
            }
        }


        private void _initRenderTarget() {
            if ( this.renderTarget == null || this.renderTarget.Width != (int)Antares.RenderSize.X || this.renderTarget.Height != (int)Antares.RenderSize.Y ) {
                if ( this.renderTarget != null ) {
                    this.renderTarget.Dispose();
                }
                this.renderTarget = new RenderTarget2D( Antares.graphics.GraphicsDevice, (int)Antares.RenderSize.X, (int)Antares.RenderSize.Y, true,
                                                        Antares.graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24 );
                this.Window_ClientSizeChanged();
            }
        }


        public void ButtonUpdate() {
            foreach ( HUD2D item in this.allHUD_2D ) {
                ButtonUpdate( item );
            }
        }

        private void ButtonUpdate( HUD2D item ) {
            if ( item.isVisible ) {
                if ( item is HUD2DButton ) {
                    ( (HUD2DButton)item ).isUpdatedClicked( Antares.inputProvider );
                }

                if ( item is HUD2DContainer ) {
                    foreach ( HUD2D child in ( (HUD2DContainer)item ).allChilds ) {
                        ButtonUpdate( child );
                    }
                }

            }
        }

    }

}
