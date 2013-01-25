using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    public class WorldModel : Model
    {

        public List<SpatialObject> allObjects;

        public WorldModel()
        {
            this.allObjects = new List<SpatialObject>();
        }

        public override void Initialize(ContentManager content)
        {
            for (int x = -2; x < 3; x++)
            {
                    for (int z = -2; z < 3; z++)
                    {
                        this.allObjects.Add( new SpatialObject(new Vector3(x * 3, 30, z * 3), "Models/compass", content));
                    }
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
        }

    }

}
