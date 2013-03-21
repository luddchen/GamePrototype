using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;

namespace Battlestation_Antares.View {

    /// <summary>
    /// abstract basis class for views
    /// </summary>
    public abstract class View {


        public RenderTarget2D renderTarget;


        /// <summary>
        /// if the view contains 3D elements
        /// </summary>
        public bool is3D;


        /// <summary>
        /// background color of this view
        /// </summary>
        public Color backgroundColor;


        /// <summary>
        /// a list of 2D HUD elements
        /// </summary>
        public List<HUD2D> allHUD_2D;


        /// <summary>
        /// a list of 3D HUD elements
        /// </summary>
        public List<HUD3D> allHUD_3D;


        /// <summary>
        /// create a new view, 3D disabled
        /// </summary>
        /// <param name="game">the game</param>
        public View() {
            this.allHUD_2D = new List<HUD2D>();
            this.allHUD_3D = new List<HUD3D>();
            this.is3D = false;
            this.backgroundColor = Color.Black;
        }


        /// <summary>
        /// initialize content
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        /// draw content
        /// HUD 
        /// </summary>
        public void Draw() {
            Antares.graphics.GraphicsDevice.Clear( Color.Transparent);

            // draw content
            DrawPreContent();

            // draw 2D HUD elements
            Antares.spriteBatch.Begin( SpriteSortMode.BackToFront, BlendState.AlphaBlend );//, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, null);

            foreach ( HUD2D element in this.allHUD_2D ) {
                element.Draw( Antares.spriteBatch );
            }

            Antares.spriteBatch.End();


            // draw 3D HUD elements if 3D is enabled
            if ( this.is3D ) {
                Antares.graphics.GraphicsDevice.DepthStencilState = new DepthStencilState() {
                    DepthBufferEnable = true
                };

                foreach ( HUD3D element in this.allHUD_3D ) {
                    element.Draw();
                }
            }

            DrawPostContent();

        }


        /// <summary>
        /// draw the view content
        /// </summary>
        abstract protected void DrawPreContent();


        protected virtual void DrawPostContent() {
        }


        public virtual void Window_ClientSizeChanged() {
            foreach ( HUD2D element in this.allHUD_2D ) {
                element.ClientSizeChanged();
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
