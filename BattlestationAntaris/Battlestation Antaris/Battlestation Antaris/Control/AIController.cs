using System;
using System.Collections.Generic;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.View;
using Battlestation_Antaris.View.HUD.AIComposer;
using Battlestation_Antaris.Control.AI;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.Control
{

    public class AIController : SituationController
    {

        public AIController(Game1 game, View.View view)
            : base(game, view)
        {
            HUD2DButton toMenuButton = new HUD2DButton("Menu", new Vector2(0.9f, 0.95f), 0.7f, this.game);
            toMenuButton.SetAction(delegate() { this.game.switchTo(Situation.MENU); });
            toMenuButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add(toMenuButton);
            this.worldUpdate = WorldUpdate.NO_UPDATE;

            HUD2DButton verifyButton = new HUD2DButton("Verify", new Vector2(0.9f, 0.8f), 0.8f, this.game);
            verifyButton.SetAction(
                delegate() 
                {
                    AI.AI ai = new AI.AI();
                    ai.Create(((AIView)this.view).aiContainer);
                    Console.WriteLine(ai);

                    foreach (Turret turret in this.game.world.allTurrets) 
                    {
                        turret.ai = new AI.AI(ai);
                        turret.ai.source = turret;
                    }
                });
            verifyButton.positionType = HUDType.RELATIV;
            this.view.allHUD_2D.Add(verifyButton);


            HUD2DArray addButtonArray = new HUD2DArray(new Vector2(0.9f, 0.5f), HUDType.RELATIV, new Vector2(250, 300), HUDType.ABSOLUT, game);
            addButtonArray.direction = LayoutDirection.VERTICAL;
            this.view.allHUD_2D.Add(addButtonArray);

            HUD2DButton addInputButton = new HUD2DButton("Input", new Vector2(0.9f, 0.3f), 0.7f, this.game);
            addInputButton.SetAction(delegate() { ((AIView)this.view).aiContainer.Add(new AI_Input(new Vector2(0.7f, 0.1f), HUDType.RELATIV, game)); });
            addInputButton.positionType = HUDType.RELATIV;
            addButtonArray.Add(addInputButton);


            HUD2DButton addTransformerButton = new HUD2DButton("Transformer", new Vector2(0.9f, 0.4f), 0.7f, this.game);
            addTransformerButton.SetAction(delegate() { ((AIView)this.view).aiContainer.Add(new AI_Transformer(new Vector2(0.7f, 0.1f), HUDType.RELATIV, game)); });
            addTransformerButton.positionType = HUDType.RELATIV;
            addButtonArray.Add(addTransformerButton);


            HUD2DButton addMixerButton = new HUD2DButton("Mixer", new Vector2(0.9f, 0.5f), 0.7f, this.game);
            addMixerButton.SetAction(delegate() { ((AIView)this.view).aiContainer.Add(new AI_Mixer(new Vector2(0.7f, 0.1f), HUDType.RELATIV, game)); });
            addMixerButton.positionType = HUDType.RELATIV;
            addButtonArray.Add(addMixerButton);


            HUD2DButton addOutputButton = new HUD2DButton("Output", new Vector2(0.9f, 0.6f), 0.7f, this.game);
            addOutputButton.SetAction(delegate() { ((AIView)this.view).aiContainer.Add(new AI_Output(new Vector2(0.7f, 0.1f), HUDType.RELATIV, game)); });
            addOutputButton.positionType = HUDType.RELATIV;
            addButtonArray.Add(addOutputButton);
        }

    }

}
