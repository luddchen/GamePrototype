using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View;
using Battlestation_Antares.View.HUD.AIComposer;
using Battlestation_Antares.Control.AI;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.Control {

    public class AIController : SituationController {

        public AIController( Antares game, View.View view )
            : base( game, view ) {
            HUD2DButton toMenuButton = new HUD2DButton( "Menu", new Vector2( 0.9f, 0.95f ), 0.7f );
            toMenuButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.MENU );
            } );
            toMenuButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add( toMenuButton );
            this.worldUpdate = WorldUpdate.NO_UPDATE;

            HUD2DButton verifyButton = new HUD2DButton( "Verify", new Vector2( 0.9f, 0.8f ), 0.8f );
            verifyButton.SetPressedAction(
                delegate() {
                    AI.AI ai = new AI.AI();
                    ai.Create( ( (AIView)this.view ).aiContainer );
                    Console.WriteLine( ai );

                    foreach ( Turret turret in Antares.world.allTurrets ) {
                        turret.ai = new AI.AI( ai );
                        turret.ai.source = turret;
                    }
                } );
            verifyButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add( verifyButton );

            HUD2DButton saveButton = new HUD2DButton( "Save", new Vector2( 0.85f, 0.88f ), 0.6f );
            saveButton.SetPressedAction(
                delegate() {
                    AI_XML.WriteAIContainer( "testAI.xml", ( (View.AIView)this.view ).aiContainer );
                } );
            saveButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add( saveButton );

            HUD2DButton loadButton = new HUD2DButton( "Load", new Vector2( 0.92f, 0.88f ), 0.6f );
            loadButton.SetPressedAction(
                delegate() {
                    AI_XML.ReadAIContainer( "testAI.xml", ( (View.AIView)this.view ).aiContainer );
                } );
            loadButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add( loadButton );

        }

    }

}
