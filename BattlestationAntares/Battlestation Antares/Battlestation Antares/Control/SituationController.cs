using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Control
{

    /// <summary>
    /// abstract basis class for situation controller
    /// </summary>
    public class SituationController
    {

        /// <summary>
        /// the game
        /// </summary>
        public Antares game;


        /// <summary>
        /// the used view
        /// </summary>
        public View.View view;


        /// <summary>
        /// the order of world updates
        /// </summary>
        public WorldUpdate worldUpdate;


        /// <summary>
        /// create a new situation controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public SituationController(Antares game, View.View view)
        {
            this.game = game;
            this.view = view;
            this.worldUpdate = WorldUpdate.PRE;
        }


        /// <summary>
        /// update this situation controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public virtual void Update(GameTime gameTime)
        {
            this.view.ButtonUpdate();
        }

        /// <summary>
        /// called upon switching to this situation
        /// </summary>
        public virtual void onEnter()
        {

        }

        /// <summary>
        /// called upon leaving this situation
        /// </summary>
        public virtual void onExit()
        {

        }


    }

}
