using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Mixer : AI_Item
    {

        public enum MixerType
        {
            AVG,
            MULTIPLY,
            MIN,
            MAX
        }

        public AI_Mixer(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.subType = MixerType.AVG;
            this.itemTypeName = "Mixer";
            this.typeString.String = this.itemTypeName;

            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort(AI_ItemPort.PortType.INPUT);
            AddPort(AI_ItemPort.PortType.OUTPUT);

            this.subTypeString.String = this.subType.ToString();
        }

    }

}
