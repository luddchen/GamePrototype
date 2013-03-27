using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HUD {

    public interface IMouseProvider {

        void Update();

        bool isLeftMouseButtonPressed();

        bool isRightMouseButtonPressed();

        bool isLeftMouseButtonDown();

        bool isRightMouseButtonDown();

        Vector2 getMousePos();

        bool isMouseMoved();

        int getMouseWheelChange();

        void setMouseTransform( Vector2 screenSizeHalf, Vector2 renderSizeHalf, float renderScale );

        void setMouseTransform( Matrix transform );

    }

}
