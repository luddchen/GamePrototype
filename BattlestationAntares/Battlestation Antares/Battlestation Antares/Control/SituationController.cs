using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Battlestation_Antaris;
using System;
using Battlestation_Antares.Tools;

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


        private List<IUpdatableItem> allUpdatable;
        private List<IUpdatableItem> registerList;
        private List<IUpdatableItem> unregisterList;


        /// <summary>
        /// create a new situation controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public SituationController( Antares game, View.View view ) {
            this.game = game;
            this.view = view;
            this.allUpdatable = new List<IUpdatableItem>();
            this.registerList = new List<IUpdatableItem>();
            this.unregisterList = new List<IUpdatableItem>();

            Antares.debugViewer.Add( new DebugElement( this, this.GetType().Name + " Updatable",
                delegate( Object obj ) {
                    return String.Format( "{0}", ( obj as SituationController ).allUpdatable.Count );
                } ) );
        }


        /// <summary>
        /// update this situation controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public virtual void Update( GameTime gameTime ) {
            // remove updatable items
            foreach ( IUpdatableItem item in this.unregisterList ) {
                this.allUpdatable.Remove( item );
            }
            this.unregisterList.Clear();

            // add updatable items
            foreach ( IUpdatableItem item in this.registerList ) {
                this.allUpdatable.Add( item );
            }
            this.registerList.Clear();

            // update items
            foreach ( IUpdatableItem item in this.allUpdatable ) {
                if ( item.Enabled ) {
                    item.Update( gameTime );
                }
            }
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


        public void Register( IUpdatableItem item ) {
            this.registerList.Add( item );
        }


        public void Unregister( IUpdatableItem item ) {
            this.unregisterList.Add( item );
        }


    }

}
