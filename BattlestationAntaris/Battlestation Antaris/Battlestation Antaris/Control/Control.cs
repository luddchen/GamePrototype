namespace Battlestation_Antaris.Control
{
    /// <summary>
    /// enumeration of control requests
    /// </summary>
    public enum Control
    {

        /// <summary>
        /// control request : pitch down
        /// </summary>
        PITCH_DOWN,

        /// <summary>
        /// control request : pitch up
        /// </summary>
        PITCH_UP,

        /// <summary>
        /// control request : yaw left
        /// </summary>
        YAW_LEFT,

        /// <summary>
        /// control request : yaw right
        /// </summary>
        YAW_RIGHT,

        /// <summary>
        /// control request : roll anticlockwise
        /// </summary>
        ROLL_ANTICLOCKWISE,

        /// <summary>
        /// control request : roll clockwise
        /// </summary>
        ROLL_CLOCKWISE,

        /// <summary>
        /// control request : increase throttle
        /// </summary>
        INCREASE_THROTTLE,

        /// <summary>
        /// control request : decrease throttle
        /// </summary>
        DECREASE_THROTTLE,

        /// <summary>
        /// control request : zero throttle
        /// </summary>
        ZERO_THROTTLE,

        /// <summary>
        /// control request : fire laser
        /// </summary>
        FIRE_LASER,

        /// <summary>
        /// control request : fire missile
        /// </summary>
        FIRE_MISSILE,

        /// <summary>
        /// control request : target next enemy
        /// </summary>
        TARGET_NEXT_ENEMY,

        /// <summary>
        /// control request : target space station
        /// </summary>
        TARGET_STATION,

        /// <summary>
        /// control request : zoom radar map in
        /// </summary>
        ZOOM_RADAR_IN,

        /// <summary>
        /// control request : zoom radar map out
        /// </summary>
        ZOOM_RADAR_OUT

    }

}
