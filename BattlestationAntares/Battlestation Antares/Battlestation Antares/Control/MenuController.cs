using System;
using Battlestation_Antares.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using System.Collections.Generic;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// the Menu controller
    /// </summary>
    class MenuController : SituationController {


        HUDRenderedItem test;

        private HUDArray buttonGrid;

        private HUDArray buttons1;

        private HUDArray optionsButtonGroup;

        private HUDButton optionsButton;


        private List<HUDContainer> contentPages;

        private HUDArray videoPage;

        private HUDArray soundPage;

        private HUDArray controlPage;

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController( Antares game, View.View view )
            : base( game, view ) {

            // test content
            HUDTexture testTex = new HUDTexture();
            testTex.abstractPosition = new Vector2( 0.5f, 0.5f );
            testTex.positionType = HUDType.RELATIV;
            testTex.abstractSize = new Vector2( 2f, 2f );
            testTex.sizeType = HUDType.RELATIV;
            testTex.Texture = Antares.content.Load<Texture2D>( "Sprites//Galaxy" );
            testTex.color = new Color( 128, 128, 128, 128 );

            test = new HUDRenderedItem( testTex , new Vector2(480,480), null);
            test.abstractPosition = new Vector2( 0.5f, 0.5f );
            test.positionType = HUDType.RELATIV;
            test.abstractSize = new Vector2( 0.7f, 0.7f );
            test.sizeType = HUDType.RELATIV;
            testTex.layerDepth = 1.0f;
            this.view.Add( test );


            this.buttonGrid = new HUDArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 800, 150 ), HUDType.ABSOLUT);
            this.buttonGrid.layerDepth = 0.5f;
            this.buttonGrid.direction = LayoutDirection.VERTICAL;

            this.buttons1 = new HUDArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 600, 150 ), HUDType.ABSOLUT);
            this.buttons1.direction = LayoutDirection.HORIZONTAL;

            this.optionsButtonGroup = new HUDArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 600, 150 ), HUDType.ABSOLUT);
            this.optionsButtonGroup.direction = LayoutDirection.HORIZONTAL;
            this.optionsButtonGroup.IsVisible = false;

            this.buttonGrid.Add( this.optionsButtonGroup );
            this.buttonGrid.Add( this.buttons1 );


            HUDButton toCommandButton = new HUDButton( "Command", Vector2.Zero, 0.9f, this);
            toCommandButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COMMAND );
            } );
            this.buttons1.Add( toCommandButton );

            HUDButton toCockpitButton = new HUDButton( "Cockpit", Vector2.Zero, 0.9f, this);
            toCockpitButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COCKPIT );
            } );
            this.buttons1.Add( toCockpitButton );

            HUDButton toAIButton = new HUDButton( "Editor", Vector2.Zero, 0.9f, this);
            toAIButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.AI_BUILDER );
            } );
            this.buttons1.Add( toAIButton );

            this.optionsButton = new HUDButton( "Options", Vector2.Zero, 0.9f, this);
            this.optionsButton.SetPressedAction(
                delegate() {
                    this.optionsButton.Toggle();
                    hidePages();
                    this.optionsButtonGroup.ToggleVisibility();
                } );
            this.buttons1.Add( this.optionsButton );

            HUDButton exitButton = new HUDButton( "Exit", Vector2.Zero, 0.9f, this);
            exitButton.SetPressedAction( delegate() {
                this.game.Exit();
            } );
            this.buttons1.Add( exitButton );

            HUDButton videoButton = new HUDButton( "Video", Vector2.Zero, 0.9f, this);
            videoButton.SetPressedAction( delegate() {
                showPage( this.videoPage );
            } );
            this.optionsButtonGroup.Add( videoButton );

            HUDButton soundButton = new HUDButton( "Sound", Vector2.Zero, 0.9f, this);
            soundButton.SetPressedAction( delegate() {
                showPage( this.soundPage );
            } );
            this.optionsButtonGroup.Add( soundButton );

            HUDButton controlButton = new HUDButton( "Control", Vector2.Zero, 0.9f, this);
            controlButton.SetPressedAction( delegate() {
                showPage( this.controlPage );
            } );
            this.optionsButtonGroup.Add( controlButton );


            this.view.Add( this.buttonGrid );


            this.contentPages = new List<HUDContainer>();

            this.videoPage = new HUDArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV);
            this.videoPage.CreateBackground( true );
            this.videoPage.Add( new HUDString( "Video") );

            this.soundPage = new HUDArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV);
            this.soundPage.CreateBackground( true );
            this.soundPage.Add( new HUDString( "Sound") );

            this.controlPage = new HUDArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV);
            this.controlPage.direction = LayoutDirection.HORIZONTAL;

            this.contentPages.Add( this.videoPage );
            this.contentPages.Add( this.soundPage );
            this.contentPages.Add( this.controlPage );

            foreach ( HUDContainer container in this.contentPages ) {
                container.IsVisible = false;
                this.view.Add( container );
            }


            HUDContainer renderResolutionArray = new HUDContainer( new Vector2(), HUDType.RELATIV );
            this.controlPage.Add( renderResolutionArray );

            HUDString resolutionTitle = new HUDString( "Render Resolution", null, new Vector2( 0, -0.05f ), null, null, 0.6f, null );
            resolutionTitle.positionType = HUDType.RELATIV;
            renderResolutionArray.Add( resolutionTitle );

            HUDButton resolutionHigh = new HUDButton( "1920 x 1080", new Vector2(0, 0), 0.7f, this );
            resolutionHigh.positionType = HUDType.RELATIV;
            HUDButton resolutionMedium = new HUDButton( "1600 x 900", new Vector2( 0, 0.05f ), 0.7f, this );
            resolutionMedium.positionType = HUDType.RELATIV;
            HUDButton resolutionLow = new HUDButton( "1280 x 720", new Vector2( 0, 0.1f ), 0.7f, this );
            resolutionLow.positionType = HUDType.RELATIV;

            renderResolutionArray.Add( resolutionHigh );
            renderResolutionArray.Add( resolutionMedium );
            renderResolutionArray.Add( resolutionLow );

            resolutionHigh.SetPressedAction( 
                delegate() {
                    resolutionHigh.style.foregroundColorNormal = Color.Green;
                    resolutionMedium.style.foregroundColorNormal = Color.White;
                    resolutionLow.style.foregroundColorNormal = Color.White;
                    Antares.setRenderSize( 1920, 1080 );
                } );

            resolutionMedium.SetPressedAction(
                delegate() {
                    resolutionHigh.style.foregroundColorNormal = Color.White;
                    resolutionMedium.style.foregroundColorNormal = Color.Green;
                    resolutionLow.style.foregroundColorNormal = Color.White;
                    Antares.setRenderSize( 1600, 900 );
                } );

            resolutionLow.SetPressedAction(
                delegate() {
                    resolutionHigh.style.foregroundColorNormal = Color.White;
                    resolutionMedium.style.foregroundColorNormal = Color.White;
                    resolutionLow.style.foregroundColorNormal = Color.Green;
                    Antares.setRenderSize( 1280, 720 );
                } );


            HUDContainer test1Array = new HUDContainer( new Vector2(), HUDType.RELATIV );
            this.controlPage.Add( test1Array );

            HUDString test1Title = new HUDString( "Placeholder", null, new Vector2( 0, -0.05f ), null, null, 0.6f, null );
            test1Title.positionType = HUDType.RELATIV;
            test1Array.Add( test1Title );

            HUDButton test1High = new HUDButton( "do nothing", new Vector2( 0, 0 ), 0.7f, null );
            test1High.positionType = HUDType.RELATIV;
            HUDButton test1Medium = new HUDButton( "wait for something", new Vector2( 0, 0.05f ), 0.7f, null );
            test1Medium.positionType = HUDType.RELATIV;
            HUDButton test1Low = new HUDButton( "do anything", new Vector2( 0, 0.1f ), 0.7f, null );
            test1Low.positionType = HUDType.RELATIV;

            test1Array.Add( test1High );
            test1Array.Add( test1Medium );
            test1Array.Add( test1Low );


            this.view.Window_ClientSizeChanged();
        }


        /// <summary>
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );

            test.Render();
        }


        private void hidePages() {
            foreach ( HUDContainer container in this.contentPages ) {
                container.IsVisible = false;
            }
        }

        private void showPage( HUDContainer container ) {
            hidePages();
            container.IsVisible = true;
        }

    }

}
