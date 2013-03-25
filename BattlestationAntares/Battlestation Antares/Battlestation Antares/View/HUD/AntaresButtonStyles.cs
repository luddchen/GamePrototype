using HUD;
using Battlestation_Antares;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD {

    public static class AntaresButtonStyles {

        public static ButtonStyle Button() {
            ButtonStyle button = ButtonStyle.BuilderButtonStyle();
            button.backgroundTextureNormal = Antares.content.Load<Texture2D>( "Sprites//builder_button" );

            return button;
        }

    }

}
