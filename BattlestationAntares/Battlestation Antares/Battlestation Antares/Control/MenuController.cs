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


        HUD2DRenderedItem test;

        private HUD2DArray buttonGrid;

        private HUD2DArray buttons1;

        private HUD2DArray optionsButtonGroup;

        private HUD2DButton optionsButton;


        private List<HUD2DContainer> contentPages;

        private HUD2DArray videoPage;

        private HUD2DArray soundPage;

        private HUD2DArray controlPage;

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController( Antares game, View.View view )
            : base( game, view ) {
            this.worldUpdate = WorldUpdate.NO_UPDATE;

            // test content
            HUD2DTexture testTex = new HUD2DTexture();
            testTex.abstractPosition = new Vector2( 0.5f, 0.5f );
            testTex.positionType = HUDType.RELATIV;
            testTex.abstractSize = new Vector2( 2f, 2f );
            testTex.sizeType = HUDType.RELATIV;
            testTex.Texture = Antares.content.Load<Texture2D>( "Sprites//Galaxy" );
            testTex.color = new Color( 128, 128, 128, 128 );

            test = new HUD2DRenderedItem( testTex , new Vector2(480,480), null);
            test.abstractPosition = new Vector2( 0.5f, 0.5f );
            test.positionType = HUDType.RELATIV;
            test.abstractSize = new Vector2( 0.7f, 0.7f );
            test.sizeType = HUDType.RELATIV;
            testTex.layerDepth = 1.0f;
            this.view.allHUD_2D.Add( test );


            this.buttonGrid = new HUD2DArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 800, 150 ), HUDType.ABSOLUT);
            this.buttonGrid.layerDepth = 0.5f;
            this.buttonGrid.direction = LayoutDirection.VERTICAL;

            this.buttons1 = new HUD2DArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 600, 150 ), HUDType.ABSOLUT);
            this.buttons1.direction = LayoutDirection.HORIZONTAL;

            this.optionsButtonGroup = new HUD2DArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 600, 150 ), HUDType.ABSOLUT);
            this.optionsButtonGroup.direction = LayoutDirection.HORIZONTAL;
            this.optionsButtonGroup.isVisible = false;

            this.buttonGrid.Add( this.optionsButtonGroup );
            this.buttonGrid.Add( this.buttons1 );


            HUD2DButton toCommandButton = new HUD2DButton( "Command", Vector2.Zero, 0.9f);
            toCommandButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COMMAND );
            } );
            this.buttons1.Add( toCommandButton );

            HUD2DButton toCockpitButton = new HUD2DButton( "Cockpit", Vector2.Zero, 0.9f);
            toCockpitButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COCKPIT );
            } );
            this.buttons1.Add( toCockpitButton );

            HUD2DButton toAIButton = new HUD2DButton( "Editor", Vector2.Zero, 0.9f);
            toAIButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.AI_BUILDER );
            } );
            this.buttons1.Add( toAIButton );

            this.optionsButton = new HUD2DButton( "Options", Vector2.Zero, 0.9f);
            this.optionsButton.SetPressedAction(
                delegate() {
                    this.optionsButton.Toggle();
                    hidePages();
                    this.optionsButtonGroup.isVisible = !( this.optionsButtonGroup.isVisible );
                } );
            this.buttons1.Add( this.optionsButton );

            HUD2DButton exitButton = new HUD2DButton( "Exit", Vector2.Zero, 0.9f);
            exitButton.SetPressedAction( delegate() {
                this.game.Exit();
            } );
            this.buttons1.Add( exitButton );

            HUD2DButton videoButton = new HUD2DButton( "Video", Vector2.Zero, 0.9f);
            videoButton.SetPressedAction( delegate() {
                showPage( this.videoPage );
            } );
            this.optionsButtonGroup.Add( videoButton );

            HUD2DButton soundButton = new HUD2DButton( "Sound", Vector2.Zero, 0.9f);
            soundButton.SetPressedAction( delegate() {
                showPage( this.soundPage );
            } );
            this.optionsButtonGroup.Add( soundButton );

            HUD2DButton controlButton = new HUD2DButton( "Control", Vector2.Zero, 0.9f);
            controlButton.SetPressedAction( delegate() {
                showPage( this.controlPage );
            } );
            this.optionsButtonGroup.Add( controlButton );


            this.view.allHUD_2D.Add( this.buttonGrid );


            this.contentPages = new List<HUD2DContainer>();

            this.videoPage = new HUD2DArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV);
            this.videoPage.CreateBackground( true );
            this.videoPage.Add( new HUD2DString( "Video") );

            this.soundPage = new HUD2DArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV);
            this.soundPage.CreateBackground( true );
            this.soundPage.Add( new HUD2DString( "Sound") );

            this.controlPage = new HUD2DArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV);
            this.controlPage.direction = LayoutDirection.HORIZONTAL;

            this.contentPages.Add( this.videoPage );
            this.contentPages.Add( this.soundPage );
            this.contentPages.Add( this.controlPage );

            foreach ( HUD2DContainer container in this.contentPages ) {
                container.isVisible = false;
                this.view.allHUD_2D.Add( container );
            }


            HUD2DContainer renderResolutionArray = new HUD2DContainer( new Vector2(), HUDType.RELATIV );
            this.controlPage.Add( renderResolutionArray );

            HUD2DString resolutionTitle = new HUD2DString( "Render Resolution", null, new Vector2( 0, -0.05f ), null, null, 0.6f, null );
            resolutionTitle.positionType = HUDType.RELATIV;
            renderResolutionArray.Add( resolutionTitle );

            HUD2DButton resolutionHigh = new HUD2DButton( "1920 x 1080", new Vector2(0, 0), 0.7f );
            resolutionHigh.positionType = HUDType.RELATIV;
            HUD2DButton resolutionMedium = new HUD2DButton( "1600 x 900", new Vector2( 0, 0.05f ), 0.7f );
            resolutionMedium.positionType = HUDType.RELATIV;
            HUD2DButton resolutionLow = new HUD2DButton( "1280 x 720", new Vector2( 0, 0.1f ), 0.7f );
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


            HUD2DContainer test1Array = new HUD2DContainer( new Vector2(), HUDType.RELATIV );
            this.controlPage.Add( test1Array );

            HUD2DString test1Title = new HUD2DString( "Placeholder", null, new Vector2( 0, -0.05f ), null, null, 0.6f, null );
            test1Title.positionType = HUDType.RELATIV;
            test1Array.Add( test1Title );

            HUD2DButton test1High = new HUD2DButton( "do nothing", new Vector2( 0, 0 ), 0.7f );
            test1High.positionType = HUDType.RELATIV;
            HUD2DButton test1Medium = new HUD2DButton( "wait for something", new Vector2( 0, 0.05f ), 0.7f );
            test1Medium.positionType = HUDType.RELATIV;
            HUD2DButton test1Low = new HUD2DButton( "do anything", new Vector2( 0, 0.1f ), 0.7f );
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
            foreach ( HUD2DContainer container in this.contentPages ) {
                container.isVisible = false;
            }
        }

        private void showPage( HUD2DContainer container ) {
            hidePages();
            container.isVisible = true;
        }

    }

}
