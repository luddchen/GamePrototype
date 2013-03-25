using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using HUD;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// depricated enumeration for control and key assignment, actually used to switch the different situations
    /// </summary>
    public enum ControlKey {
        UP = Keys.Up,
        DOWN = Keys.Down,
        LEFT = Keys.Left,
        RIGHT = Keys.Right,
        SPEEDUP = Keys.W,
        SPEEDDOWN = Keys.S,
        ESC = Keys.Escape,
        SPACE = Keys.Space
    }


    /// <summary>
    /// depricated enumeration for key states, actually used to switch the different situations
    /// </summary>
    public enum ControlState {
        PRESSED,
        RELEASED,
        DOWN,
        UP
    }


    /// <summary>
    /// a class that collects and provides user/player input
    /// </summary>
    public class InputProvider : IInputProvider {

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
        /// the old mouse state
        /// </summary>
        private MouseState oldMouseState;


        /// <summary>
        /// the new mouse state
        /// </summary>
        private MouseState newMouseState;


        private Vector2 screenSizeHalf;
        private Vector2 renderSizeHalf;
        private float renderScale;


        /// <summary>
        /// create a new input provider
        /// </summary>
        public InputProvider() {
            keyAssignments = new List<KeyAssignment>();

            // create some key assignments
            keyAssignments.Add( new KeyAssignment( Control.PITCH_UP, Keys.Up ) );
            keyAssignments.Add( new KeyAssignment( Control.PITCH_DOWN, Keys.Down ) );
            keyAssignments.Add( new KeyAssignment( Control.ROLL_ANTICLOCKWISE, Keys.A ) );
            keyAssignments.Add( new KeyAssignment( Control.ROLL_CLOCKWISE, Keys.D ) );
            keyAssignments.Add( new KeyAssignment( Control.YAW_LEFT, Keys.Left ) );
            keyAssignments.Add( new KeyAssignment( Control.YAW_RIGHT, Keys.Right ) );
            keyAssignments.Add( new KeyAssignment( Control.INCREASE_THROTTLE, Keys.W ) );
            keyAssignments.Add( new KeyAssignment( Control.DECREASE_THROTTLE, Keys.S ) );

            keyAssignments.Add( new KeyAssignment( Control.FIRE_LASER, Keys.Space ) );
            keyAssignments.Add( new KeyAssignment( Control.FIRE_MISSILE, Keys.LeftControl ) );
            keyAssignments.Add( new KeyAssignment( Control.TARGET_NEXT_ENEMY, Keys.Tab ) );
        }


        /// <summary>
        /// update the input provider
        /// </summary>
        public void Update() {
            this.oldKeyboardState = this.newKeyboardState;
            this.newKeyboardState = Keyboard.GetState();

            this.oldMouseState = this.newMouseState;
            this.newMouseState = Mouse.GetState();
        }


        // depricated input version , actually used to switch sitations 
        public bool isKeyOnState( ControlKey key, ControlState state ) {
            bool oldState = this.oldKeyboardState.IsKeyDown( (Keys)key );
            bool newState = this.newKeyboardState.IsKeyDown( (Keys)key );
            
            if ( state == ControlState.PRESSED && !oldState && newState )
                return true;
            if ( state == ControlState.RELEASED && oldState && !newState )
                return true;
            if ( state == ControlState.DOWN && newState )
                return true;
            if ( state == ControlState.UP && !newState )
                return true;
            return false;
        }


        public bool isLeftMouseButtonPressed() {
            if ( newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released )
                return true;
            return false;
        }

        public bool isRightMouseButtonPressed() {
            if ( newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released )
                return true;
            return false;
        }

        public bool isLeftMouseButtonDown() {
            return newMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool isRightMouseButtonDown() {
            return newMouseState.RightButton == ButtonState.Pressed;
        }

        public Vector2 getMousePos() {
            return transformMouseCoords( new Vector2( newMouseState.X, newMouseState.Y ) );
        }


        public bool isMouseMoved() {
            return ( oldMouseState.X == newMouseState.X && oldMouseState.Y == newMouseState.Y ) ? false : true;
        }


        // depricated -> need adjustment for render coord transform
        // ----------------------------------------------------------
        //public Vector2 getMousePosChange() {
        //    int XChange = oldMouseState.X - newMouseState.X;
        //    int YChange = oldMouseState.Y - newMouseState.Y;
        //    return new Vector2( XChange, YChange );
        //}
        // ---------------------------------------------------------

        public int getMouseWheelChange() {
            return ( newMouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue );
        }

        /// <summary>
        /// get a list of control requests, depending on the configured key assignments
        /// </summary>
        /// <returns>a list of control requests</returns>
        public List<Control> getInput() {
            List<Control> controlSequence = new List<Control>();

            foreach ( KeyAssignment assignment in this.keyAssignments ) {
                if ( this.newKeyboardState.IsKeyDown( assignment.key ) ) {
                    controlSequence.Add( assignment.control );
                }
            }

            return controlSequence;
        }


        public void setMouseTransform( Vector2 screenSizeHalf, Vector2 renderSizeHalf, float renderScale ) {
            this.screenSizeHalf = screenSizeHalf;
            this.renderSizeHalf = renderSizeHalf;
            this.renderScale = renderScale;
        }

        public Vector2 transformMouseCoords( Vector2 input ) {
            Vector2 newVec = input;
            newVec -= screenSizeHalf;
            newVec /= renderScale;
            newVec += renderSizeHalf;

            return newVec;
        }

    }

}
