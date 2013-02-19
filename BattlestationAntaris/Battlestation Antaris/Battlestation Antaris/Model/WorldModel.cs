using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// represents a model of the game world
    /// </summary>
    public class WorldModel
    {

        /// <summary>
        /// the game
        /// </summary>
        public Game1 game;


        /// <summary>
        /// the players space ship
        /// </summary>
        public SpaceShip spaceShip;


        /// <summary>
        /// the players space station
        /// </summary>
        public SpaceStation spaceStation;


        /// <summary>
        /// a list of all player radars
        /// </summary>
        public List<Radar> allRadars;


        /// <summary>
        /// a list of all player turrets
        /// </summary>
        public List<Turret> allTurrets;


        /// <summary>
        /// a list of all spatial objects
        /// </summary>
        public List<SpatialObject> allObjects;


        /// <summary>
        /// creates the world
        /// </summary>
        /// <param name="game">the game</param>
        public WorldModel(Game1 game)
        {
            this.game = game;
            this.allObjects = new List<SpatialObject>();
            this.allRadars = new List<Radar>();
            this.allTurrets = new List<Turret>();
        }


        /// <summary>
        /// initialize the world content
        /// </summary>
        /// <param name="content">the game content manager</param>
        public void Initialize(ContentManager content)
        {

            // create 500 random objects to fill the space for testing
            Random random = new Random();

            for (int i = 0; i < 500; i++ )
            {
                if (random.Next(2) == 0)
                {
                    this.allObjects.Add(new SpatialObject(new Vector3(random.Next(600) - 300, random.Next(600) - 300, random.Next(600) - 300), "Models/compass", content));
                }
                else
                {
                    this.allObjects.Add(new SpatialObject(new Vector3(random.Next(600) - 300, random.Next(600) - 300, random.Next(600) - 300), "Models/spaceship_d3", content));
                }
            }


            // create the player space ship
            this.spaceShip = new SpaceShip(new Vector3(0,30,500), "Models/compass", content);
            this.allObjects.Add(this.spaceShip);


            // create the player space station
            this.spaceStation = new SpaceStation(Vector3.Zero, "Models/SpaceStation/spacestation", content);
            this.allObjects.Add(this.spaceStation);
        }


        /// <summary>
        /// update the world
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            // update all spatial objects
            foreach (SpatialObject obj in this.allObjects)
            {
                obj.Update(gameTime);
            }
        }

    }

}
