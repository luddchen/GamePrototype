using Battlestation_Antares.View.HUD.AIComposer;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View {

    public class AIView : View {

        private AI_Container aiContainer;

        public AIView( Color? backgroundColor ) : base( backgroundColor ) {
        }


        public override void Initialize() {
            HUDTexture bg = new HUDTexture();
            bg.AbstractPosition = new Vector2( 0.41f, 0.5f );
            bg.PositionType = HUDType.RELATIV;
            bg.AbstractSize = new Vector2( 0.82f, 0.95f );
            bg.SizeType = HUDType.RELATIV;
            bg.Texture = Antares.content.Load<Texture2D>( "Sprites//builder_bg_temp" );
            bg.LayerDepth = 1.0f;
            bg.color = new Color( 60, 64, 56);
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
