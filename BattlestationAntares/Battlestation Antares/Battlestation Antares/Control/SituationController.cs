using HUD;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// abstract basis class for situation controller
    /// </summary>
    public class SituationController : UpdateController {

        /// <summary>
        /// the game
        /// </summary>
        protected Antares game;


        /// <summary>
        /// the used view
        /// </summary>
        public HUDView view;


        /// <summary>
        /// create a new situation controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public SituationController( Antares game, HUDView view ) : base() {
            this.game = game;
            this.view = view;
        }


        /// <summary>
        /// called upon switching to this situation
        /// </summary>
        public virtual void onEnter() {
        }

        /// <summary>
        /// called upon leaving this situation
        /// </summary>
        public virtual void onExit() {
        }

    }

}
