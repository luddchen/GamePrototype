using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antaris.Control.AI;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares;

namespace Battlestation_Antaris.Model.WorldObjects {

    class EnemyShip : TactileSpatialObject {

        private EnemyAI parent;

        public EnemyShip( String modelName, Vector3 position , EnemyAI parent ) : base( modelName, position: position ) {
            this.parent = parent;
            this.miniMapIcon.color = MiniMap.ENEMY_COLOR;
        }

        public override void OnDeath() {
            if ( this.parent != null ) {
                this.parent.Remove( this );
                this.parent = null;
            }
            Antares.world.Remove( this );
        }

    }

}
