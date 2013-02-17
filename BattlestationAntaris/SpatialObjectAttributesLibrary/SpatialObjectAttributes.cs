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
        public Weapon Laser;

        public float LaserHeatProduction;

        public float LaserHeatUntilCooldown;

        public float LaserCurrentHeat;

        public float LaserHeatRegeneration;


        // Missile
        public Weapon Missile;

        public float MissileMaxAmount;

        public float MissileCurrentAmount;

        public float MissileReloadTime;

        public float MissileCurrentReloadTime;


        public SpatialObjectAttributes()
        {
            this.Shield = new Health();
            this.Hull = new Health();

            this.Engine = new Engine();
            this.EnginePitch = new Engine();
            this.EngineRoll = new Engine();
            this.EngineYaw = new Engine();

            this.Laser = new Weapon();
            this.LaserCurrentHeat = 0;
            this.LaserHeatProduction = 0;
            this.LaserHeatRegeneration = 0;
            this.LaserHeatUntilCooldown = 0;

            this.Missile = new Weapon();
            this.MissileCurrentAmount = 0;
            this.MissileCurrentReloadTime = 0;
            this.MissileMaxAmount = 0;
            this.MissileReloadTime = 0;
        }

    }

}
