using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HUD {

    public interface IInputProvider {

        bool isLeftMouseButtonPressed();

        bool isRightMouseButtonPressed();

        bool isLeftMouseButtonDown();

        bool isRightMouseButtonDown();

        Vector2 getMousePos();

    }

}
