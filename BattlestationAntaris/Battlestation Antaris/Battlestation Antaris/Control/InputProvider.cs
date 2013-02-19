using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// depricated enumeration for control and key assignment, actually used to switch the different situations
    /// </summary>
    public enum ControlKey
    {
        UP = Keys.Up, DOWN = Keys.Down, LEFT = Keys.Left, RIGHT = Keys.Right, SPEEDUP = Keys.W, SPEEDDOWN = Keys.S, ESC = Keys.Escape, SPACE = Keys.Space
    }


    /// <summary>
    /// depricated enumeration for key states, actually used to switch the different situations
    /// </summary>
    public enum ControlState
    {
        PRESSED, RELEASED, DOWN, UP
    }


    /// <summary>
    /// a class that collects and provides user/player input
    /// </summary>
    public class InputProvider
    {

        /// <summary>
        /// the old keyboard state
        /// </summary>
        private KeyboardState oldKeyboardState;


        /// <summary>
        /// the new keyboard state
        /// </summary>
        private KeyboardState newKeyboardState;


        /// <summary>
        /// a list of all key assignments
        /// </summary>
        private List<KeyAssignment> keyAssignments; 


        /// <summary>
        /// create a new input provider
        /// </summary>
        public InputProvider()
        {
            keyAssignments = new List<KeyAssignment>();

            // create some key assignments
            keyAssignments.Add(new KeyAssignment(Control.PITCH_UP, Keys.Up));
            keyAssignments.Add(new KeyAssignment(Control.PITCH_DOWN, Keys.Down)); 
            keyAssignments.Add(new KeyAssignment(Control.ROLL_ANTICLOCKWISE, Keys.A));
            keyAssignments.Add(new KeyAssignment(Control.ROLL_CLOCKWISE, Keys.D));
            keyAssignments.Add(new KeyAssignment(Control.YAW_LEFT, Keys.Left));
            keyAssignments.Add(new KeyAssignment(Control.YAW_RIGHT, Keys.Right));
            keyAssignments.Add(new KeyAssignment(Control.INCREASE_THROTTLE, Keys.W));
            keyAssignments.Add(new KeyAssignment(Control.DECREASE_THROTTLE, Keys.S));
        }


        /// <summary>
        /// update the input provider
        /// </summary>
        public void Update()
        {
            this.oldKeyboardState = this.newKeyboardState;
            this.newKeyboardState = Keyboard.GetState();
        }


        // depricated input version , actually used to switch sitations 
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


        /// <summary>
        /// get a list of control requests, depending on the configured key assignments
        /// </summary>
        /// <returns>a list of control requests</returns>
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
