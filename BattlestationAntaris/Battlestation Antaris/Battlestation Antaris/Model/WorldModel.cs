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


        /// <summary>
        /// loader for shield model
        /// </summary>
        public SpatialObject Shield;


        private List<SpatialObject> removeList;


        private Tools.DynamicOctree<SpatialObject> treeTest;


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

            // octree test
            this.treeTest = new Tools.DynamicOctree<SpatialObject>(3, 1, 10, new BoundingBox(new Vector3(-5000, -5000, -5000), new Vector3(5000,5000,5000)));
        }


        /// <summary>
        /// initialize the world content
        /// </summary>
        /// <param name="content">the game content manager</param>
        public void Initialize(ContentManager content)
        {

            // create 500 random objects to fill the space for testing
            Random random = new Random();

            //for (int i = 0; i < 40; i++ )
            //{
            //    if (random.Next(2) == 0)
            //    {
            //        new SpatialObject(new Vector3(random.Next(2400) - 1200, 0, random.Next(2400) - 1200), "Models/TargetShip//targetship_2", content, this);
            //    }
            //    else
            //    {
            //        new SpatialObject(new Vector3(random.Next(2400) - 1200, 0, random.Next(2400) - 1200), "Models/SpaceShip//spaceship_2", content, this);
            //    }
            //}

            //for (int i = 0; i < 12; i++)
            //{
            //    this.allTurrets.Add(
            //        new Turret(
            //            new Vector3(random.Next(2400) - 1200, 0, random.Next(2400) - 1200), 
            //            content, 
            //            this));
            //}

            //for (int i = 0; i < 32; i++)
            //{
            //    this.allRadars.Add(
            //        new Radar(
            //            new Vector3(random.Next(2400) - 1200, 0, random.Next(2400) - 1200),
            //            content,
            //            this));
            //}


            // create the player space ship
            this.spaceShip = new SpaceShip(new Vector3(0,30,500), "Models/compass2", content, this);


            // create the player space station
            this.spaceStation = new SpaceStation(Vector3.Zero, "Models/SpaceStation/spacestation", content, this);

            this.Shield = new SpatialObject(Vector3.Zero, "Models//shield", content, this);
            this.Shield.isVisible = false;
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


            // octree test
            //treeTest.Clear();
            //foreach (SpatialObject obj in this.allObjects)
            //{
            //    if (obj.isVisible)
            //    {
            //        BoundingSphere itemSphere = new BoundingSphere(obj.bounding.Center + obj.globalPosition, obj.bounding.Radius);
            //        if (!treeTest.Add(obj, itemSphere))
            //        {
            //            treeTest.AddItem(obj, itemSphere);
            //        }
            //    }
            //}

            //foreach (SpatialObject obj in this.allWeapons)
            //{
            //    if (obj.isVisible)
            //    {
            //        BoundingSphere itemSphere = new BoundingSphere(obj.bounding.Center + obj.globalPosition, obj.bounding.Radius);
            //        if (!treeTest.Add(obj, itemSphere))
            //        {
            //            treeTest.AddItem(obj, itemSphere);
            //        }
            //    }
            //}

            //treeTest.BuildTree();
            //Console.Out.WriteLine(treeTest.Count);
            //Console.Out.WriteLine(treeTest.getCountString());
            //Console.Out.WriteLine();
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
