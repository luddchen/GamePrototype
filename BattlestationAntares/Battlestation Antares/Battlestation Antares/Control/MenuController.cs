using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HUD.HUD;
using Battlestation_Antaris.View.HUD;
using HUD;
using Battlestation_Antares.View.HUD;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// the Menu controller
    /// </summary>
    class MenuController : SituationController {


        HUDRenderedItem test;

        private List<HUD_Item> contentPages;

        private HUDArray optionButtons;

        private List<DiscoLight> discoLights;

        /// <summary>
        /// create a new menu controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public MenuController( Antares game, HUDView view )
            : base( game, view ) {

            _createTestBackground();
            _createMainMenu();

            _addVideoPage();
            _addSoundPage();

            this.discoLights = new List<DiscoLight>();
            for ( int i = 0; i < 3; i++ ) {
                DiscoLight dLight = new DiscoLight( new Vector2( 0.5f, 0.5f ), new Vector2( 0.5f, 0.5f ), 3, 3 );
                dLight.LayerDepth = 0.1f;
                this.view.Add( dLight );
                this.discoLights.Add( dLight );
            }

            this.view.Window_ClientSizeChanged();
        }


        /// <summary>
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );

            this.test.Update( gameTime );
            foreach ( DiscoLight light in this.discoLights ) {
                light.Update( gameTime );
            }
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
            HUDArray buttonsGroup1 = new HUDArray( new Vector2( 0.5f, 0.9f ), new Vector2( 0.7f, 0.05f ) );
            buttonsGroup1.borderSize = new Vector2( 0.02f, 0.0f );
            buttonsGroup1.direction = LayoutDirection.HORIZONTAL;

            this.view.Add( buttonsGroup1 );

            this.optionButtons = new HUDArray( new Vector2( 0.1f, 0.5f ), new Vector2( 0.1f, 0.01f ) );
            this.optionButtons.borderSize = new Vector2( 0.0f, 0.05f );
            this.optionButtons.direction = LayoutDirection.VERTICAL;
            this.optionButtons.IsVisible = false;

            this.contentPages = new List<HUD_Item>();

            this.view.Add( this.optionButtons );


            HUDButton toCommandButton = new HUDButton( "Command", Vector2.Zero, 0.9f, this );
            toCommandButton.style = AntaresButtonStyles.Button();
            toCommandButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COMMAND );
            } );
            buttonsGroup1.Add( toCommandButton );

            HUDButton toCockpitButton = new HUDButton( "Cockpit", Vector2.Zero, 0.9f, this );
            toCockpitButton.style = AntaresButtonStyles.Button();
            toCockpitButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COCKPIT );
            } );
            buttonsGroup1.Add( toCockpitButton );

            HUDButton toAIButton = new HUDButton( "Editor", Vector2.Zero, 0.9f, this );
            toAIButton.style = AntaresButtonStyles.Button();
            toAIButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.AI_BUILDER );
            } );
            buttonsGroup1.Add( toAIButton );

            HUDButton optionsButton = new HUDButton( "Options", Vector2.Zero, 0.9f, this );
            optionsButton.style = AntaresButtonStyles.Button();
            optionsButton.SetPressedAction(
                delegate() {
                    optionsButton.Toggle();
                    hidePages();
                    this.optionButtons.ToggleVisibility();
                } );
            buttonsGroup1.Add( optionsButton );

            HUDButton exitButton = new HUDButton( "Exit", Vector2.Zero, 0.9f, this );
            exitButton.style = AntaresButtonStyles.Button();
            exitButton.SetPressedAction( delegate() {
                this.game.Exit();
            } );
            buttonsGroup1.Add( exitButton );
        }


        private void _addOptionPage( String name, HUD_Item item ) {
            this.optionButtons.AbstractSize += new Vector2( 0, 0.1f );

            HUDButton newButton = new HUDButton( name, Vector2.Zero, 0.9f, this );
            newButton.style = AntaresButtonStyles.Button();
            newButton.SetPressedAction( delegate() {
                showPage( item );
            } );
            this.optionButtons.Add( newButton );

            this.contentPages.Add( item );
            this.view.Add( item );
            item.IsVisible = false;
        }


        private void _addVideoPage() {
            HUDContainer videoPage = new HUDContainer( new Vector2( 0.5f, 0.4f ) );

            HUDArray renderResolutionArray = new HUDArray( new Vector2( -0.15f, 0.0f ),  new Vector2( 0.2f, 0.3f ) );
            renderResolutionArray.borderSize = new Vector2( 0.025f, 0.02f );
            videoPage.Add( renderResolutionArray );

            HUDString resolutionTitle = new HUDString( "Render Resolution", 0.7f );
            HUDButton resolutionHigh = new HUDButton( "1920 x 1080", new Vector2(), 0.7f, this );
            HUDButton resolutionMedium = new HUDButton( "1600 x 900", new Vector2(), 0.7f, this );
            HUDButton resolutionLow = new HUDButton( "1280 x 720", new Vector2(), 0.7f, this );

            resolutionHigh.style = AntaresButtonStyles.Button();
            resolutionMedium.style = AntaresButtonStyles.Button();
            resolutionLow.style = AntaresButtonStyles.Button();

            renderResolutionArray.Add( resolutionTitle );
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


            HUDArray test1Array = new HUDArray( new Vector2( 0.15f, 0.0f ), new Vector2( 0.2f, 0.3f ) );
            test1Array.borderSize = new Vector2( 0.025f, 0.02f );
            videoPage.Add( test1Array );

            HUDString test1Title = new HUDString( "Placeholder", 0.7f );
            HUDButton test1High = new HUDButton( "do nothing", new Vector2(), 0.7f, null );
            HUDButton test1Medium = new HUDButton( "wait for something", new Vector2(), 0.7f, null );
            HUDButton test1Low = new HUDButton( "do anything", new Vector2(), 0.7f, null );

            test1Array.Add( test1Title );
            test1Array.Add( test1High );
            test1Array.Add( test1Medium );
            test1Array.Add( test1Low );

            _addOptionPage( "Video", videoPage );
        }


        private void _addSoundPage() {
            HUDArray soundPage = new HUDArray( new Vector2( 0.5f, 0.4f ), new Vector2( 0.7f, 0.5f ) );
            soundPage.Add( new HUDString( "Sound", 0.5f ) );

            _addOptionPage( "Sound", soundPage );
        }


        private void _createTestBackground() {
            HUDTexture testTex = new HUDTexture();
            testTex.AbstractPosition = new Vector2( 0.5f, 0.5f );
            testTex.PositionType = HUDType.RELATIV;
            testTex.AbstractSize = new Vector2( 2f, 2f );
            testTex.SizeType = HUDType.RELATIV;
            testTex.Texture = Antares.content.Load<Texture2D>( "Sprites//StationScreen" );
            testTex.color = new Color( 128, 128, 128, 128 );

            this.test = new HUDRenderedItem( testTex, new Point( 480, 480 ), null );
            this.test.AbstractPosition = new Vector2( 0.5f, 0.5f );
            this.test.PositionType = HUDType.RELATIV;
            this.test.AbstractSize = new Vector2( 0.7f, 0.7f );
            this.test.SizeType = HUDType.RELATIV;
            this.view.Add( this.test );
        }

    }

}
