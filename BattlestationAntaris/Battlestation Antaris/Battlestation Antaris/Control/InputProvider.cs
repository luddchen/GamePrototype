using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Battlestation_Antaris.Control
{

    public enum ControlKey
    {
        UP = Keys.Up, DOWN = Keys.Down, LEFT = Keys.Left, RIGHT = Keys.Right, SPEEDUP = Keys.W, SPEEDDOWN = Keys.S, ESC = Keys.Escape, SPACE = Keys.Space
    }

    public enum ControlState
    {
        PRESSED, RELEASED, DOWN, UP
    }

    public class InputProvider
    {
        KeyboardState oldKeyboardState;
        KeyboardState newKeyboardState;     

        public InputProvider()
        {
        }

        public void Update()
        {
            this.oldKeyboardState = this.newKeyboardState;
            this.newKeyboardState = Keyboard.GetState();
        }

        public bool isKeyOnState(ControlKey key, ControlState state) 
        {
            bool oldState = this.oldKeyboardState.IsKeyDown((Keys)key);
            bool newState = this.newKeyboardState.IsKeyDown((Keys)key);

            if (state == ControlState.PRESSED && !oldState && newState) return true;
            if (state == ControlState.RELEASED && oldState && !newState) return true;
            if (state == ControlState.DOWN && newState) return true;
            if (state == ControlState.UP && !newState) return true;
            return false;
        }

    }

}
