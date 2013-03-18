using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD.AIComposer;

namespace Battlestation_Antares.View {

    public class AIView : View {

        public AI_Container aiContainer;


        public override void Initialize() {
            this.aiContainer = new AI_Container();
            this.allHUD_2D.Add( this.aiContainer );
        }

        protected override void DrawPreContent() {
            this.aiContainer.Update();
        }


        protected override void DrawPostContent() {
            this.aiContainer.DrawConnections();
        }

    }

}
