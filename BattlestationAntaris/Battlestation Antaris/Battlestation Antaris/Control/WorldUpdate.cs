namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// enumeration of update orders
    /// </summary>
    public enum WorldUpdate
    {

        /// <summary>
        /// update the world as first
        /// </summary>
        PRE, 

        /// <summary>
        /// update the world as last
        /// </summary>
        POST,

        /// <summary>
        /// dont update the world
        /// </summary>
        NO_UPDATE
    }

}
