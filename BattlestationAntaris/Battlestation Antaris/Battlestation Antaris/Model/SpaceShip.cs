using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Battlestation_Antaris.Control;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    public class SpaceShip : SpatialObject
    {

        public SpaceShip(Vector3 position, String modelName, ContentManager content) : base(position, modelName, content) 
        {
            init();
        }

        public SpaceShip(Vector3 position, String modelName, ContentManager content, WorldModel world) : base(position, modelName, content, world) 
        {
            init();
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }


        private void init()
        {
            this.attributes.Engine.Acceleration = 0.01f;
            this.attributes.Engine.MaxVelocity = 1.0f;

            this.attributes.EngineRoll.Acceleration = (float)(Math.PI / 14400);
            this.attributes.EngineRoll.MaxVelocity = (float)(Math.PI / 360);

            this.attributes.EnginePitch.Acceleration = (float)(Math.PI / 14400);
            this.attributes.EnginePitch.MaxVelocity = (float)(Math.PI / 360);

            this.attributes.EngineYaw.Acceleration = (float)(Math.PI / 14400);
            this.attributes.EngineYaw.MaxVelocity = (float)(Math.PI / 360);
        }

    }

}
