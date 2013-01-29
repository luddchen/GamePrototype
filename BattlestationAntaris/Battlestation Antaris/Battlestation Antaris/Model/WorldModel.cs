﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Control;

namespace Battlestation_Antaris.Model
{

    public class WorldModel : Model
    {

        public List<SpatialObject> allObjects;

        public WorldModel(Controller controller) : base(controller)
        {
            this.allObjects = new List<SpatialObject>();
        }

        public override void Initialize(ContentManager content)
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
                    this.allObjects.Add(new SpatialObject(new Vector3(random.Next(600) - 300, random.Next(600) - 300, random.Next(600) - 300), "Models/spaceship_d1", content));
                }
            }

            this.allObjects.Add(new SpatialObject(new Vector3(0, 0, 0), "Models/battlestation", content));
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            foreach (SpatialObject obj in this.allObjects)
            {
                obj.Update(gameTime);
            }
        }

    }

}
