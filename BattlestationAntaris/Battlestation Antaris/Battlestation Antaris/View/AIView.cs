using System;
using System.Collections.Generic;
using Battlestation_Antaris.View.HUD.AIComposer;

namespace Battlestation_Antaris.View
{

    public class AIView : View
    {

        public AI_Container aiContainer;

        public AIView(Game1 game)
            : base(game)
        {
        }

        public override void Initialize()
        {
            this.aiContainer = new AI_Container(this.game);
            this.allHUD_2D.Add(this.aiContainer);
        }

        protected override void DrawPreContent()
        {
            this.aiContainer.Update();
        }


        protected override void DrawPostContent()
        {
            this.aiContainer.DrawConnections();
        }

    }

}
