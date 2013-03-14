using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Item : HUD2DContainer
    {
        public AI_Container container;

        protected Object subType;

        public static int PORT_OFFSET = 4;

        public List<AI_ItemPort> inputs;

        public List<AI_ItemPort> outputs;

        public HUD2DTexture background;

        public HUD2DString typeString;

        protected HUD2DString subTypeString;

        public float[] parameters;

        private HUD2DButton nextSubType;

        private HUD2DButton previousSubType;

        private HUD2DButton removeButton;


        public AI_Item(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.sizeType = HUDType.ABSOLUT;
            this.abstractSize = new Vector2(200, 100);

            this.inputs = new List<AI_ItemPort>();
            this.outputs = new List<AI_ItemPort>();

            this.background = new HUD2DTexture(game);
            this.background.color = new Color(0 ,48, 0);
            this.background.sizeType = this.sizeType;
            this.background.abstractSize = this.abstractSize;
            Add(this.background);

            this.typeString = new HUD2DString("X", game);
            this.typeString.positionType = this.sizeType;
            this.typeString.scale = 0.6f;
            this.typeString.abstractPosition = new Vector2(0, -(this.abstractSize.Y - this.typeString.size.Y) / 2);
            Add(this.typeString);

            this.removeButton = new HUD2DButton("X", new Vector2(this.abstractSize.X / 2 - 8, -(this.abstractSize.Y / 2) + 8), 0.5f, game);
            this.removeButton.positionType = this.sizeType;
            this.removeButton.style = ButtonStyle.RemoveButtonStyle();
            this.removeButton.SetAction(delegate() { Destroy(); });
            Add(this.removeButton);

            this.subTypeString = new HUD2DString(" ", game);
            this.subTypeString.scale = 0.5f;
            this.subTypeString.positionType = this.sizeType;
            this.subTypeString.abstractSize = new Vector2(this.abstractSize.X, this.subTypeString.size.Y);
            this.subTypeString.abstractPosition = new Vector2(0, -(this.abstractSize.Y - this.typeString.size.Y * 3) / 2);
            Add(this.subTypeString);

            this.nextSubType = new HUD2DButton(">", Vector2.Zero, 0.8f, game);
            this.nextSubType.positionType = this.sizeType;
            this.nextSubType.abstractPosition = new Vector2((this.abstractSize.X - this.nextSubType.size.X) / 2 - 2, -(this.abstractSize.Y - this.typeString.size.Y * 3) / 2);
            this.nextSubType.SetAction(delegate() { switchToNextSubType(); });
            Add(this.nextSubType);

            this.previousSubType = new HUD2DButton("<", Vector2.Zero, 0.8f, game);
            this.previousSubType.positionType = this.sizeType;
            this.previousSubType.abstractPosition = new Vector2(-(this.abstractSize.X - this.nextSubType.size.X) / 2 + 2, -(this.abstractSize.Y - this.typeString.size.Y * 3) / 2);
            this.previousSubType.SetAction(delegate() { switchToPreviousSubType(); });
            Add(this.previousSubType);
        }


        public void AddPort(AI_ItemPort.PortType portType)
        {
            AI_ItemPort newPort = new AI_ItemPort(Vector2.Zero, HUDType.ABSOLUT, portType, this, this.game);

            Vector2 portPosition = new Vector2(-this.abstractSize.X / 2, 0);
            float portOffset = 0;

            switch (portType)
            {
                case AI_ItemPort.PortType.INPUT :
                    this.inputs.Add(newPort);
                    portPosition.Y = -(this.abstractSize.Y / 2 + PORT_OFFSET);
                    portOffset = this.abstractSize.X / (this.inputs.Count + 1);
                    foreach (AI_ItemPort port in this.inputs)
                    {
                        portPosition.X += portOffset;
                        port.abstractPosition = portPosition;
                    }
                    break;
                case AI_ItemPort.PortType.OUTPUT :
                    this.outputs.Add(newPort);
                    portPosition.Y = (this.abstractSize.Y / 2 + PORT_OFFSET);
                    portOffset = this.abstractSize.X / (this.outputs.Count + 1);
                    foreach (AI_ItemPort port in this.outputs)
                    {
                        portPosition.X += portOffset;
                        port.abstractPosition = portPosition;
                    }
                    break;
            }

            Add(newPort);
        }


        public override void setLayerDepth(float layerDepth)
        {
            base.setLayerDepth(layerDepth);
            foreach (AI_ItemPort port in this.inputs)
            {
                port.setLayerDepth(this.layerDepth + 0.01f);
            }
            foreach (AI_ItemPort port in this.outputs)
            {
                port.setLayerDepth(this.layerDepth + 0.01f);
            }
            this.background.setLayerDepth(this.layerDepth);
        }


        protected virtual void switchToNextSubType()
        {
            if (this.subType is Enum)
            {
                Type type = this.subType.GetType();

                int oldSubType = (int)this.subType;
                Array subTypes = Enum.GetValues(type);
                Object newSubType = null;
                if (oldSubType + 1 < subTypes.Length)
                {
                    newSubType = subTypes.GetValue(oldSubType + 1);
                }
                else
                {
                    newSubType = subTypes.GetValue(0);
                }
                SetSubType(newSubType);
            }
        }


        protected virtual void switchToPreviousSubType()
        {
            if (this.subType is Enum)
            {
                Type type = this.subType.GetType();

                int oldSubType = (int)this.subType;
                Array subTypes = Enum.GetValues(type);
                Object newSubType = null;
                if (oldSubType - 1 >= 0)
                {
                    newSubType = subTypes.GetValue(oldSubType - 1);
                }
                else
                {
                    newSubType = subTypes.GetValue(subTypes.Length - 1);
                }
                SetSubType(newSubType);
            }
        }

        public virtual void SetSubType(Object subType)
        {
            this.subType = subType;
            this.subTypeString.String = this.subType.ToString().Replace('_', ' ');
        }

        public Object GetSubType()
        {
            return this.subType;
        }


        private void Destroy() 
        {
            foreach (AI_ItemPort port in this.inputs)
            {
                while(port.connections.Count > 0)
                {
                    this.container.aiConnections.Remove(port.connections[0]);
                    port.connections[0].Delete();
                }
                port.connections.Clear();
            }

            foreach (AI_ItemPort port in this.outputs)
            {
                while (port.connections.Count > 0)
                {
                    this.container.aiConnections.Remove(port.connections[0]);
                    port.connections[0].Delete();
                }
                port.connections.Clear();
            }

            this.container.removeList.Add(this);

        }

    }

}
