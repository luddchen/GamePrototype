using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Battlestation_Antaris;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// abstract basis class for situation controller
    /// </summary>
    public class SituationController {

        /// <summary>
        /// the game
        /// </summary>
        protected Antares game;


        /// <summary>
        /// the used view
        /// </summary>
        public View.View view;


        /// <summary>
        /// the order of world updates
        /// </summary>
        public WorldUpdate worldUpdate;


        private List<IUpdatableItem> allUpdatable;
        private List<IUpdatableItem> allUpdatableAddList;
        private List<IUpdatableItem> allUpdatableRemoveList;



        /// <summary>
        /// create a new situation controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public SituationController( Antares game, View.View view ) {
            this.game = game;
            this.view = view;
            this.worldUpdate = WorldUpdate.PRE;
            this.allUpdatable = new List<IUpdatableItem>();
            this.allUpdatableAddList = new List<IUpdatableItem>();
            this.allUpdatableRemoveList = new List<IUpdatableItem>();
        }


        /// <summary>
        /// update this situation controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public virtual void Update( GameTime gameTime ) {
            // remove updatable items
            foreach ( IUpdatableItem item in this.allUpdatableRemoveList ) {
                this.allUpdatable.Remove( item );
            }
            this.allUpdatableRemoveList.Clear();

            // add updatable items
            foreach ( IUpdatableItem item in this.allUpdatableAddList ) {
                this.allUpdatable.Add( item );
            }
            this.allUpdatableAddList.Clear();

            // update items
            foreach ( IUpdatableItem item in this.allUpdatable ) {
                item.Update( gameTime );
            }

            this.view.ButtonUpdate();
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
