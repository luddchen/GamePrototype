﻿using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control
{

    /// <summary>
    /// abstract basis class for situation controller
    /// </summary>
    abstract class SituationController
    {

        /// <summary>
        /// the game
        /// </summary>
        public Game1 game;


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
        public SituationController(Game1 game, View.View view)
        {
            this.game = game;
            this.view = view;
            this.worldUpdate = WorldUpdate.PRE;
        }


        /// <summary>
        /// update this situation controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public abstract void Update(GameTime gameTime);

    }

}
