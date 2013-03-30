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
            
            menu = new HUDButton( "Menu", Vector2.Zero, 0.8f, null );
            command = new HUDButton( "Command", Vector2.Zero, 0.8f, null );
            cockpit = new HUDButton( "Cockpit", Vector2.Zero, 0.8f, null );
            builder = new HUDButton( "Editor", Vector2.Zero, 0.8f, null );

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
