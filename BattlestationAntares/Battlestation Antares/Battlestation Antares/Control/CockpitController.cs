using System;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD.CockpitHUD;
using Battlestation_Antaris.View.HUD.CockpitHUD;
using Battlestation_Antares.View;

namespace Battlestation_Antares.Control {

    /// <summary>
    /// the cockpit controller
    /// </summary>
    class CockpitController : SituationController {

        private int mouseTimeOut = 120;
        private int mouseVisibleCounter;

        private HUDButton toCommandButton;
        private HUDButton toMenuButton;
        private FpsDisplay fpsDisplay;

        private Compass compass;

        private TargetInfo targetInfo;

        private MiniMap.Config mapConfig;

        /// <summary>
        /// create a new cockpit controller
        /// </summary>
        /// <param name="game">the game</param>
        /// <param name="view">the used view</param>
        public CockpitController( Antares game, View.View view )
            : base( game, view ) {

            this.compass = new Compass();
            this.compass.Initialize( Antares.world.spaceShip );
            this.compass.abstractPosition = new Vector2( 0.5f, 0.15f );
            this.compass.positionType = HUDType.RELATIV;
            this.compass.abstractSize = new Vector2( 0.125f, 0.15f );
            this.compass.sizeType = HUDType.RELATIV;
            this.compass.layerDepth = 0.1f;
            this.compass.color = new Color( 192, 192, 64 );
            this.view.Add( this.compass );

            this.targetInfo = new TargetInfo( new Vector2( 60, 200 ), HUDType.ABSOLUT, new Vector2( 150, 60 ), HUDType.ABSOLUT );
            this.view.Add( this.targetInfo );

            mouseVisibleCounter = mouseTimeOut;

            HUDContainer buttons = new HUDContainer( new Vector2( 0.8f, 0.95f ), HUDType.RELATIV );

            toCommandButton = new HUDButton( "Command", new Vector2( 0, 0 ), 0.5f, this );

            this.toCommandButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.COMMAND );
            } );
            buttons.Add( toCommandButton );

            toMenuButton = new HUDButton( "Menu", new Vector2( toCommandButton.size.X + 10, 0 ), 0.5f, this );

            this.toMenuButton.SetPressedAction( delegate() {
                this.game.switchTo( Situation.MENU );
            } );
            buttons.Add( toMenuButton );
            this.view.Add( buttons );

            fpsDisplay = new FpsDisplay( new Vector2( 50, 20 ) );
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
            compass.Render();

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

            Antares.world.miniMap.ZoomOnMouseWheelOver();

            // redirect input
            Antares.world.spaceShip.InjectControl( Antares.inputProvider.getInput() );

        }

    }

}
