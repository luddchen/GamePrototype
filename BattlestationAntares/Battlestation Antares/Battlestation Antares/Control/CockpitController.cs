using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD.CockpitHUD;
using Battlestation_Antaris.View.HUD.CockpitHUD;
using Battlestation_Antares.View;
using HUD.HUD;
using HUD;
using Battlestation_Antares.Model;
using System;

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

        private MiniMap.Config mapConfig;

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

            this.targetInfo = new TargetInfo( new Vector2( 0.4f, 0.03f ), HUDType.RELATIV, new Vector2( 0.125f, 0.025f ), HUDType.RELATIV );
            this.view.Add( this.targetInfo );

            mouseVisibleCounter = mouseTimeOut;

            HUDArray buttons = new HUDArray( new Vector2( 0.8f, 0.95f ),  new Vector2(0.1f, 0.04f) );
            buttons.direction = LayoutDirection.HORIZONTAL;
            buttons.borderSize = new Vector2( 0.01f, 0 );

            HUDButton easyDockButton = new HUDButton( "Dock", new Vector2(), 0.9f, this );

            easyDockButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.DOCK );
            } );
            buttons.Add( easyDockButton );

            this.view.Add( buttons );

            this.dockButton = new DockButton( new Vector2( 0.225f, 0.925f ) );
            this.view.Add( this.dockButton );
            Register( this.dockButton );

            fpsDisplay = new FpsDisplay( new Vector2( 0.125f, 0.03f ) );
            this.view.Add( fpsDisplay );

            mapConfig = new MiniMap.Config( new Vector2( 0.5f, 0.91f ), new Vector2( 0.25f, 0.18f ), new Vector2( 0.25f, 0.18f ), Antares.world.spaceShip );
        }

        public override void onEnter() {
            Antares.world.miniMap.changeConfig( this.mapConfig );
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

            if ( this.dockButton.isActivated() ) {
                SpaceShip ship = Antares.world.spaceShip;
                float distance = Vector3.Distance( ship.globalPosition, Antares.world.spaceStation.AirlockCurrentPosition );
                ( (CockpitView)this.view ).SetBeamParameter(
                                ship.globalPosition,
                                ship.rotation.Forward * 500,
                                Antares.world.spaceStation.AirlockCurrentPosition,
                                Antares.world.spaceStation.rotation.Forward * 1);
                ( (CockpitView)this.view ).drawBeam = true;

            } else {
                ( (CockpitView)this.view ).drawBeam = false;
            }

            Antares.world.miniMap.ZoomOnMouseWheelOver();

            // redirect input
            Antares.world.spaceShip.InjectControl( Antares.inputProvider.getInput() );

        }

    }

}
