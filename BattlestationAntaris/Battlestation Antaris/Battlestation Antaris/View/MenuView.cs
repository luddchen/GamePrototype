using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD.AIComposer;
using System;

namespace Battlestation_Antaris.View
{

    /// <summary>
    /// the menu view
    /// </summary>
    class MenuView : View
    {

        private AI_Item moveItem;

        /// <summary>
        /// creates a new menu view
        /// </summary>
        /// <param name="game">the game</param>
        public MenuView(Game1 game)
            : base(game)
        {
        }
        
                /// <summary>
        /// draw the view content
        /// </summary>
        protected override void DrawPreContent()
        {
            // dirty test code
            if (this.moveItem == null)
            {
                if (this.game.inputProvider.isLeftMouseButtonPressed())
                {
                    foreach (HUD2D item in this.allHUD_2D)
                    {
                        if (item is AI_Item)
                        {
                            if (((AI_Item)item).Intersects(this.game.inputProvider.getMousePos()))
                            {
                                this.moveItem = (AI_Item)item;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                if (this.game.inputProvider.isLeftMouseButtonDown())
                {
                    this.moveItem.position = this.game.inputProvider.getMousePos();
                    switch (this.moveItem.positionType)
                    {
                        case HUDType.ABSOLUT:
                            this.moveItem.abstractPosition = this.moveItem.position;
                            break;
                        case HUDType.RELATIV:
                            this.moveItem.abstractPosition.X = this.moveItem.position.X / this.game.GraphicsDevice.Viewport.Width;
                            this.moveItem.abstractPosition.Y = this.moveItem.position.Y / this.game.GraphicsDevice.Viewport.Height;
                            break;
                    }
                    this.moveItem.ClientSizeChanged();
                }
                else
                {
                    this.moveItem = null;
                }
            }
        }

        /// <summary>
        /// draw the view post content
        /// </summary>
        protected override void DrawPostContent()
        {
            this.game.primitiveBatch.Begin(Microsoft.Xna.Framework.Graphics.PrimitiveType.LineList);

            foreach (HUD2D item in this.allHUD_2D)
            {
                if (item is AI_Connection)
                {
                    ((AI_Connection)item).Draw(this.game.primitiveBatch);
                }
            }

            this.game.primitiveBatch.End();
        }


        /// <summary>
        /// initialize menu view HUD and content
        /// </summary>
        public override void Initialize()
        {
            // test content
            HUD2DTexture testTex = new HUD2DTexture(this.game);
            testTex.abstractPosition = new Vector2(0.5f, 0.4f);
            testTex.positionType = HUDType.RELATIV;
            testTex.abstractSize = new Vector2(1f, 1f);
            testTex.sizeType = HUDType.RELATIV;
            testTex.Texture = game.Content.Load<Texture2D>("Sprites//battlestation");
            testTex.layerDepth = 1.0f;

            this.allHUD_2D.Add(testTex);

            HUD2DString testString = new HUD2DString("Antaris Menu", this.game);
            testString.abstractPosition = new Vector2(0.5f, 0.1f);
            testString.positionType = HUDType.RELATIV;

            this.allHUD_2D.Add(testString);


            AI_Input ai_Item1 = new AI_Input(new Vector2(0.1f, 0.2f), HUDType.RELATIV, this.game);

            this.allHUD_2D.Add(ai_Item1);

            AI_Transformer ai_Item2 = new AI_Transformer(new Vector2(0.1f, 0.4f), HUDType.RELATIV, this.game);

            this.allHUD_2D.Add(ai_Item2);

            AI_Input ai_Item12 = new AI_Input(new Vector2(0.3f, 0.2f), HUDType.RELATIV, this.game);

            this.allHUD_2D.Add(ai_Item12);

            AI_Transformer ai_Item22 = new AI_Transformer(new Vector2(0.3f, 0.4f), HUDType.RELATIV, this.game);

            this.allHUD_2D.Add(ai_Item22);

            AI_Mixer ai_Item3 = new AI_Mixer(new Vector2(0.2f, 0.6f), HUDType.RELATIV, this.game);

            this.allHUD_2D.Add(ai_Item3);

            AI_Output ai_Item4 = new AI_Output(new Vector2(0.2f, 0.8f), HUDType.RELATIV, this.game);

            this.allHUD_2D.Add(ai_Item4);


            AI_Connection con1 = new AI_Connection(this.game);
            con1.setSource(ai_Item1.outputs[0]);
            con1.setTarget(ai_Item2.inputs[0]);

            this.allHUD_2D.Add(con1);

            AI_Connection con2 = new AI_Connection(this.game);
            con2.setSource(ai_Item12.outputs[0]);
            con2.setTarget(ai_Item22.inputs[0]);

            this.allHUD_2D.Add(con2);

            AI_Connection con3 = new AI_Connection(this.game);
            con3.setSource(ai_Item2.outputs[0]);
            con3.setTarget(ai_Item3.inputs[0]);

            this.allHUD_2D.Add(con3);

            AI_Connection con4 = new AI_Connection(this.game);
            con4.setSource(ai_Item22.outputs[0]);
            con4.setTarget(ai_Item3.inputs[1]);

            this.allHUD_2D.Add(con4);

            AI_Connection con5 = new AI_Connection(this.game);
            con5.setSource(ai_Item3.outputs[0]);
            con5.setTarget(ai_Item4.inputs[0]);

            this.allHUD_2D.Add(con5);

        }

    }

}
