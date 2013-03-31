using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HUD.HUD;
using Battlestation_Antaris.View.HUD;
using HUD;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.Tools;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// the Menu controller
    /// </summary>
    class MenuController : SituationController {

        private HUDContainer mainMenuButtons;

        private List<HUD_Item> contentPages;

        private HUDArray optionButtons;

        private List<DiscoLight> discoLights;

        private double animationValue = 0.0;

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
                dLight.LayerDepth = (float)RandomGen.random.NextDouble();
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

            foreach ( DiscoLight light in this.discoLights ) {
                light.Update( gameTime );
            }

            this.animationValue += Math.PI / 45.0;
            if ( this.animationValue >= Math.PI * 2 ) {
                this.animationValue = 0.0;
            }

            this.mainMenuButtons.AbstractScale = 1.0f + (float)Math.Sin( this.animationValue ) * 0.01f;
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
            this.mainMenuButtons = new HUDContainer( new Vector2( 0.5f, 0.9f ), new Vector2( 0.7f, 0.05f ) );

            this.view.Add( this.mainMenuButtons );

            this.optionButtons = new HUDArray( new Vector2( 0.1f, 0.5f ), new Vector2( 0.1f, 0.01f ) );
            this.optionButtons.borderSize = new Vector2( 0.0f, 0.05f );
            this.optionButtons.direction = LayoutDirection.VERTICAL;
            this.optionButtons.IsVisible = false;

            this.contentPages = new List<HUD_Item>();

            this.view.Add( this.optionButtons );


            HUDButton toCommandButton = new HUDButton( "Command", new Vector2( -0.3f, 0f ), new Vector2( 0.1f, 0.05f ), 0.9f, this );
            toCommandButton.AbstractRotation = (float)( -Math.PI / 12.0 );
            toCommandButton.style = AntaresButtonStyles.Button();
            toCommandButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COMMAND );
            } );
            this.mainMenuButtons.Add( toCommandButton );

            HUDButton toAIButton = new HUDButton( "Editor", new Vector2( -0.1f, 0.04f ), new Vector2( 0.1f, 0.05f ), 0.9f, this );
            toAIButton.AbstractRotation = (float)( -Math.PI / 46.0 );
            toAIButton.style = AntaresButtonStyles.Button();
            toAIButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.AI_BUILDER );
            } );
            this.mainMenuButtons.Add( toAIButton );

            HUDButton optionsButton = new HUDButton( "Options", new Vector2( 0.1f, 0.04f ), new Vector2( 0.1f, 0.05f ), 0.9f, this );
            optionsButton.AbstractRotation = (float)( Math.PI / 46.0 );
            optionsButton.style = AntaresButtonStyles.Button();
            optionsButton.SetPressedAction(
                delegate() {
                    optionsButton.Toggle();
                    hidePages();
                    this.optionButtons.ToggleVisibility();
                } );
            this.mainMenuButtons.Add( optionsButton );

            HUDButton exitButton = new HUDButton( "Exit", new Vector2( 0.3f, 0f ), new Vector2( 0.1f, 0.05f ), 0.9f, this );
            exitButton.AbstractRotation = (float)( Math.PI / 12.0 );
            exitButton.style = AntaresButtonStyles.Button();
            exitButton.SetPressedAction( delegate() {
                this.game.Exit();
            } );
            this.mainMenuButtons.Add( exitButton );
        }


        private void _addOptionPage( String name, HUD_Item item ) {
            this.optionButtons.AbstractSize += new Vector2( 0, 0.1f );

            HUDButton newButton = new HUDButton( text: name, scale: 0.9f, controller: this );
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

            HUDString resolutionTitle = new HUDString( "Render Resolution", null, null, scale: 0.7f );
            HUDButton resolutionHigh = new HUDButton( "1920 x 1080", scale: 0.7f, controller: this );
            HUDButton resolutionMedium = new HUDButton( "1600 x 900", scale: 0.7f, controller: this );
            HUDButton resolutionLow = new HUDButton( "1280 x 720", scale: 0.7f, controller: this );

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
                    HUDService.RenderSize = new Point( 1920, 1080 );
                } );

            resolutionMedium.SetPressedAction(
                delegate() {
                    resolutionHigh.style.foregroundColorNormal = Color.White;
                    resolutionMedium.style.foregroundColorNormal = Color.Green;
                    resolutionLow.style.foregroundColorNormal = Color.White;
                    HUDService.RenderSize = new Point( 1600, 900 );
                } );

            resolutionLow.SetPressedAction(
                delegate() {
                    resolutionHigh.style.foregroundColorNormal = Color.White;
                    resolutionMedium.style.foregroundColorNormal = Color.White;
                    resolutionLow.style.foregroundColorNormal = Color.Green;
                    HUDService.RenderSize = new Point( 1280, 720 );
                } );


            HUDArray multiSampleArray = new HUDArray( new Vector2( 0.15f, 0.0f ), new Vector2( 0.2f, 0.3f ) );
            multiSampleArray.borderSize = new Vector2( 0.025f, 0.02f );
            videoPage.Add( multiSampleArray );

            HUDString multiSampleTitle = new HUDString( "Multi Sampling", null, null, scale: 0.7f );
            HUDButton samplingOff = new HUDButton( "off", scale: 0.7f, controller: this );
            HUDButton sampling2x = new HUDButton( "2x", scale: 0.7f, controller: this );
            HUDButton sampling4x = new HUDButton( "4x", scale: 0.7f, controller: this );

            samplingOff.style = AntaresButtonStyles.Button();
            sampling2x.style = AntaresButtonStyles.Button();
            sampling4x.style = AntaresButtonStyles.Button();

            multiSampleArray.Add( multiSampleTitle );
            multiSampleArray.Add( samplingOff );
            multiSampleArray.Add( sampling2x );
            multiSampleArray.Add( sampling4x );

            samplingOff.SetPressedAction(
                delegate() {
                    samplingOff.style.foregroundColorNormal = Color.Green;
                    sampling2x.style.foregroundColorNormal = Color.White;
                    sampling4x.style.foregroundColorNormal = Color.White;
                    HUDService.MultiSampleCount = 1;
                } );

            sampling2x.SetPressedAction(
                delegate() {
                    samplingOff.style.foregroundColorNormal = Color.White;
                    sampling2x.style.foregroundColorNormal = Color.Green;
                    sampling4x.style.foregroundColorNormal = Color.White;
                    HUDService.MultiSampleCount = 2;
                } );

            sampling4x.SetPressedAction(
                delegate() {
                    samplingOff.style.foregroundColorNormal = Color.White;
                    sampling2x.style.foregroundColorNormal = Color.White;
                    sampling4x.style.foregroundColorNormal = Color.Green;
                    HUDService.MultiSampleCount = 4;
                } );

            _addOptionPage( "Video", videoPage );
        }


        private void _addSoundPage() {
            HUDArray soundPage = new HUDArray( new Vector2( 0.5f, 0.4f ), new Vector2( 0.7f, 0.5f ) );
            soundPage.Add( new HUDString( "Sound", null, null, scale: 0.5f ) );

            _addOptionPage( "Sound", soundPage );
        }


        private void _createTestBackground() {
            HUDTexture testTex = 
                new HUDTexture( "Sprites//StationScreen", new Color( 128, 128, 128, 128 ), position: new Vector2( 0.5f, 0.5f ), size: new Vector2( 0.8f, 0.8f ) );
            testTex.LayerDepth = 0.6f;
            this.view.Add( testTex );

            //this.test = new HUDRenderedItem( testTex, new Point( 480, 480 ), null );
            //this.test.AbstractPosition = new Vector2( 0.5f, 0.5f );
            //this.test.PositionType = HUDType.RELATIV;
            //this.test.AbstractSize = new Vector2( 0.7f, 0.7f );
            //this.test.SizeType = HUDType.RELATIV;
            //this.view.Add( this.test );
        }

    }

}
