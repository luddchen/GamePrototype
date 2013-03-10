using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Container : HUD2DContainer
    {

        public List<AI_Item> aiItems;

        public List<AI_Connection> aiConnections;

        private AI_Item moveItem;


        public AI_Container(Game1 game)
            : base(new Vector2(0, 0), HUDType.RELATIV, game)
        {
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();


            AI_Input ai_Item1 = new AI_Input(new Vector2(0.1f, 0.2f), HUDType.RELATIV, this.game);

            Add(ai_Item1);

            AI_Transformer ai_Item2 = new AI_Transformer(new Vector2(0.1f, 0.4f), HUDType.RELATIV, this.game);

            Add(ai_Item2);

            AI_Input ai_Item12 = new AI_Input(new Vector2(0.3f, 0.2f), HUDType.RELATIV, this.game);

            Add(ai_Item12);

            AI_Transformer ai_Item22 = new AI_Transformer(new Vector2(0.3f, 0.4f), HUDType.RELATIV, this.game);

            Add(ai_Item22);

            AI_Mixer ai_Item3 = new AI_Mixer(new Vector2(0.2f, 0.6f), HUDType.RELATIV, this.game);

            Add(ai_Item3);

            AI_Output ai_Item4 = new AI_Output(new Vector2(0.2f, 0.8f), HUDType.RELATIV, this.game);

            Add(ai_Item4);


            AI_Connection con1 = new AI_Connection(this.game);
            con1.setSource(ai_Item1.outputs[0]);
            con1.setTarget(ai_Item2.inputs[0]);

            Add(con1);

            AI_Connection con2 = new AI_Connection(this.game);
            con2.setSource(ai_Item12.outputs[0]);
            con2.setTarget(ai_Item22.inputs[0]);

            Add(con2);

            AI_Connection con3 = new AI_Connection(this.game);
            con3.setSource(ai_Item2.outputs[0]);
            con3.setTarget(ai_Item3.inputs[0]);

            Add(con3);

            AI_Connection con4 = new AI_Connection(this.game);
            con4.setSource(ai_Item22.outputs[0]);
            con4.setTarget(ai_Item3.inputs[1]);

            Add(con4);

            AI_Connection con5 = new AI_Connection(this.game);
            con5.setSource(ai_Item3.outputs[0]);
            con5.setTarget(ai_Item4.inputs[0]);

            Add(con5);

            foreach (AI_Item item in this.aiItems)
            {
                item.setLayerDepth(this.layerDepth);
                Console.Out.WriteLine("item layerdepth = " + item.layerDepth);
                Console.Out.WriteLine("item background layerdepth = " + item.background.layerDepth);
            }
        }


        public override void Add(HUD2D element)
        {
            if (element is AI_Item)
            {
                this.aiItems.Add((AI_Item)element);
            }
            base.Add(element);
        }


        public void Add(AI_Connection connection) 
        {
            this.aiConnections.Add(connection);
        }


        public void DrawConnections()
        {
            this.game.primitiveBatch.Begin(PrimitiveType.LineList);

            foreach (AI_Connection connection in this.aiConnections)
            {
                    connection.Draw(this.game.primitiveBatch);
            }

            this.game.primitiveBatch.End();
        }

        public void Update()
        {
            if (this.moveItem == null)
            {
                if (this.game.inputProvider.isLeftMouseButtonPressed())
                {
                    foreach (HUD2D item in this.allChilds)
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

    }

}
