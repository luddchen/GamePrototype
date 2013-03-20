using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.View.HUD {
    public class ButtonStyle {
        public Color backgroundColorNormal;

        public Color backgroundColorHover;

        public Color backgroundColorPressed;

        public Color foregroundColorNormal;

        public Color foregroundColorHover;

        public Color foregroundColorPressed;

        public float scaleNormal;

        public float scaleHover;

        public float scalePressed;


        public static ButtonStyle DefaultButtonStyle() {
            ButtonStyle newStyle = new ButtonStyle();

            newStyle.backgroundColorNormal = new Color( 32, 48, 48, 160 );
            newStyle.backgroundColorHover = new Color( 40, 64, 64, 192 );
            newStyle.backgroundColorPressed = new Color( 32, 48, 48, 255 );

            newStyle.foregroundColorNormal = Color.White;
            newStyle.foregroundColorHover = new Color( 255, 255, 128 );
            newStyle.foregroundColorPressed = new Color( 128, 255, 128 );

            newStyle.scaleNormal = 1.0f;
            newStyle.scaleHover = 0.99f;
            newStyle.scalePressed = 0.96f;

            return newStyle;
        }

        public static ButtonStyle BuilderButtonStyle() {
            ButtonStyle newStyle = new ButtonStyle();

            newStyle.backgroundColorNormal = new Color( 56, 64, 32, 128);
            newStyle.backgroundColorHover = new Color( 80, 96, 64, 128);
            newStyle.backgroundColorPressed = new Color( 32, 48, 48);

            newStyle.foregroundColorNormal = Color.White;
            newStyle.foregroundColorHover = new Color( 255, 255, 128 );
            newStyle.foregroundColorPressed = new Color( 128, 255, 128 );

            newStyle.scaleNormal = 1.0f;
            newStyle.scaleHover = 1.0f;
            newStyle.scalePressed = 0.96f;

            return newStyle;
        }

        public static ButtonStyle NoBackgroundButtonStyle() {
            ButtonStyle newStyle = new ButtonStyle();

            newStyle.backgroundColorNormal = new Color( 0, 0, 0, 0 );
            newStyle.backgroundColorHover = new Color( 64, 64, 64, 32 );
            newStyle.backgroundColorPressed = new Color( 32, 48, 48, 255 );

            newStyle.foregroundColorNormal = Color.White;
            newStyle.foregroundColorHover = new Color( 255, 255, 64 );
            newStyle.foregroundColorPressed = new Color( 128, 255, 128 );

            newStyle.scaleNormal = 1.0f;
            newStyle.scaleHover = 0.99f;
            newStyle.scalePressed = 0.96f;

            return newStyle;
        }

        public static ButtonStyle RemoveButtonStyle() {
            ButtonStyle newStyle = new ButtonStyle();

            newStyle.backgroundColorNormal = new Color( 0, 0, 0, 0 );
            newStyle.backgroundColorHover = new Color( 0, 0, 0, 0 );
            newStyle.backgroundColorPressed = new Color( 0, 0, 0, 0 );

            newStyle.foregroundColorNormal = Color.Red;
            newStyle.foregroundColorHover = Color.White;
            newStyle.foregroundColorPressed = Color.White;

            newStyle.scaleNormal = 1.0f;
            newStyle.scaleHover = 0.99f;
            newStyle.scalePressed = 0.96f;

            return newStyle;
        }

        public static ButtonStyle SliderButtonStyle() {
            ButtonStyle newStyle = new ButtonStyle();

            newStyle.backgroundColorNormal = Color.White;
            newStyle.backgroundColorHover = new Color( 128, 255, 128 );
            newStyle.backgroundColorPressed = new Color( 128, 255, 128 );

            newStyle.foregroundColorNormal = Color.White;
            newStyle.foregroundColorHover = Color.White;
            newStyle.foregroundColorPressed = Color.White;

            newStyle.scaleNormal = 1.0f;
            newStyle.scaleHover = 1.0f;
            newStyle.scalePressed = 1.0f;

            return newStyle;
        }
    }
}
