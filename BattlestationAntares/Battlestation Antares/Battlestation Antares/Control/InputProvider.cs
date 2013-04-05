using System.Collections.Generic;
using HUD;
using Microsoft.Xna.Framework.Input;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// a class that collects and provides user/player input
    /// </summary>
    public class InputProvider : MouseProvider {

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
        public InputProvider() {
            keyAssignments = new List<KeyAssignment>();

            // create some key assignments
            keyAssignments.Add( new KeyAssignment( Command.PITCH_UP, Keys.Up ) );
            keyAssignments.Add( new KeyAssignment( Command.PITCH_DOWN, Keys.Down ) );
            keyAssignments.Add( new KeyAssignment( Command.ROLL_ANTICLOCKWISE, Keys.A ) );
            keyAssignments.Add( new KeyAssignment( Command.ROLL_CLOCKWISE, Keys.D ) );
            keyAssignments.Add( new KeyAssignment( Command.YAW_LEFT, Keys.Left ) );
            keyAssignments.Add( new KeyAssignment( Command.YAW_RIGHT, Keys.Right ) );
            keyAssignments.Add( new KeyAssignment( Command.INCREASE_THROTTLE, Keys.W ) );
            keyAssignments.Add( new KeyAssignment( Command.DECREASE_THROTTLE, Keys.S ) );

            keyAssignments.Add( new KeyAssignment( Command.FIRE_LASER, Keys.Space ) );
            keyAssignments.Add( new KeyAssignment( Command.FIRE_MISSILE, Keys.LeftControl ) );
            keyAssignments.Add( new KeyAssignment( Command.TARGET_NEXT_ENEMY, Keys.Tab ) );
        }


        /// <summary>
        /// update the input provider
        /// </summary>
        public override void Update() {
            this.oldKeyboardState = this.newKeyboardState;
            this.newKeyboardState = Keyboard.GetState();
            base.Update();
        }


        /// <summary>
        /// get a list of control requests, depending on the configured key assignments
        /// </summary>
        /// <returns>a list of control requests</returns>
        public List<Command> getInput() {
            List<Command> controlSequence = new List<Command>();

            foreach ( KeyAssignment assignment in this.keyAssignments ) {
                if ( this.newKeyboardState.IsKeyDown( assignment.key ) ) {
                    controlSequence.Add( assignment.control );
                }
            }

            return controlSequence;
        }

    }

}
