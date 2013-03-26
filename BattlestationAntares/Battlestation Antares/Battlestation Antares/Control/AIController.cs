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

            this.aiContainer = new AI_Container(this);
            this.view.Add( this.aiContainer );

            HUDButton toMenuButton = new HUDButton( "Menu", new Vector2( 0.95f, 0.95f ), 0.5f, this );
            toMenuButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.MENU );
            } );
            toMenuButton.PositionType = HUDType.RELATIV;
            toMenuButton.style = ButtonStyle.BuilderButtonStyle();
            toMenuButton.SetBackground( Antares.content.Load<Texture2D>( "Sprites//HUD//Ship" ) );
            toMenuButton.SizeType = HUDType.RELATIV;
            toMenuButton.AbstractSize = new Vector2( 0.03f, 0.055f );
            this.view.Add( toMenuButton );

            HUDArray verifyArray = new HUDArray( new Vector2( 0.9f, 0.6f ),  new Vector2( 0.15f, 0.1f ) );
            verifyArray.direction = LayoutDirection.HORIZONTAL;
            this.view.Add( verifyArray );

            HUDButton verifyButton = new HUDButton( "Verify", new Vector2(), 0.66f, this );
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
            verifyButton.SetBackground( Antares.content.Load<Texture2D>( "Sprites//builder_button_round" ) );
            verifyArray.Add( verifyButton );

            HUDArray statusArray = new HUDArray( new Vector2(), new Vector2()  );
            statusArray.direction = LayoutDirection.VERTICAL;
            verifyArray.Add( statusArray );

            statusArray.Add( new HUDString( "Status", 0.66f ) );
            statusArray.Add( new HUDString( "unknown", Color.Yellow, 0.66f ) );


            HUDArray aiButtonArray = new HUDArray( new Vector2( 0.9f, 0.8f ), new Vector2( 0.1f, 0.15f ) );
            aiButtonArray.direction = LayoutDirection.VERTICAL;
            this.view.Add( aiButtonArray );

            HUDButton saveButton = new HUDButton( "Save", new Vector2(), 0.75f, this );
            saveButton.SetPressedAction(
                delegate() {
                    AI_XML.WriteAIContainer( "testAI.xml", this.aiContainer );
                } );
            saveButton.style = ButtonStyle.BuilderButtonStyle();
            aiButtonArray.Add( saveButton );

            HUDButton loadButton = new HUDButton( "Load", new Vector2(), 0.75f, this );
            loadButton.SetPressedAction(
                delegate() {
                    AI_XML.ReadAIContainer( "testAI.xml", this.aiContainer, this );
                } );
            loadButton.style = ButtonStyle.BuilderButtonStyle();
            aiButtonArray.Add( loadButton );

            HUDButton clearButton = new HUDButton( "Clear", new Vector2(), 0.75f, this );
            clearButton.SetPressedAction(
                delegate() {
                    this.aiContainer.ClearAI();
                } );
            clearButton.style = ButtonStyle.BuilderButtonStyle();
            aiButtonArray.Add( clearButton );

        }

    }

}
