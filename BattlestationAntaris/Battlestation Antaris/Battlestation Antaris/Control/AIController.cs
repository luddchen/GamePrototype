using System;
using System.Collections.Generic;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Control
{

    public class AIController : SituationController
    {

        public AIController(Game1 game, View.View view)
            : base(game, view)
        {
            HUD2DButton toMenuButton = new HUD2DButton("Menu", new Vector2(0.1f, 0.95f), 0.7f, this.game);
            toMenuButton.SetAction(delegate() { this.game.switchTo(Situation.MENU); });
            toMenuButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add(toMenuButton);
        }

    }

}
