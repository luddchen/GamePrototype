using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Item : HUD2DContainer
    {

        String itemTypeName;

        public List<AI_ItemPort> inputs;

        public List<AI_ItemPort> outputs;

        public HUD2DTexture background;

        public HUD2DString typeString;


        public AI_Item(Vector2 abstractPosition, HUDType positionType, String typeName, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.sizeType = HUDType.ABSOLUT;
            this.abstractSize = new Vector2(200, 100);

            this.itemTypeName = typeName;
            this.inputs = new List<AI_ItemPort>();
            this.outputs = new List<AI_ItemPort>();

            this.background = new HUD2DTexture(game);
            this.background.color = Color.DarkGreen;
            this.background.sizeType = this.sizeType;
            this.background.abstractSize = this.abstractSize;
            Add(this.background);

            this.typeString = new HUD2DString(typeName, game);
            this.typeString.positionType = this.sizeType;
            this.typeString.abstractPosition = new Vector2(0, -this.abstractSize.Y / 5);
            this.typeString.scale = 0.6f;
            Add(this.typeString);
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
                    portPosition.Y = -(this.abstractSize.Y / 2 + 3);
                    portOffset = this.abstractSize.X / (this.inputs.Count + 1);
                    foreach (AI_ItemPort port in this.inputs)
                    {
                        portPosition.X += portOffset;
                        port.abstractPosition = portPosition;
                    }
                    break;
                case AI_ItemPort.PortType.OUTPUT :
                    this.outputs.Add(newPort);
                    portPosition.Y = (this.abstractSize.Y / 2 + 3);
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

    }

}
