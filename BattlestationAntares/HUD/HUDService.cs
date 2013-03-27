using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HUD {

    public sealed class HUDService {

        public static void Initialize( Game game, Texture2D defaultTexture, SpriteFont defaultFont, int? multiSampleCount, IMouseProvider input ) {
            HUDService.game = game;

            if ( HUDService.game == null ) {
                throw new ArgumentNullException( "HUDService.game" );
            }
            HUDService.DefaultTexture = defaultTexture;
            HUDService.DefaultFont = defaultFont;
            HUDService.MultiSampleCount = multiSampleCount ?? 0;
            HUDService.Input = input;
            HUDService.renderSize = new Point( 1920, 1080 );

            HUDService.game.Window.ClientSizeChanged += new EventHandler<EventArgs>( HUDService.Window_ClientSizeChanged );
            HUDService.CalculateRenderTextureParameter();
        }


        public static GraphicsDevice Device {
            get {
                if ( HUDService.game == null ) {
                    throw new ArgumentNullException( "HUDService.game" );
                }
                return HUDService.game.GraphicsDevice;
            }
        }


        public static ContentManager Content {
            get {
                if ( HUDService.game == null ) {
                    throw new ArgumentNullException( "HUDService.game" );
                }
                return HUDService.game.Content;
            }
        }


        public static Texture2D DefaultTexture;


        public static SpriteFont DefaultFont;


        public static int MultiSampleCount;


        public static IMouseProvider Input;


        public static Point RenderSize {
            get {
                Point newSize = new Point( renderSize.X, renderSize.Y );
                if ( HUDService.Device.GetRenderTargets().Length > 0 ) {
                    newSize.X = ( (Texture2D)HUDService.Device.GetRenderTargets()[0].RenderTarget ).Width;
                    newSize.Y = ( (Texture2D)HUDService.Device.GetRenderTargets()[0].RenderTarget ).Height;
                }
                return newSize;
            }
            set {
                HUDService.renderSize.X = value.X;
                HUDService.renderSize.Y = value.Y;
                HUDService.CalculateRenderTextureParameter();
            }
        }


        public static void CalculateRenderTextureParameter() {
            HUDService.RenderTexturePosition = new Vector2( HUDService.Device.Viewport.Width / 2.0f, HUDService.Device.Viewport.Height / 2.0f );
            HUDService.RenderTextureOrigin = new Vector2( HUDService.renderSize.X / 2.0f, HUDService.renderSize.Y / 2.0f );

            float xScale = (float)HUDService.Device.Viewport.Width / (float)HUDService.renderSize.X;
            float yScale = (float)HUDService.Device.Viewport.Height / (float)HUDService.renderSize.Y;

            HUDService.RenderTextureScale = Math.Min( xScale, yScale );
            HUDService.Input.setMouseTransform( HUDService.RenderTexturePosition, HUDService.RenderTextureOrigin, HUDService.RenderTextureScale );
        }


        static void Window_ClientSizeChanged( object sender, EventArgs e ) {
            HUDService.CalculateRenderTextureParameter();
        }


        private static Game game;


        private static Point renderSize;


        public static Vector2 RenderTexturePosition {
            get;
            private set;
        }


        public static Vector2 RenderTextureOrigin {
            get;
            private set;
        }


        public static float RenderTextureScale {
            get;
            private set;
        }

    }

}
