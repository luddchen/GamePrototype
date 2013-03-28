using System;
using Battlestation_Antares.Control.AI;
using Battlestation_Antares.Model;
using Battlestation_Antares.View.HUD.AIComposer;
using HUD;
using HUD.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.Control {

    public class AIController : SituationController {

        public AI_Container aiContainer;

        public AIController( Antares game, HUDView view )
            : base( game, view ) {

            HUDContainer aiScaleContainer = new HUDContainer( Vector2.Zero );

            this.aiContainer = new AI_Container(this);
            this.view.Add( this.aiContainer );
            this.aiContainer.Parent = aiScaleContainer;

            HUDTexture menuBG = new HUDTexture();
            menuBG.color = Color.Black;
            menuBG.AbstractSize = new Vector2( 0.18f, 1.0f );
            menuBG.AbstractPosition = new Vector2( 0.91f, 0.5f );
            menuBG.LayerDepth = 0.4f;
            this.view.Add( menuBG );

            HUDButton toMenuButton = new HUDButton( "Menu", new Vector2( 0.95f, 0.95f ), new Vector2( 0.04f, 0.06f ), 0.6f, this );
            toMenuButton.style = ButtonStyle.BuilderButtonStyle();
            toMenuButton.SetBackground( "Sprites//HUD//Ship" );
            toMenuButton.LayerDepth = 0.3f;
            toMenuButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.MENU );
            } );
            this.view.Add( toMenuButton );

            HUDArray verifyArray = new HUDArray( new Vector2( 0.9f, 0.6f ),  new Vector2( 0.15f, 0.1f ) );
            verifyArray.direction = LayoutDirection.HORIZONTAL;
            verifyArray.LayerDepth = 0.3f;
            this.view.Add( verifyArray );

            HUDButton verifyButton = new HUDButton( "Verify", new Vector2(), 0.36f, this );
            verifyButton.style = ButtonStyle.BuilderButtonStyle();
            verifyButton.SetBackground( "Sprites//builder_button_round" );
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
            verifyArray.Add( verifyButton );

            HUDArray statusArray = new HUDArray( new Vector2(), new Vector2()  );
            statusArray.direction = LayoutDirection.VERTICAL;
            verifyArray.Add( statusArray );

            statusArray.Add( new HUDString( "Status", 0.66f ) );
            statusArray.Add( new HUDString( "unknown", Color.Yellow, 0.66f ) );

            HUDArray aiButtonArray = new HUDArray( new Vector2( 0.9f, 0.8f ), new Vector2( 0.07f, 0.15f ) );
            aiButtonArray.direction = LayoutDirection.VERTICAL;
            aiButtonArray.LayerDepth = 0.3f;
            aiButtonArray.borderSize = new Vector2( 0, 0.01f );
            this.view.Add( aiButtonArray );

            HUDButton saveButton = new HUDButton( "Save", new Vector2(), 0.9f, this );
            saveButton.style = ButtonStyle.BuilderButtonStyle();
            saveButton.SetBackground( "Sprites//builder_button" );
            saveButton.SetPressedAction(
                delegate() {
                    AI_XML.WriteAIContainer( "testAI.xml", this.aiContainer );
                } );
            aiButtonArray.Add( saveButton );

            HUDButton loadButton = new HUDButton( "Load", new Vector2(), 0.9f, this );
            loadButton.style = ButtonStyle.BuilderButtonStyle();
            loadButton.SetBackground( "Sprites//builder_button" );
            loadButton.SetPressedAction(
                delegate() {
                    AI_XML.ReadAIContainer( "testAI.xml", this.aiContainer, this );
                } );
            aiButtonArray.Add( loadButton );

            HUDButton clearButton = new HUDButton( "Clear", new Vector2(), 0.9f, this );
            clearButton.style = ButtonStyle.BuilderButtonStyle();
            clearButton.SetBackground( "Sprites//builder_button" );
            clearButton.SetPressedAction(
                delegate() {
                    this.aiContainer.ClearAI();
                } );
            aiButtonArray.Add( clearButton );

        }

    }

}
