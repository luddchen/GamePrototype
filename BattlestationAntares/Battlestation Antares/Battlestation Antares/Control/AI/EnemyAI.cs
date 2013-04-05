using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;
using Battlestation_Antares;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model.WorldObjects;

namespace Battlestation_Antaris.Control.AI {

    class EnemyAI {

        private List<EnemyShip> attackers;

        private int checkCountdown = 0;

        private SpatialObject root;


        public EnemyAI( SpatialObject root ) {
            this.root = root;
            this.attackers = new List<EnemyShip>();
            Antares.debugViewer.Add( new DebugElement( this, "Enemies", delegate( Object obj ) {
                return String.Format( "{0}", ( obj as EnemyAI ).attackers.Count );
            } ) );
        }

        public void Update( GameTime gameTime ) {
            if ( this.checkCountdown > 0 ) {
                this.checkCountdown--;
            }

            if ( this.checkCountdown == 0 ) {
                this.checkCountdown = 1200;

                if ( this.attackers.Count < 10 ) {
                    EnemyShip obj;

                    //if ( RandomGen.random.Next( 2 ) == 0 ) {
                        obj = new EnemyShip( "TargetShip", new Vector3( RandomGen.random.Next( 2400 ) - 1200, 0, RandomGen.random.Next( 2400 ) - 1200 ), this );
                        obj.attributes.Engine.CurrentVelocity = obj.attributes.Engine.MaxVelocity;
                        obj.globalPosition = this.root.globalPosition;
                        obj.rotation = this.root.rotation;
                    //} else {
                    //    obj = new EnemyShip( "Cubus", new Vector3( RandomGen.random.Next( 2400 ) - 1200, 0, RandomGen.random.Next( 2400 ) - 1200 ), this );
                    //}
                    this.attackers.Add( obj );
                }

            }
        }

        public void Remove( EnemyShip ship ) {
            this.attackers.Remove( ship );
        }

    }

}
