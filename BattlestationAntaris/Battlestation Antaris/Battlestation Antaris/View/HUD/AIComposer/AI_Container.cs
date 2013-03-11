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
        private Vector2 moveOffset;

        private AI_Connection moveConnection;
        private AI_ItemPort movePort;

        public List<HUD2D> removeList;


        public AI_Container(Game1 game)
            : base(new Vector2(0, 0), HUDType.RELATIV, game)
        {
            this.removeList = new List<HUD2D>();
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();

            AI_Input ai_Item1 = new AI_Input(new Vector2(0.4f, 0.3f), HUDType.RELATIV, this.game);
            Add(ai_Item1);

            AI_Output ai_Item2 = new AI_Output(new Vector2(0.4f, 0.7f), HUDType.RELATIV, this.game);
            Add(ai_Item2);

            AI_Connection con1 = new AI_Connection(this.game);
            con1.setSource(ai_Item1.outputs[0]);
            con1.setTarget(ai_Item2.inputs[0]);

            Add(con1);

        }


        public override void Add(HUD2D element)
        {
            if (element is AI_Item)
            {
                ((AI_Item)element).container = this;
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
            foreach (HUD2D item in this.removeList)
            {
                Remove(item);
            }
            this.removeList.Clear();

            if (isMouseInBuildingBox())
            {
                if (this.moveItem == null)
                {
                    if (this.game.inputProvider.isLeftMouseButtonPressed())
                    {
                        foreach (HUD2D item in this.allChilds)
                        {
                            if (item is AI_Item)
                            {
                                if (((AI_Item)item).typeString.Intersects(this.game.inputProvider.getMousePos()))
                                {
                                    this.moveItem = (AI_Item)item;
                                    this.moveOffset = item.position - this.game.inputProvider.getMousePos();
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
                        this.moveItem.position = this.game.inputProvider.getMousePos() + this.moveOffset;
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

                AI_ItemPort port = getMouseOverPort();
                if (port != null)
                {
                    if (this.game.inputProvider.isLeftMouseButtonPressed())
                    {

                        if (this.moveConnection != null)
                        {
                            Console.WriteLine(port.portType + " , " + this.movePort.portType);

                            if (port.portType == this.movePort.portType)
                            {
                                if (port.portType == AI_ItemPort.PortType.INPUT && port.connections.Count > 0)
                                {
                                    AI_Connection old = port.connections[0];
                                    old.setSource(null);
                                    old.setTarget(null);
                                    this.aiConnections.Remove(old);
                                }

                                port.Add(this.moveConnection);
                                if (this.moveConnection.getTarget() != null 
                                    && this.moveConnection.getSource() != null 
                                    && this.moveConnection.getTarget().item == this.moveConnection.getSource().item)
                                {
                                    this.moveConnection.setTarget(null);
                                    this.moveConnection.setSource(null);
                                    this.aiConnections.Remove(this.moveConnection);
                                }
                                Remove(this.movePort);
                                this.movePort = null;
                                this.moveConnection = null;
                            }
                        }
                        else
                        {
                            this.moveConnection = new AI_Connection(this.game);

                            AI_ItemPort.PortType portType = (port.portType == AI_ItemPort.PortType.INPUT) ? AI_ItemPort.PortType.OUTPUT : AI_ItemPort.PortType.INPUT;
                            this.movePort = new AI_ItemPort(this.game.inputProvider.getMousePos(), HUDType.ABSOLUT, portType, this.game);

                            port.Add(this.moveConnection);
                            this.movePort.Add(this.moveConnection);

                            Add(this.movePort);
                            this.aiConnections.Add(this.moveConnection);
                        }

                    }
                }
                else
                {
                    if (this.moveConnection != null)
                    {
                        if (this.game.inputProvider.isLeftMouseButtonPressed())
                        {
                            this.moveConnection.setTarget(null);
                            this.moveConnection.setSource(null);

                            this.aiConnections.Remove(this.moveConnection);
                            Remove(this.movePort);

                            this.moveConnection = null;
                            this.movePort = null;
                        }
                    }
                }

                if (this.moveConnection != null)
                {
                    this.movePort.position = this.game.inputProvider.getMousePos();
                }

            }
        }


        private AI_ItemPort getMouseOverPort()
        {

            foreach (AI_Item item in this.aiItems)
            {
                foreach (AI_ItemPort port in item.inputs)
                {
                    if (port.Intersects(this.game.inputProvider.getMousePos()))
                    {
                        port.color = Color.Green;
                        return port;
                    }
                    else
                    {
                        port.color = Color.White;
                    }
                }

                foreach (AI_ItemPort port in item.outputs)
                {
                    if (port.Intersects(this.game.inputProvider.getMousePos()))
                    {
                        port.color = Color.Green;
                        return port;
                    }
                    else
                    {
                        port.color = Color.White;
                    }
                }
            }

            return null;
        }


        private bool isMouseInBuildingBox()
        {
            bool isWithin = true;
            Vector2 mousePos = this.game.inputProvider.getMousePos();
            if (mousePos.X < this.game.GraphicsDevice.Viewport.Width * 0.05f
                || mousePos.X > this.game.GraphicsDevice.Viewport.Width * 0.75f
                || mousePos.Y < this.game.GraphicsDevice.Viewport.Height * 0.05f
                || mousePos.Y > this.game.GraphicsDevice.Viewport.Height * 0.95f)
            {
                isWithin = false;
            }

            return isWithin;
        }

    }

}
