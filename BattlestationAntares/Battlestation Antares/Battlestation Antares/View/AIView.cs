using Battlestation_Antares.View.HUD.AIComposer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View {

    public class AIView : HUDView {

        private AI_Container aiContainer;

        public AIView( Color? backgroundColor ) : base( backgroundColor ) {
        }


        public override void Initialize() {
            HUDTexture bg = new HUDTexture( "Sprites//builder_bg_temp", new Vector2( 0.41f, 0.5f ), new Vector2( 0.82f, 1f ), new Color( 60, 64, 56 ), null, null );
            bg.LayerDepth = 1.0f;
            this.Add( bg );
        }

        public override void Add( HUD_Item item ) {
            base.Add( item );

            // dirty way to get post draw content (2nd draw pass)
            if ( item is AI_Container ) {
                this.aiContainer = (AI_Container)item;
            }
        }


        protected override void DrawPostContent() {
            if ( this.aiContainer != null ) {
                this.aiContainer.DrawConnections();
            }
        }

    }

}
