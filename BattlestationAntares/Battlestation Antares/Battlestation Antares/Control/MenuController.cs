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

        private List<HUD_Item> contentPages;

        private HUDArray optionButtons;

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController( Antares game, View.View view )
            : base( game, view ) {

            _createTestBackground();
            _createMainMenu();

            _addVideoPage();
            _addSoundPage();

            this.view.Window_ClientSizeChanged();
        }


        /// <summary>
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );

            this.test.Render();
        }


        private void hidePages() {
            foreach ( HUD_Item page in this.contentPages ) {
                page.IsVisible = false;
            }
        }

        private void showPage( HUD_Item page ) {
            hidePages();
            page.IsVisible = true;
        }


        private void _createMainMenu() {
            HUDArray buttonsGroup1 = new HUDArray( new Vector2( 0.5f, 0.9f ), HUDType.RELATIV, new Vector2( 0.7f, 0.05f ), HUDType.RELATIV );
            buttonsGroup1.borderSize = new Vector2( 0.02f, 0.0f );
            buttonsGroup1.direction = LayoutDirection.HORIZONTAL;

            this.view.Add( buttonsGroup1 );

            this.optionButtons = new HUDArray( new Vector2( 0.1f, 0.5f ), HUDType.RELATIV, new Vector2( 0.1f, 0.01f ), HUDType.RELATIV );
            this.optionButtons.borderSize = new Vector2( 0.0f, 0.05f );
            this.optionButtons.direction = LayoutDirection.VERTICAL;
            this.optionButtons.IsVisible = false;

            this.contentPages = new List<HUD_Item>();

            this.view.Add( this.optionButtons );


            HUDButton toCommandButton = new HUDButton( "Command", Vector2.Zero, 0.9f, this );
            toCommandButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COMMAND );
            } );
            buttonsGroup1.Add( toCommandButton );

            HUDButton toCockpitButton = new HUDButton( "Cockpit", Vector2.Zero, 0.9f, this );
            toCockpitButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COCKPIT );
            } );
            buttonsGroup1.Add( toCockpitButton );

            HUDButton toAIButton = new HUDButton( "Editor", Vector2.Zero, 0.9f, this );
            toAIButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.AI_BUILDER );
            } );
            buttonsGroup1.Add( toAIButton );

            HUDButton optionsButton = new HUDButton( "Options", Vector2.Zero, 0.9f, this );
            optionsButton.SetPressedAction(
                delegate() {
                    optionsButton.Toggle();
                    hidePages();
                    this.optionButtons.ToggleVisibility();
                } );
            buttonsGroup1.Add( optionsButton );

            HUDButton exitButton = new HUDButton( "Exit", Vector2.Zero, 0.9f, this );
            exitButton.SetPressedAction( delegate() {
                this.game.Exit();
            } );
            buttonsGroup1.Add( exitButton );
        }


        private void _addOptionPage( String name, HUD_Item item ) {
            this.optionButtons.abstractSize += new Vector2( 0, 0.1f );

            HUDButton newButton = new HUDButton( name, Vector2.Zero, 0.9f, this );
            newButton.SetPressedAction( delegate() {
                showPage( item );
            } );
            this.optionButtons.Add( newButton );
            this.optionButtons.ClientSizeChanged();

            this.contentPages.Add( item );
            this.view.Add( item );
            item.IsVisible = false;
        }


        private void _addVideoPage() {
            HUDArray videoPage = new HUDArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV );
            videoPage.direction = LayoutDirection.HORIZONTAL;

            HUDContainer renderResolutionArray = new HUDContainer( new Vector2(), HUDType.RELATIV );
            videoPage.Add( renderResolutionArray );

            HUDString resolutionTitle = new HUDString( "Render Resolution", null, new Vector2( 0, -0.05f ), null, null, 0.6f, null );
            resolutionTitle.positionType = HUDType.RELATIV;
            renderResolutionArray.Add( resolutionTitle );

            HUDButton resolutionHigh = new HUDButton( "1920 x 1080", new Vector2( 0, 0 ), 0.7f, this );
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
            videoPage.Add( test1Array );

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

            _addOptionPage( "Video", videoPage );
        }


        private void _addSoundPage() {
            HUDArray soundPage = new HUDArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV );
            //soundPage.CreateBackground( true );
            soundPage.Add( new HUDString( "Sound" ) );

            _addOptionPage( "Sound", soundPage );
        }


        private void _createTestBackground() {
            HUDTexture testTex = new HUDTexture();
            testTex.abstractPosition = new Vector2( 0.5f, 0.5f );
            testTex.positionType = HUDType.RELATIV;
            testTex.abstractSize = new Vector2( 2f, 2f );
            testTex.sizeType = HUDType.RELATIV;
            testTex.Texture = Antares.content.Load<Texture2D>( "Sprites//Galaxy" );
            testTex.color = new Color( 128, 128, 128, 128 );

            this.test = new HUDRenderedItem( testTex, new Vector2( 480, 480 ), null );
            this.test.abstractPosition = new Vector2( 0.5f, 0.5f );
            this.test.positionType = HUDType.RELATIV;
            this.test.abstractSize = new Vector2( 0.7f, 0.7f );
            this.test.sizeType = HUDType.RELATIV;
            this.view.Add( this.test );
        }

    }

}
