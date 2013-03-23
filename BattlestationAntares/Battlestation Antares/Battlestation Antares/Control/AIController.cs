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

        public AI_Container aiContainer;

        public AIController( Antares game, View.View view )
            : base( game, view ) {

            this.aiContainer = new AI_Container(this);
            this.view.Add( this.aiContainer );

            HUDButton toMenuButton = new HUDButton( "  ", new Vector2( 0.95f, 0.95f ), 1.3f, this );
            toMenuButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.MENU );
            } );
            toMenuButton.positionType = HUDType.RELATIV;
            toMenuButton.style = ButtonStyle.BuilderButtonStyle();
            toMenuButton.SetBackgroundTexture( "Sprites//HUD//Ship" );
            this.view.Add( toMenuButton );
            this.worldUpdate = WorldUpdate.NO_UPDATE;


            HUDArray verifyArray = new HUDArray( new Vector2( 0.9f, 0.6f ), HUDType.RELATIV, new Vector2( 0.15f, 0.1f ), HUDType.RELATIV );
            verifyArray.direction = LayoutDirection.HORIZONTAL;
            this.view.Add( verifyArray );

            HUDButton verifyButton = new HUDButton( "Verify", new Vector2(), 0.8f, this );
            verifyButton.SetPressedAction(
                delegate() {
                    AI.AI ai = new AI.AI();
                    ai.Create( this.aiContainer );
                    Console.WriteLine( ai );

                    foreach ( Turret turret in Antares.world.allTurrets ) {
                        turret.ai = new AI.AI( ai );
                        turret.ai.source = turret;
                    }
                } );
            verifyButton.style = ButtonStyle.BuilderButtonStyle();
            verifyButton.SetBackgroundTexture( "Sprites//builder_button_round" );
            verifyArray.Add( verifyButton );

            HUDArray statusArray = new HUDArray( new Vector2(), HUDType.RELATIV, new Vector2(), HUDType.RELATIV );
            statusArray.direction = LayoutDirection.VERTICAL;
            verifyArray.Add( statusArray );

            statusArray.Add( new HUDString( "Status", null, null, null, null, 0.6f, 0 ) );
            statusArray.Add( new HUDString( "unknown", null, null, Color.Yellow, null, 0.6f, 0 ) );


            HUDArray aiButtonArray = new HUDArray( new Vector2( 0.9f, 0.8f ), HUDType.RELATIV, new Vector2( 0.1f, 0.15f ), HUDType.RELATIV );
            aiButtonArray.direction = LayoutDirection.VERTICAL;
            this.view.Add( aiButtonArray );

            HUDButton saveButton = new HUDButton( "Save", new Vector2(), 0.6f, this );
            saveButton.SetPressedAction(
                delegate() {
                    AI_XML.WriteAIContainer( "testAI.xml", this.aiContainer );
                } );
            saveButton.style = ButtonStyle.BuilderButtonStyle();
            saveButton.SetBackgroundTexture( "Sprites//builder_button" );
            aiButtonArray.Add( saveButton );

            HUDButton loadButton = new HUDButton( "Load", new Vector2(), 0.6f, this );
            loadButton.SetPressedAction(
                delegate() {
                    AI_XML.ReadAIContainer( "testAI.xml", this.aiContainer, this );
                } );
            loadButton.style = ButtonStyle.BuilderButtonStyle();
            loadButton.SetBackgroundTexture( "Sprites//builder_button" );
            aiButtonArray.Add( loadButton );

            HUDButton clearButton = new HUDButton( "Clear", new Vector2(), 0.6f, this );
            clearButton.SetPressedAction(
                delegate() {
                    this.aiContainer.ClearAI();
                } );
            clearButton.style = ButtonStyle.BuilderButtonStyle();
            clearButton.SetBackgroundTexture( "Sprites//builder_button" );
            aiButtonArray.Add( clearButton );

        }

    }

}
