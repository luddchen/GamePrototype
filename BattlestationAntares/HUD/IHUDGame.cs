using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HUD {

    public interface IHUDGame {

        Point RenderSize ();

        int MultiSampleCount {
            get;
            set;
        }

        GraphicsDevice GraphicsDevice {
            get;
        }

        ContentManager Content {
            get;
        }

        Texture2D DefaultTexture {
            get;
        }

        SpriteFont DefaultFont {
            get;
        }

    }

}
