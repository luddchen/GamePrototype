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
        /// spritebatch of this view
        /// </summary>
        private SpriteBatch spriteBatch;


        /// <summary>
        /// a list of HUD elements
        /// </summary>
        private List<HUD.HUD_Item> allItems;


        /// <summary>
        /// create a new view
        /// </summary>
        /// <param name="backgroundColor">background color of this view</param>
        public View(Color? backgroundColor) {
            this.allItems = new List<HUD.HUD_Item>();
            this.backgroundColor = backgroundColor ?? Color.Transparent;
            this.spriteBatch = new SpriteBatch( Antares.graphics.GraphicsDevice );
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
            this.spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend );// SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, null);

            foreach ( HUD.HUD_Item element in this.allItems ) {
                element.Draw( this.spriteBatch );
            }

            this.spriteBatch.End();

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


        public void Add( HUD.HUD_Item item ) {
            this.allItems.Add( item );
        }


        public void AddRange( IEnumerable<HUD.HUD_Item> collection ) {
            this.allItems.AddRange( collection );
        }


        public virtual void Window_ClientSizeChanged() {
            foreach ( HUD.HUD_Item element in this.allItems ) {
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
            foreach ( HUD.HUD_Item item in this.allItems ) {
                ButtonUpdate( item );
            }
        }

        private void ButtonUpdate( HUD.HUD_Item item ) {
            if ( item.isVisible ) {
                if ( item is HUDButton ) {
                    ( (HUDButton)item ).isUpdatedClicked( Antares.inputProvider );
                }

                if ( item is HUDContainer ) {
                    foreach ( HUD.HUD_Item child in ( (HUDContainer)item ).allChilds ) {
                        ButtonUpdate( child );
                    }
                }

            }
        }

    }

}
