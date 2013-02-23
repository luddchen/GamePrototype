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
        /// a list of all Laser beams
        /// </summary>
        public List<SpatialObject> allWeapons;


        private List<SpatialObject> removeList;


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

            this.allWeapons = new List<SpatialObject>();
            this.removeList = new List<SpatialObject>();
        }


        /// <summary>
        /// initialize the world content
        /// </summary>
        /// <param name="content">the game content manager</param>
        public void Initialize(ContentManager content)
        {

            // create 500 random objects to fill the space for testing
            Random random = new Random();

            for (int i = 0; i < 200; i++ )
            {
                if (random.Next(2) == 0)
                {
                    new SpatialObject(new Vector3(random.Next(2400) - 1200, random.Next(2400) - 1200, random.Next(2400) - 1200), "Models/compass", content, this);
                }
                else
                {
                    new SpatialObject(new Vector3(random.Next(2400) - 1200, random.Next(2400) - 1200, random.Next(2400) - 1200), "Models/spaceship_d3", content, this);
                }
            }

            for (int i = 0; i < 12; i++)
            {
                this.allTurrets.Add(
                    new Turret(
                        new Vector3(random.Next(2400) - 1200, 1, random.Next(2400) - 1200), 
                        content, 
                        this));
            }

            for (int i = 0; i < 12; i++)
            {
                this.allRadars.Add(
                    new Radar(
                        new Vector3(random.Next(2400) - 1200, 1, random.Next(2400) - 1200),
                        content,
                        this));
            }


            // create the player space ship
            this.spaceShip = new SpaceShip(new Vector3(0,30,500), "Models/compass", content, this);


            // create the player space station
            this.spaceStation = new SpaceStation(Vector3.Zero, "Models/SpaceStation/spacestation", content, this);
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

            // update all laser beams
            foreach (SpatialObject obj in this.allWeapons)
            {
                obj.Update(gameTime);
            }

            foreach (SpatialObject obj in this.removeList)
            {
                if ((obj is Laser) || (obj is Missile))
                {
                    this.allWeapons.Remove(obj);
                }
                else
                {
                    this.allObjects.Remove(obj);
                }
            }
            this.removeList.Clear();
        }


        /// <summary>
        /// add a spatial object to this world
        /// </summary>
        /// <param name="obj">a spatial object</param>
        public void addObject(SpatialObject obj)
        {
            if ((obj is Laser) || (obj is Missile))
            {
                this.allWeapons.Add(obj);
            }
            else
            {
                this.allObjects.Add(obj);
            }
        }


        /// <summary>
        /// trigger object remove
        /// </summary>
        /// <param name="laser">laser to remove</param>
        public void removeObject(SpatialObject obj)
        {
            this.removeList.Add(obj);
        }

    }

}
