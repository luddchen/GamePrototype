using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpatialObjectAttributesLibrary {

    /// <summary>
    /// Spatial Object Attributes
    /// </summary>
    public class SpatialObjectAttributes {

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


        public SpatialObjectAttributes() {
            this.Shield = new Health();
            this.Hull = new Health();

            this.Radar = new Radar();

            this.Engine = new Engine();

            this.EngineYaw = new Engine();
            this.EnginePitch = new Engine();
            this.EngineRoll = new Engine();

            this.Laser = new Laser();
            this.Missile = new Missile();

            setNames();
        }

        public SpatialObjectAttributes( SpatialObjectAttributes soa ) {
            this.Shield = new Health( soa.Shield );
            this.Hull = new Health( soa.Hull );

            this.Radar = new Radar( soa.Radar );

            this.Engine = new Engine( soa.Engine );

            this.EngineYaw = new Engine( soa.EngineYaw );
            this.EnginePitch = new Engine( soa.EnginePitch );
            this.EngineRoll = new Engine( soa.EngineRoll );

            this.Laser = new Laser( soa.Laser );
            this.Missile = new Missile( soa.Missile );

            setNames();
        }

        public List<AttributeItem> getItems() {
            List<AttributeItem> items = new List<AttributeItem>();
            items.Add( this.Shield );
            items.Add( this.Hull );
            items.Add( this.Radar );
            items.Add( this.Engine );
            items.Add( this.EngineYaw );
            items.Add( this.EnginePitch );
            items.Add( this.EngineRoll );
            items.Add( this.Laser );
            items.Add( this.Missile );

            return items;
        }

        public override string ToString() {
            String output = "SpatialObjectAttributes\n=======================\n";
            foreach ( AttributeItem item in getItems() ) {
                output += item.ToString();
            }
            output += "\n";

            return output;
        }

        private void setNames() {
            this.Shield.name = "Shield";
            this.Hull.name = "Hull";
            this.EngineYaw.name += "(Yaw)";
            this.EnginePitch.name += "(Pitch)";
            this.EngineRoll.name += "(Roll)";
        }

    }

}
