using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace HUD {

    /// <summary>
    /// a class to handle a set of items that will be updated
    /// </summary>
    public class UpdateController : IUpdateController {

        private List<IUpdatableItem> allUpdatable;

        private List<IUpdatableItem> registerList;

        private List<IUpdatableItem> unregisterList;


        /// <summary>
        /// create a new update controller
        /// </summary>
        public UpdateController() {
            this.allUpdatable = new List<IUpdatableItem>();
            this.registerList = new List<IUpdatableItem>();
            this.unregisterList = new List<IUpdatableItem>();
        }


        /// <summary>
        /// add an item, so it will from now on updated by this controller
        /// </summary>
        /// <param name="item">the item</param>
        public void Register( IUpdatableItem item ) {
            this.registerList.Add( item );
        }


        /// <summary>
        /// remove an item, so it will not longer updated by this controller
        /// </summary>
        /// <param name="item">the item</param>
        public void Unregister( IUpdatableItem item ) {
            this.unregisterList.Add( item );
        }


        /// <summary>
        /// update the controlled items
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

    }

}
