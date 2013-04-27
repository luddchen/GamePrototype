using HUD;
using Battlestation_Antares;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD {

    public static class AntaresButtonStyles {

        public static ButtonStyle Button() {
            ButtonStyle button = ButtonStyle.BuilderButtonStyle();
            button.backgroundTextureNormal = Antares.content.Load<Texture2D>( "Sprites//builder_button" );

            return button;
        }

        public static ButtonStyle EngineControllerStyle() {
            ButtonStyle newStyle = new ButtonStyle();

            newStyle.backgroundColorNormal = new Color( 32, 32, 0, 180 );
            newStyle.backgroundColorHover = new Color( 40, 40, 0, 180 );
            newStyle.backgroundColorPressed = new Color( 32, 32, 32, 255 );

            newStyle.foregroundColorNormal = Color.White;
            newStyle.foregroundColorHover = Color.White;
            newStyle.foregroundColorPressed = Color.White;

            newStyle.scaleNormal = 1f;
            newStyle.scaleHover = 1f;
            newStyle.scalePressed = 1f;

            newStyle.backgroundTextureNormal = Antares.content.Load<Texture2D>( "Sprites//OuterCircle" );

            return newStyle;
        }

        public static ButtonStyle EngineControllerStyle2() {
            ButtonStyle newStyle = new ButtonStyle();

            newStyle.backgroundColorNormal = new Color( 0, 0, 32, 180 );
            newStyle.backgroundColorHover = new Color( 0, 0, 40, 180 );
            newStyle.backgroundColorPressed = new Color( 32, 32, 32, 255 );

            newStyle.foregroundColorNormal = Color.White;
            newStyle.foregroundColorHover = Color.White;
            newStyle.foregroundColorPressed = Color.White;

            newStyle.scaleNormal = 1f;
            newStyle.scaleHover = 1f;
            newStyle.scalePressed = 1f;

            newStyle.backgroundTextureNormal = Antares.content.Load<Texture2D>( "Sprites//OuterCircle" );

            return newStyle;
        }

    }

}
