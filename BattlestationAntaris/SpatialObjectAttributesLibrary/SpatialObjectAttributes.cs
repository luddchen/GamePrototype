using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary
{

    public class SpatialObjectAttributes
    {

        // Shield
        public Health Shield;


        // Hull
        public Health Hull;


        // Radar
        public float RadarRange;


        // Engine
        public Engine Engine;

        public Engine EngineRoll;

        public Engine EngineYaw;

        public Engine EnginePitch;
        

        // Laser
        public Laser Laser;


        // Missile
        public Missile Missile;


        public SpatialObjectAttributes()
        {
            this.Shield = new Health();
            this.Hull = new Health();

            this.Engine = new Engine();
            this.EnginePitch = new Engine();
            this.EngineRoll = new Engine();
            this.EngineYaw = new Engine();

            this.Laser = new Laser();
            this.Missile = new Missile();
        }

    }

}
