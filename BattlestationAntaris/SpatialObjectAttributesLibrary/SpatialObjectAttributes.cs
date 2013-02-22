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
        public Radar Radar;


        // Engine
        public Engine Engine;

        public Engine EngineYaw;

        public Engine EnginePitch;

        public Engine EngineRoll;
        

        // Laser
        public Laser Laser;


        // Missile
        public Missile Missile;


        public SpatialObjectAttributes()
        {
            this.Shield = new Health();
            this.Hull = new Health();

            this.Radar = new Radar();

            this.Engine = new Engine();

            this.EngineYaw = new Engine();
            this.EnginePitch = new Engine();
            this.EngineRoll = new Engine();

            this.Laser = new Laser();
            this.Missile = new Missile();
        }

        public void set(SpatialObjectAttributes soa)
        {
            this.Shield.set(soa.Shield);
            this.Hull.set(soa.Hull);
            this.Radar.set(soa.Radar);
            this.Engine.set(soa.Engine);
            this.EngineYaw.set(soa.EngineYaw);
            this.EnginePitch.set(soa.EnginePitch);
            this.EngineRoll.set(soa.EngineRoll);
            this.Laser.set(soa.Laser);
            this.Missile.set(soa.Missile);
        }

        public List<AttributeItem> getItems()
        {
            List<AttributeItem> items = new List<AttributeItem>();
            items.Add(this.Shield);
            items.Add(this.Hull);
            items.Add(this.Radar);
            items.Add(this.Engine);
            items.Add(this.EngineYaw);
            items.Add(this.EnginePitch);
            items.Add(this.EngineRoll);
            items.Add(this.Laser);
            items.Add(this.Missile);

            return items;
        }

    }

}
