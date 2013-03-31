using Battlestation_Antares;
using Battlestation_Antares.Control;
using HUD;
using HUD.HUD;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD {

    class SituationSwitch : HUDArray, IUpdatableItem {

        HUDButton menu;
        HUDButton command;
        HUDButton cockpit;
        HUDButton builder;

        public SituationSwitch(Antares game) : base(new Vector2(0.975f, 0.1f), new Vector2(0.05f, 0.1f) ) {
            
            menu = new HUDButton( "Menu", scale: 0.8f);
            command = new HUDButton( "Command", scale: 0.8f );
            cockpit = new HUDButton( "Cockpit", scale: 0.8f );
            builder = new HUDButton( "Editor", scale: 0.8f );

            Add( menu );
            Add( command );
            Add( cockpit );
            Add( builder );

            menu.SetPressedAction(
                delegate() {
                    game.switchTo( Situation.MENU );
                });

            command.SetPressedAction(
                delegate() {
                    game.switchTo( Situation.COMMAND );
                } );

            cockpit.SetPressedAction(
                delegate() {
                    game.switchTo( Situation.COCKPIT );
                } );

            builder.SetPressedAction(
                delegate() {
                    game.switchTo( Situation.AI_BUILDER );
                } );

            this.LayerDepth = 0.1f;
        }


        public void Update( GameTime gameTime ) {
            menu.Update( gameTime );
            command.Update( gameTime );
            cockpit.Update( gameTime );
            builder.Update( gameTime );
        }

        public bool Enabled {
            get {
                return true;
            }
        }
    }

}
