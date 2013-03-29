namespace Battlestation_Antares.Control {

    /// <summary>
    /// enumeration of game situations
    /// </summary>
    public enum Situation {

        /// <summary>
        /// the cockpit sitation
        /// </summary>
        COCKPIT = 0,

        /// <summary>
        /// the command situation
        /// </summary>
        COMMAND = 1,

        /// <summary>
        /// the menu situation
        /// </summary>
        MENU = 2,

        /// <summary>
        /// the ai-builder/editor situation
        /// </summary>
        AI_BUILDER = 3,

        /// <summary>
        /// the situation while dock / undock
        /// </summary>
        DOCK = 4
    }

}
