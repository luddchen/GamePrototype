using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.Model
{

    public class WorldModel
    {
        public Game1 game;

        public SpaceShip spaceShip;
        public SpaceStation spaceStation;
        public List<Radar> allRadars;
        public List<Turret> allTurrets;

        public List<SpatialObject> allObjects;

        public WorldModel(Game1 game)
        {
            this.game = game;
            this.allObjects = new List<SpatialObject>();
            this.allRadars = new List<Radar>();
            this.allTurrets = new List<Turret>();
        }

        public void Initialize(ContentManager content)
        {
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

            this.spaceShip = new SpaceShip(new Vector3(0,30,500), "Models/compass", content);
            this.allObjects.Add(this.spaceShip);

            this.spaceStation = new SpaceStation(Vector3.Zero, "Models/SpaceStation/spacestation", content);
            this.allObjects.Add(this.spaceStation);
        }

        public void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (SpatialObject obj in this.allObjects)
            {
                obj.Update(gameTime);
            }
        }

    }

}
