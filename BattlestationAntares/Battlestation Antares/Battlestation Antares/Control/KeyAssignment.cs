using Microsoft.Xna.Framework.Input;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// an assignment of a key to a control request 
    /// </summary>
    public class KeyAssignment {

        /// <summary>
        /// the control request
        /// </summary>
        public Control control;


        /// <summary>
        /// the assigned key
        /// </summary>
        public Keys key;


        /// <summary>
        /// create a new key assignment
        /// </summary>
        /// <param name="control">the control request</param>
        /// <param name="key">the assigned key</param>
        public KeyAssignment( Control control, Keys key ) {
            this.control = control;
            this.key = key;
        }

    }

}
