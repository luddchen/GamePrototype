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

        List<KeyAssignment> keyAssignments; 

        public InputProvider()
        {
            keyAssignments = new List<KeyAssignment>();
            keyAssignments.Add(new KeyAssignment(Control.PITCH_UP, Keys.Up));
            keyAssignments.Add(new KeyAssignment(Control.PITCH_DOWN, Keys.Down));
            keyAssignments.Add(new KeyAssignment(Control.YAW_LEFT, Keys.Left));
            keyAssignments.Add(new KeyAssignment(Control.YAW_RIGHT, Keys.Right));
            keyAssignments.Add(new KeyAssignment(Control.INCREASE_THROTTLE, Keys.W));
            keyAssignments.Add(new KeyAssignment(Control.DECREASE_THROTTLE, Keys.S));
        }

        public void Update()
        {
            this.oldKeyboardState = this.newKeyboardState;
            this.newKeyboardState = Keyboard.GetState();
        }


        // old input version for testing
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

        public List<Control> getInput()
        {
            List<Control> controlSequence = new List<Control>();

            foreach (KeyAssignment assignment in this.keyAssignments)
            {
                if ( this.newKeyboardState.IsKeyDown(assignment.key) )
                {
                    controlSequence.Add(assignment.control);
                }
            }

            return controlSequence;
        }

    }

}
