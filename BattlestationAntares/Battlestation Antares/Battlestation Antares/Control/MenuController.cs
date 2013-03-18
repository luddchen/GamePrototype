using System;
using Battlestation_Antares.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using System.Collections.Generic;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// the Menu controller
    /// </summary>
    class MenuController : SituationController {

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

            this.buttonGrid = new HUD2DArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 800, 150 ), HUDType.ABSOLUT, game );
            this.buttonGrid.layerDepth = 0.5f;
            this.buttonGrid.direction = LayoutDirection.VERTICAL;

            this.buttons1 = new HUD2DArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 600, 150 ), HUDType.ABSOLUT, game );
            this.buttons1.direction = LayoutDirection.HORIZONTAL;

            this.optionsButtonGroup = new HUD2DArray( new Vector2( 0.5f, 0.8f ), HUDType.RELATIV, new Vector2( 600, 150 ), HUDType.ABSOLUT, game );
            this.optionsButtonGroup.direction = LayoutDirection.HORIZONTAL;
            this.optionsButtonGroup.isVisible = false;

            this.buttonGrid.Add( this.optionsButtonGroup );
            this.buttonGrid.Add( this.buttons1 );


            HUD2DButton toCommandButton = new HUD2DButton( "Command", Vector2.Zero, 0.9f, this.game );
            toCommandButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COMMAND );
            } );
            this.buttons1.Add( toCommandButton );

            HUD2DButton toCockpitButton = new HUD2DButton( "Cockpit", Vector2.Zero, 0.9f, this.game );
            toCockpitButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COCKPIT );
            } );
            this.buttons1.Add( toCockpitButton );

            HUD2DButton toAIButton = new HUD2DButton( "Editor", Vector2.Zero, 0.9f, this.game );
            toAIButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.AI_BUILDER );
            } );
            this.buttons1.Add( toAIButton );

            this.optionsButton = new HUD2DButton( "Options", Vector2.Zero, 0.9f, this.game );
            this.optionsButton.SetPressedAction(
                delegate() {
                    this.optionsButton.Toggle();
                    hidePages();
                    this.optionsButtonGroup.isVisible = !( this.optionsButtonGroup.isVisible );
                } );
            this.buttons1.Add( this.optionsButton );

            HUD2DButton exitButton = new HUD2DButton( "Exit", Vector2.Zero, 0.9f, this.game );
            exitButton.SetPressedAction( delegate() {
                this.game.Exit();
            } );
            this.buttons1.Add( exitButton );

            HUD2DButton videoButton = new HUD2DButton( "Video", Vector2.Zero, 0.9f, this.game );
            videoButton.SetPressedAction( delegate() {
                showPage( this.videoPage );
            } );
            this.optionsButtonGroup.Add( videoButton );

            HUD2DButton soundButton = new HUD2DButton( "Sound", Vector2.Zero, 0.9f, this.game );
            soundButton.SetPressedAction( delegate() {
                showPage( this.soundPage );
            } );
            this.optionsButtonGroup.Add( soundButton );

            HUD2DButton controlButton = new HUD2DButton( "Control", Vector2.Zero, 0.9f, this.game );
            controlButton.SetPressedAction( delegate() {
                showPage( this.controlPage );
            } );
            this.optionsButtonGroup.Add( controlButton );


            this.view.allHUD_2D.Add( this.buttonGrid );


            this.contentPages = new List<HUD2DContainer>();

            this.videoPage = new HUD2DArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV, game );
            this.videoPage.CreateBackground( true );
            this.videoPage.Add( new HUD2DString( "Video", game ) );

            this.soundPage = new HUD2DArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV, game );
            this.soundPage.CreateBackground( true );
            this.soundPage.Add( new HUD2DString( "Sound", game ) );

            this.controlPage = new HUD2DArray( new Vector2( 0.5f, 0.4f ), HUDType.RELATIV, new Vector2( 0.7f, 0.5f ), HUDType.RELATIV, game );
            this.controlPage.CreateBackground( true );
            this.controlPage.Add( new HUD2DString( "Control", game ) );

            this.contentPages.Add( this.videoPage );
            this.contentPages.Add( this.soundPage );
            this.contentPages.Add( this.controlPage );

            foreach ( HUD2DContainer container in this.contentPages ) {
                container.isVisible = false;
                this.view.allHUD_2D.Add( container );
            }

            this.view.Window_ClientSizeChanged();
        }


        /// <summary>
        /// update the menu controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );
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
