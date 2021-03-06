﻿using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD.CockpitHUD;
using Battlestation_Antaris.View.HUD.CockpitHUD;
using Battlestation_Antares.View;
using HUD.HUD;
using HUD;
using Battlestation_Antares.Model;
using System;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework.Input;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// the cockpit controller
    /// </summary>
    class CockpitController : SituationController {

        private int mouseTimeOut = 120;
        private int mouseVisibleCounter;

        DockButton dockButton;

        private FpsDisplay fpsDisplay;

        private Compass compass;

        private TargetInfo targetInfo;

        private MiniMapRenderer.Config mapConfig;

        private EngineControllVisualizer engineVisualizer;

        /// <summary>
        /// create a new cockpit controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CockpitController( Antares game, HUDView view )
            : base( game, view ) {

            this.compass = new Compass();
            this.compass.Initialize( Antares.world.spaceShip );
            this.compass.AbstractPosition = new Vector2( 0.5f, 0.15f );
            this.compass.PositionType = HUDType.RELATIV;
            this.compass.AbstractSize = new Vector2( 0.125f, 0.15f );
            this.compass.SizeType = HUDType.RELATIV;
            this.compass.LayerDepth = 0.1f;
            this.compass.color = new Color( 192, 192, 64 );
            this.view.Add( this.compass );

            this.targetInfo = new TargetInfo( new Vector2( 0.3f, 0.03f ), new Vector2( 0.25f, 0.025f ) );
            this.view.Add( this.targetInfo );

            mouseVisibleCounter = mouseTimeOut;

            this.dockButton = new DockButton( new Vector2( 0.225f, 0.925f ) );
            this.view.Add( this.dockButton );
            Register( this.dockButton );

            fpsDisplay = new FpsDisplay( new Vector2( 0.125f, 0.03f ) );
            this.view.Add( fpsDisplay );

            mapConfig = new MiniMapRenderer.Config( new Vector2( 0.5f, 0.83f ), new Vector2( 0.2f, 0.3125f ), new MiniMap.Config( 0.3f, Antares.world.spaceShip ) );
            Register( Antares.world.miniMapRenderer.miniMap );

            this.engineVisualizer = new EngineControllVisualizer( new Vector2( 0.7125f, 0.825f ), new Vector2( 0.2f, 0.325f ), this );
            this.view.Add( this.engineVisualizer );
        }

        public override void onEnter() {
            Antares.world.miniMapRenderer.changeConfig( this.mapConfig );
        }

        public override void onExit() {
            this.dockButton.Deactivate();
        }


        /// <summary>
        /// update the cockpit controller
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );
            Antares.world.Update( gameTime );

            // init compass
            if ( this.targetInfo.target != null ) {
                ( (CockpitView)this.view ).target = this.targetInfo.target;
                this.compass.target = this.targetInfo.target.globalPosition;
            } else {
                ( (CockpitView)this.view ).target = null;
                this.compass.target = Antares.world.spaceStation.globalPosition;
            }
            compass.Update( gameTime );

            fpsDisplay.Update( gameTime );

            if ( this.engineVisualizer.isActive() ) {
                this.engineVisualizer.use();
            } else {
                if ( Antares.inputProvider.isMouseMoved() ) {
                    mouseVisibleCounter = mouseTimeOut;
                    this.game.IsMouseVisible = true;
                } else {
                    if ( mouseVisibleCounter > 0 ) {
                        mouseVisibleCounter--;
                    } else {
                        this.game.IsMouseVisible = false;
                    }
                }
            }

            SpaceShip ship = Antares.world.spaceShip;
            float distance = Vector3.Distance( ship.globalPosition, Antares.world.spaceStation.CurrentDockPosition );

            if ( this.dockButton.isActivated() ) {
                if ( Antares.world.spaceStation.CurrentDockState != SpaceStation.DockState.OPEN ) {
                    Antares.world.spaceStation.OpenDock( 0 );
                }

                if ( distance < 10 && Math.Abs(ship.attributes.Engine.CurrentVelocity) < 0.5f) {
                    this.game.switchTo( Situation.DOCK );
                }

                ( (CockpitView)this.view ).SetBeamParameter(
                                ship.globalPosition,
                                ship.rotation.Forward * 500,
                                Antares.world.spaceStation.CurrentDockPosition,
                                Antares.world.spaceStation.rotation.Forward * 1);
                ( (CockpitView)this.view ).drawBeam = true;

            } else {
                ( (CockpitView)this.view ).drawBeam = false;
                if ( Antares.world.spaceStation.CurrentDockState != SpaceStation.DockState.CLOSED && distance > 50) {
                    Antares.world.spaceStation.CloseDock( 0 );
                }
            }

            // redirect input
            Antares.world.spaceShip.InjectControl( Antares.inputProvider.getInput() );

        }

    }

}
