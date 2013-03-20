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
            HUD2DButton toMenuButton = new HUD2DButton( "  ", new Vector2( 0.95f, 0.95f ), 1.3f );
            toMenuButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.MENU );
            } );
            toMenuButton.positionType = HUDType.RELATIV;
            toMenuButton.style = ButtonStyle.BuilderButtonStyle();
            toMenuButton.SetBackgroundTexture( "Sprites//HUD//Ship" );
            this.view.allHUD_2D.Add( toMenuButton );
            this.worldUpdate = WorldUpdate.NO_UPDATE;


            HUD2DArray verifyArray = new HUD2DArray( new Vector2( 0.9f, 0.6f ), HUDType.RELATIV, new Vector2( 0.15f, 0.1f ), HUDType.RELATIV );
            verifyArray.direction = LayoutDirection.HORIZONTAL;
            this.view.allHUD_2D.Add( verifyArray );

            HUD2DButton verifyButton = new HUD2DButton( "Verify", new Vector2(), 0.8f );
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
            verifyButton.style = ButtonStyle.BuilderButtonStyle();
            verifyButton.SetBackgroundTexture( "Sprites//builder_button_round" );
            verifyArray.Add( verifyButton );

            HUD2DArray statusArray = new HUD2DArray( new Vector2(), HUDType.RELATIV, new Vector2(), HUDType.RELATIV );
            statusArray.direction = LayoutDirection.VERTICAL;
            verifyArray.Add( statusArray );

            statusArray.Add( new HUD2DString( "Status", null, null, null, null, 0.6f, 0 ) );
            statusArray.Add( new HUD2DString( "unknown", null, null, Color.Yellow, null, 0.6f, 0 ) );


            HUD2DArray aiButtonArray = new HUD2DArray( new Vector2( 0.9f, 0.8f ), HUDType.RELATIV, new Vector2( 0.1f, 0.15f ), HUDType.RELATIV );
            aiButtonArray.direction = LayoutDirection.VERTICAL;
            this.view.allHUD_2D.Add( aiButtonArray );

            HUD2DButton saveButton = new HUD2DButton( "Save", new Vector2(), 0.6f );
            saveButton.SetPressedAction(
                delegate() {
                    AI_XML.WriteAIContainer( "testAI.xml", ( (View.AIView)this.view ).aiContainer );
                } );
            saveButton.style = ButtonStyle.BuilderButtonStyle();
            saveButton.SetBackgroundTexture( "Sprites//builder_button" );
            aiButtonArray.Add( saveButton );

            HUD2DButton loadButton = new HUD2DButton( "Load", new Vector2(), 0.6f );
            loadButton.SetPressedAction(
                delegate() {
                    AI_XML.ReadAIContainer( "testAI.xml", ( (View.AIView)this.view ).aiContainer );
                } );
            loadButton.style = ButtonStyle.BuilderButtonStyle();
            loadButton.SetBackgroundTexture( "Sprites//builder_button" );
            aiButtonArray.Add( loadButton );

            HUD2DButton clearButton = new HUD2DButton( "Clear", new Vector2(), 0.6f );
            clearButton.SetPressedAction(
                delegate() {
                    ( (View.AIView)this.view ).aiContainer.ClearAI();
                } );
            clearButton.style = ButtonStyle.BuilderButtonStyle();
            clearButton.SetBackgroundTexture( "Sprites//builder_button" );
            aiButtonArray.Add( clearButton );

        }

    }

}
