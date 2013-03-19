using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD.AIComposer;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View {

    public class AIView : View {

        public AI_Container aiContainer;


        public override void Initialize() {
            this.aiContainer = new AI_Container();
            this.allHUD_2D.Add( this.aiContainer );

            HUD2DTexture bg = new HUD2DTexture();
            bg.abstractPosition = new Vector2( 0.5f, 0.5f );
            bg.positionType = HUDType.RELATIV;
            bg.abstractSize = new Vector2( 1, 1 );
            bg.sizeType = HUDType.RELATIV;
            bg.Texture = Antares.content.Load<Texture2D>( "Sprites//builder_bg_temp" );
            bg.layerDepth = 1.0f;
            bg.color = new Color( 84, 88, 80 );
            this.allHUD_2D.Add( bg );
        }

        protected override void DrawPreContent() {
            this.aiContainer.Update();
        }


        protected override void DrawPostContent() {
            this.aiContainer.DrawConnections();
        }

    }

}
