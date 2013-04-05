using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antaris.Control.AI;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares;
using Battlestation_Antares.Tools;
using Battlestation_Antares.Control;

namespace Battlestation_Antaris.Model.WorldObjects {

    class EnemyShip : TactileSpatialObject {

        private EnemyAI parent;

        public EnemyShip( String modelName, Vector3 position , EnemyAI parent ) : base( modelName, position: position ) {
            this.parent = parent;
            this.miniMapIcon.color = MiniMap.ENEMY_COLOR;
            this.objectType = ObjectType.ENEMY;
        }

        public override void Update( GameTime gameTime ) {
            base.Update( gameTime );

            Vector3 rot = Tools.GetRotation( Antares.world.spaceShip.globalPosition - this.globalPosition, this.rotation );

            if ( rot.Z < this.attributes.EngineYaw.CurrentVelocity ) {
                InjectControl( Command.YAW_RIGHT );
            }

            if ( rot.Z > this.attributes.EngineYaw.CurrentVelocity ) {
                InjectControl( Command.YAW_LEFT );
            }

            if ( rot.X < this.attributes.EnginePitch.CurrentVelocity ) {
                InjectControl( Command.PITCH_DOWN );
            }

            if ( rot.X > this.attributes.EnginePitch.CurrentVelocity ) {
                InjectControl( Command.PITCH_UP );
            }
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
