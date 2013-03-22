using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD.AIComposer;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View {

    public class AIView : View {

        public AI_Container aiContainer;


        public AIView( Color? backgroundColor ) : base( backgroundColor ) {
        }


        public override void Initialize() {

            this.aiContainer = new AI_Container();
            this.Add( this.aiContainer );

            HUDTexture bg = new HUDTexture();
            bg.abstractPosition = new Vector2( 0.41f, 0.5f );
            bg.positionType = HUDType.RELATIV;
            bg.abstractSize = new Vector2( 0.82f, 0.95f );
            bg.sizeType = HUDType.RELATIV;
            bg.Texture = Antares.content.Load<Texture2D>( "Sprites//builder_bg_temp" );
            bg.layerDepth = 1.0f;
            bg.color = new Color( 60, 64, 56);
            this.Add( bg );
        }

        protected override void DrawPreContent() {
            this.aiContainer.Update();
        }


        protected override void DrawPostContent() {
            this.aiContainer.DrawConnections();
        }

    }

}
