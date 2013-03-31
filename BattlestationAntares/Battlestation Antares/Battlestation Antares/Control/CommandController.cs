using System;
using System.Collections.Generic;
using Battlestation_Antares.Model;
using Battlestation_Antares.View.HUD;
using Battlestation_Antares.View.HUD.CommandHUD;
using Battlestation_Antaris.Model;
using Battlestation_Antaris.View.HUD;
using HUD;
using HUD.HUD;
using Microsoft.Xna.Framework;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// the command controller
    /// </summary>
    class CommandController : SituationController {

        private enum CommandMode {
            NORMAL,
            BUILD
        }

        private CommandMode currentMode;

        private MiniMapRenderer.Config mapConfig;

        private BuildMenu buildMenu;

        private Dictionary<Type, MouseTexture> mouseTextures;

        private MouseTexture currentMouseTexture;

        /// <summary>
        /// create a new command controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CommandController( Antares game, HUDView view )
            : base( game, view ) {
            this.currentMode = CommandMode.NORMAL;

            HUDButton toMenuButton = new HUDButton( "Menu", new Vector2( 0.1f, 0.9f ), new Vector2( 0.1f, 0.05f ), 0.7f, this );
            toMenuButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.MENU );
            } );
            this.view.Add( toMenuButton );

            HUDButton undockButton = new HUDButton( "Undock", new Vector2( 0.9f, 0.9f ), new Vector2( 0.1f, 0.05f ), 0.7f, this );
            undockButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.DOCK );
            } );
            this.view.Add( undockButton );

            buildMenu = new BuildMenu( new Vector2( 0.9f, 0.5f ), 
                delegate() {
                    if ( this.currentMouseTexture != null ) {
                        this.currentMouseTexture.IsVisible = false;
                    }
                    this.currentMode = CommandMode.BUILD;
                } ,
                this);
            this.view.Add( buildMenu );

            mouseTextures = new Dictionary<Type, MouseTexture>();
            mouseTextures.Add( typeof( Battlestation_Antares.Model.Turret ), new MouseTexture( "Models//Turret//turret_2d", this ) );
            mouseTextures.Add( typeof( Battlestation_Antares.Model.Radar ), new MouseTexture( "Models//Radar//radar_2d", this ) );
            this.view.AddRange( mouseTextures.Values );

            mapConfig = new MiniMapRenderer.Config( new Vector2( 0.5f, 0.5f ), new Vector2( 0.625f, 1f ), new MiniMap.Config( 0.1f, Antares.world.spaceStation ) );
            Register( Antares.world.miniMapRenderer.miniMap );
        }

        public override void onEnter() {
            Antares.world.miniMapRenderer.changeConfig( this.mapConfig );
        }

        public override void onExit() {
            this.currentMode = CommandMode.NORMAL;
        }


        /// <summary>
        /// update the command controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( GameTime gameTime ) {
            base.Update( gameTime );
            Antares.world.Update( gameTime );

            if ( currentMode == CommandMode.BUILD ) {
                // activate mouse texture
                Type buildingType = buildMenu.getStructureType();
                this.mouseTextures[buildingType].IsVisible = true;
            }

            if ( currentMode == CommandMode.BUILD && Antares.inputProvider.isLeftMouseButtonPressed() ) {
                createNewStructure();
            }

            if ( currentMode == CommandMode.BUILD && Antares.inputProvider.isRightMouseButtonPressed() ) {
                Type structureType = buildMenu.getStructureType();
                this.mouseTextures[structureType].IsVisible = false;
                this.currentMode = CommandMode.NORMAL;
            }

        }

        private void createNewStructure() {
            Type structureType = buildMenu.getStructureType();
            Vector2 mousePos = Antares.inputProvider.getMousePos();
            if ( Antares.world.miniMapRenderer.Intersects( mousePos ) ) {
                SpatialObject newStructure = SpatialObjectFactory.buildSpatialObject( structureType );
                //Vector2 miniMapCoord = Antares.world.miniMap.screenToMiniMapCoord( Antares.inputProvider.getMousePos() );
                //newStructure.globalPosition = Antares.world.miniMap.miniMapToWorldCoord( miniMapCoord );
            }
        }

    }

}
